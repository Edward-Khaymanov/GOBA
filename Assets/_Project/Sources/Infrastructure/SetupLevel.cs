using Cysharp.Threading.Tasks;
using GOBA.Assets._Project.Sources._Test;
using GOBA.Network;
using GOBA.CORE;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.AI.Navigation;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class SetupLevel : NetworkBehaviour
    {
        [SerializeField] public Hero _heroTemplate;
        [SerializeField] public Level _terrainTemplate;
        [SerializeField] public HeroSelectionMenu _heroSelectionMenu;
        [SerializeField] public MainCommandSender _mainCommandSender;

        private IProjectileManager _projectileManager;
        private IProjectileProvider _projectileProvider;
        private IParticleManager _particleManager;
        private AbilityProvider _abilityProvider;

        public string CurrentTime => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

        private Dictionary<ulong, int> _clientsSelectedHero;

        public async UniTask SetupGame(LobbyData lobbyData, bool test)
        {
            var projectileProvider = new ProjectileProvider();
            var addressablesProvider = new AddressablesProvider();
            await projectileProvider.Initialize(addressablesProvider);
            var projectileManager = new ProjectileManager(projectileProvider);
            var abilityProvider = new AbilityProvider();
            await abilityProvider.Initialize(addressablesProvider);

            _projectileProvider = projectileProvider;
            _projectileManager = projectileManager;
            _particleManager = null;
            _abilityProvider = abilityProvider;

            DevCheats.Init(DIContainer.EntityManager, _particleManager, _projectileManager, _projectileProvider, _abilityProvider);

            if (test)
            {
                //await TestSetup(lobbyData);
            }
            else
            {
                await Setup(lobbyData);
            }
        }

        private async UniTask Setup(LobbyData lobbyData)
        {
            MyLogger.Log("Setup");

            var terrain = Instantiate(_terrainTemplate);
            terrain.NetworkObject.Spawn(true);
            var commandSender = Instantiate(_mainCommandSender);
            commandSender.NetworkObject.Spawn(true);

            var targetClients = HELPERS.GetClientsRPC(lobbyData.SessionUsers.Select(x => x.NetworkUser.ClientId));
            await SelectionMenu(targetClients);
            await UniTask.NextFrame();
            await SpawnHeroes(terrain.Spawnpoints, lobbyData.SessionTeams);
            await UniTask.NextFrame();
            BakeLevelRpc(terrain.NetworkObject);
            await UniTask.NextFrame();
            OnGameStartedRpc();
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
                    await UniTask.NextFrame();
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

            //var rpcTarget = RpcTarget.Group(
            //                new ulong[]
            //                {
            //                    NetworkManager.ServerClientId,
            //                    ownerClientId,
            //                }, RpcTargetUse.Temp);

            hero.NetworkObject.SpawnWithOwnership(ownerClientId);
            await UniTask.NextFrame();
            heroModel.GetComponent<NetworkObject>().SpawnWithOwnership(ownerClientId);
            await UniTask.NextFrame();
            heroModel.GetComponent<NetworkObject>().TrySetParent(hero.NetworkBehaviour.NetworkObject, false);
            hero.Initialize(heroId, teamId);
            var ability = DevCheats.AddAbilityToUnit("Fireball", hero);
            ability.SetLevel(1);
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

            var playerCamera = FindAnyObjectByType<PlayerCamera>();
            var playerInput = FindAnyObjectByType<PlayerInput>();
            var playerUI = FindAnyObjectByType<PlayerUI>();
            //var input = new InputSystemHeroController(hero, playerCamera);

            playerCamera.Init();
            playerInput.Init(playerCamera, playerUI);
            playerInput.SelectUnits(new List<IUnit>() { hero });
        }

        [Rpc(SendTo.SpecifiedInParams)]
        public void SetCameraRpc(Vector3 newPosition, RpcParams rpcParams)
        {
            MyLogger.Log("SetCameraRpc");
            var camera = FindAnyObjectByType<PlayerCamera>();
            camera.transform.position = newPosition;
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void BakeLevelRpc(NetworkObjectReference terrainReference)
        {
            MyLogger.Log("BakeLevelRpc");
            terrainReference.TryGet(out NetworkObject terrainNetworkObject);
            terrainNetworkObject.TryGetComponent<NavMeshSurface>(out NavMeshSurface navMeshSurface);
            navMeshSurface.BuildNavMesh();
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void OnGameStartedRpc()
        {
            var playerInput = FindAnyObjectByType<PlayerInput>();
            playerInput.Enable();
        }
    }
}