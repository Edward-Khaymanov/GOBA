using Cysharp.Threading.Tasks;
using GOBA.Network;
using MapModCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.AI.Navigation;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GOBA
{
    public class SetupLevel : NetworkBehaviour
    {
        [SerializeField] public Hero _heroTemplate;
        [SerializeField] public Level _terrainTemplate;
        [SerializeField] public HeroSelectionMenu _heroSelectionMenu;
        [SerializeField] public PlayerNetworkInput _playerNetworkInputTemplate;

        public string CurrentTime => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

        private Dictionary<ulong, int> _clientsSelectedHero;

        public async UniTask SetupGame(LobbyData lobbyData, bool test)
        {
            if (test)
            {
                await TestSetup(lobbyData);
            }
            else
            {
                await Setup(lobbyData);
            }
        }

        private async UniTask TestSetup(LobbyData lobbyData)
        {
            MyLogger.Log("TestSetup");

            var terrain = Instantiate(_terrainTemplate);
            terrain.NetworkObject.Spawn(true);
            //await UniTask.Delay(2000);

            _clientsSelectedHero = new Dictionary<ulong, int>();
            foreach (var item in lobbyData.SessionUsers)
            {
                _clientsSelectedHero.Add(item.NetworkUser.ClientId, 1);
            }

            SpawnHeroes(terrain.Spawnpoints, lobbyData.SessionTeams);

            var spawnPosition = terrain.Spawnpoints.FirstOrDefault(x => x.Id == 1).transform.position;
            SpawnTestUnits(spawnPosition + new Vector3(2, 0, 2));
        }

        private void SpawnTestUnits(Vector3 center)
        {
            var positions = new List<Vector3>();
            var ownerClientId = (ulong)152;

            for (int i = 0; i < 3; i++)
            {
                var pos = center;
                pos.x += i * 6;
                for (int k = 0; k < 2; k++)
                {
                    pos.z += k * 6;
                    positions.Add(pos);
                }
            }

            SpawnHero(CONSTANTS.NEUTRAL_TEAM_ID, -1, positions[0], ownerClientId);
            SpawnHero(CONSTANTS.NEUTRAL_TEAM_ID, -1, positions[1], ownerClientId);
            SpawnHero(1, -1, positions[2], ownerClientId);
            SpawnHero(1, -1, positions[3], ownerClientId);
            SpawnHero(2, -1, positions[4], ownerClientId);
            SpawnHero(2, -1, positions[5], ownerClientId);
        }

        private async UniTask Setup(LobbyData lobbyData)
        {
            MyLogger.Log("Setup");

            var terrain = Instantiate(_terrainTemplate);
            terrain.NetworkObject.Spawn(true);

            var targetClients = HELPERS.GetClientsRPC(lobbyData.SessionUsers.Select(x => x.NetworkUser.ClientId));
            await SelectionMenu(targetClients);
            await SpawnHeroes(terrain.Spawnpoints, lobbyData.SessionTeams);
            BakeLevelRpc(terrain.NetworkObject);
        }

        private async UniTask SpawnHeroes(List<TeamSpawnpoint> teamSpawnpoints, List<SessionTeam> lobbyTeams)
        {
            var positionOffset = Vector3.zero;
            foreach (var team in lobbyTeams)
            {
                var spawnPosition = teamSpawnpoints.FirstOrDefault(x => x.Id == team.Id).transform.position;
                foreach (var sessionUser in team.Users)
                {
                    spawnPosition += positionOffset;
                    positionOffset += Vector3.right;

                    var userClientRpc = RpcTarget.Single(sessionUser.NetworkUser.ClientId, RpcTargetUse.Temp);
                    var heroId = _clientsSelectedHero[sessionUser.NetworkUser.ClientId];
                    var hero = await SpawnHero(team.Id, heroId, spawnPosition, sessionUser.NetworkUser.ClientId);
                    var playerCameraPosition = new Vector3(hero.transform.position.x, hero.transform.position.y + 20, hero.transform.position.z - 10);

                    SetCameraRpc(playerCameraPosition, userClientRpc);
                    InitLocalClientRpc(hero, userClientRpc);
                }
                positionOffset = Vector3.zero;
            }
        }

        private async UniTask<Hero> SpawnHero(int teamId, int heroId, Vector3 spawnPosition, ulong ownerClientId)
        {
            MyLogger.Log("SpawnHero");

            var heroAsset = UnitAssetProvider.GetHero(heroId);
            var hero = Instantiate(_heroTemplate, spawnPosition, Quaternion.identity);
            var heroModel = Instantiate(heroAsset.Model);

            var rpcTarget = RpcTarget.Group(
                            new ulong[]
                            {
                                NetworkManager.ServerClientId,
                                ownerClientId,
                            }, RpcTargetUse.Temp);

            hero.NetworkObject.SpawnWithOwnership(ownerClientId);
            heroModel.GetComponent<NetworkObject>().SpawnWithOwnership(ownerClientId);
            heroModel.GetComponent<NetworkObject>().TrySetParent(hero.NetworkBehaviour.NetworkObject, false);
            hero.Initialize(heroId, teamId);

            return hero;
        }

        private async UniTask SelectionMenu(ClientRpcParams targetClients)
        {
            _clientsSelectedHero = new Dictionary<ulong, int>();

            var clientsId = targetClients.Send.TargetClientIds;
            var menu = Instantiate(_heroSelectionMenu);
            menu.ClientHeroSelected += OnClientSelectedHero;
            menu.NetworkObject.Spawn();
            menu.StartSelecting(targetClients);
            await UniTask.WaitWhile(() => clientsId.Except(_clientsSelectedHero.Keys).Any());
            menu.ClientHeroSelected -= OnClientSelectedHero;
            menu.NetworkObject.Despawn();
        }

        private void OnClientSelectedHero(ulong clientId, int heroId)
        {
            _clientsSelectedHero.TryAdd(clientId, heroId);
        }







        [Rpc(SendTo.SpecifiedInParams)]
        public void InitLocalClientRpc(NetworkBehaviourReference heroReference, RpcParams rpcParams)
        {
            MyLogger.Log("InitLocalClientRpc");

            heroReference.TryGet(out Hero hero);

            var player = FindObjectOfType<Player>();
            var playerCamera = FindObjectOfType<PlayerCamera>();
            var playerInput = FindObjectOfType<PlayerInput>();
            var playerUI = FindObjectOfType<PlayerUI>();
            var input = new InputSystemHeroController(hero, playerCamera);

            hero.InitLocal(input);
            playerCamera.Init();
            playerInput.Init(playerCamera, player);
            player.Init(playerUI);
            player.SelectUnit(hero);
        }

        [Rpc(SendTo.SpecifiedInParams)]
        public void SetCameraRpc(Vector3 newPosition, RpcParams rpcParams)
        {
            MyLogger.Log("SetCameraRpc");
            var camera = FindObjectOfType<PlayerCamera>();
            camera.transform.position = newPosition;
        }

        [Rpc(SendTo.NotServer)]
        public void BakeLevelRpc(NetworkObjectReference terrainReference)
        {
            MyLogger.Log("BakeLevelRpc");
            terrainReference.TryGet(out NetworkObject terrainNetworkObject);
            terrainNetworkObject.TryGetComponent<NavMeshSurface>(out NavMeshSurface navMeshSurface);
            navMeshSurface.BuildNavMesh();
        }
    }
}