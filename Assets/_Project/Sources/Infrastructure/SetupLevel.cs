using Cysharp.Threading.Tasks;
using GOBA.Network;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class SetupLevel : NetworkBehaviour
    {
        [SerializeField] public Hero _heroTemplate;
        [SerializeField] public Terrain _terrainTemplate;
        [SerializeField] public HeroSelectionMenu _heroSelectionMenu;

        public string CurrentTime => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

        private Dictionary<ulong, int> _clientsSelectedHero;

        public async UniTask SetupGame(LobbyData lobbyData)
        {
            //await Setup(lobbyData);
            await TestSetup(lobbyData);
        }

        private async UniTask TestSetup(LobbyData lobbyData)
        {
            var terrain = Instantiate(_terrainTemplate);
            terrain.NetworkObject.Spawn(true);

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

            SpawnTestUnit(CONSTANTS.NEUTRAL_TEAM_ID, -1, positions[0]);
            SpawnTestUnit(CONSTANTS.NEUTRAL_TEAM_ID, -1, positions[1]);
            SpawnTestUnit(1, -1, positions[2]);
            SpawnTestUnit(1, -1, positions[3]);
            SpawnTestUnit(2, -1, positions[4]);
            SpawnTestUnit(2, -1, positions[5]);
        }

        private void SpawnTestUnit(int teamId, int heroid, Vector3 spawnPosition)
        {
            var hero = Instantiate(_heroTemplate, spawnPosition, Quaternion.identity);
            hero.GetComponent<ClientNetworkTransform>().enabled = false;
            hero.GetComponent<NetworkObject>().SpawnWithOwnership(152);
            hero.Constructor(heroid, teamId);
            hero.ConstructorClientRPC(heroid, teamId, HELPERS.GetObserverRPC());
        }






        private async UniTask Setup(LobbyData lobbyData)
        {
            var terrain = Instantiate(_terrainTemplate);
            terrain.NetworkObject.Spawn(true);

            var targetClients = HELPERS.GetClientsRPC(lobbyData.SessionUsers.Select(x => x.NetworkUser.ClientId));
            await SelectionMenu(targetClients);
            SpawnHeroes(terrain.Spawnpoints, lobbyData.SessionTeams);
        }

        private void SpawnHeroes(List<TeamSpawnpoint> teamSpawnpoints, List<SessionTeam> lobbyTeams)
        {
            foreach (var team in lobbyTeams)
            {
                var spawnPosition = teamSpawnpoints.FirstOrDefault(x => x.Id == team.Id).transform.position;

                foreach (var sessionUser in team.Users)
                {
                    var heroId = _clientsSelectedHero[sessionUser.NetworkUser.ClientId];
                    var hero = Instantiate(_heroTemplate, spawnPosition, Quaternion.identity);
                    hero.GetComponent<NetworkObject>().SpawnWithOwnership(sessionUser.NetworkUser.ClientId);
                    hero.Constructor(heroId, team.Id);
                    hero.ConstructorClientRPC(heroId, team.Id, HELPERS.GetObserverRPC());

                    var targetClient = HELPERS.GetClientRPC(sessionUser.NetworkUser.ClientId);
                    InitLocalClientRpc(hero, targetClient);
                }
            }
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
            _clientsSelectedHero.Add(clientId, heroId);
        }

        [ClientRpc]
        public void InitLocalClientRpc(NetworkBehaviourReference heroReference, ClientRpcParams rpc)
        {
            var player = FindObjectOfType<Player>();
            var camera = FindObjectOfType<PlayerCamera>();

            heroReference.TryGet(out Unit hero);
            player.Init();
            player.AddUnit(hero);
            player.SelectUnit(hero);
            camera.transform.position = new Vector3(hero.transform.position.x, hero.transform.position.y + 20, hero.transform.position.z - 10);
        }
    }
}