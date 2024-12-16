using Cysharp.Threading.Tasks;
using GOBA.CORE;
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
        [SerializeField] public Level _terrainTemplate;
        [SerializeField] public HeroSelectionMenu _heroSelectionMenu;
        [SerializeField] public MainCommandSender _mainCommandSender;
        [SerializeField] public PlayerController _playerControllerTemplate;

        private IProjectileManager _projectileManager;
        private IProjectileProvider _projectileProvider;
        private IParticleManager _particleManager;
        private AbilityProvider _abilityProvider;

        private readonly Dictionary<UserID, PlayerController> _usersPlayers = new();
        private Dictionary<UserID, int> _usersSelectedHero = new();

        public string CurrentTime => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);


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

            ServerFunctions.Init(DIContainer.EntityManager, _particleManager, _projectileManager, _projectileProvider, _abilityProvider);

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

            var terrain = await SpawnTerrain();
            var commandSender = Instantiate(_mainCommandSender);
            commandSender.NetworkObject.Spawn(true);
            var targetClients = HELPERS.GetClientsRPC(lobbyData.SessionUsers.Select(x => x.NetworkUser.ClientId));

            await SetupPlayers(lobbyData.SessionUsers);
            await UniTask.NextFrame();
            await SelectionMenu(lobbyData.SessionUsers, targetClients);
            await UniTask.NextFrame();
            var usersHeroes = await SpawnHeroes(terrain.Spawnpoints, lobbyData.SessionTeams);
            await UniTask.NextFrame();
            StartGame(usersHeroes);
        }

        private void StartGame(Dictionary<SessionUser, Hero> usersHeroes)
        {
            foreach (var item in usersHeroes)
            {
                OnGameStartedRpc(item.Value.EntityId, RpcTarget.Single(item.Key.NetworkUser.ClientId, RpcTargetUse.Temp));
            }
        }

        private async UniTask SetupPlayers(IEnumerable<SessionUser> players)
        {
            var newId = 1;
            foreach (var sessionUser in players)
            {
                var playerController = GameObject.Instantiate(_playerControllerTemplate);
                playerController.NetworkObject.SpawnWithOwnership(sessionUser.NetworkUser.ClientId);
                playerController.NetworkObject.NetworkShow(sessionUser.NetworkUser.ClientId);
                playerController.SetPlayerId(newId);
                playerController.SetPlayerTeam(sessionUser.Team.Id);
                newId++;
                _usersPlayers.Add(sessionUser.UserId, playerController);
            }
        }

        private async UniTask<Level> SpawnTerrain()
        {
            var terrain = Instantiate(_terrainTemplate);
            terrain.NetworkObject.Spawn(true);
            return terrain;
        }

        private async UniTask<Dictionary<SessionUser, Hero>> SpawnHeroes(List<TeamSpawnpoint> teamSpawnpoints, List<SessionTeam> lobbyTeams)
        {
            var result = new Dictionary<SessionUser, Hero>();
            var positionOffset = Vector3.zero;

            foreach (var team in lobbyTeams)
            {
                var spawnPosition = teamSpawnpoints.FirstOrDefault(x => x.Id == team.Id).transform.position;
                foreach (var sessionUser in team.Users)
                {
                    spawnPosition += positionOffset;
                    positionOffset += Vector3.right;

                    var heroId = _usersSelectedHero[sessionUser.UserId];
                    var hero = await ServerFunctions.SpawnHero(heroId, team.Id, spawnPosition);
                    var ability = ServerFunctions.AddAbilityToUnit("Fireball", hero);
                    ability.SetLevel(1);
                    result.Add(sessionUser, hero);
                }
                positionOffset = Vector3.zero;
            }

            return result;
        }

        private async UniTask SelectionMenu(List<SessionUser> sessionUsers, ClientRpcParams targetClients)
        {
            foreach (var sessionUser in sessionUsers)
            {
                _usersSelectedHero.TryAdd(sessionUser.UserId, 1);//YBOT
            }
            //_clientsSelectedHero = new Dictionary<ulong, int>();

            //var clientsId = targetClients.Send.TargetClientIds;
            //var menu = Instantiate(_heroSelectionMenu);
            //menu.ClientHeroSelected += OnClientSelectedHero;
            //menu.NetworkObject.Spawn();
            //menu.StartSelecting(targetClients);
            //await UniTask.WaitWhile(() => clientsId.Except(_clientsSelectedHero.Keys).Any());
            //menu.ClientHeroSelected -= OnClientSelectedHero;
            //menu.NetworkObject.Despawn();
        }

        //private void OnClientSelectedHero(ulong clientId, int heroId)
        //{
        //    _usersSelectedHero.TryAdd(clientId, heroId);
        //}


        [Rpc(SendTo.SpecifiedInParams)]
        private void OnGameStartedRpc(int unitEnityId, RpcParams rpcParams)
        {
            PlayerLocalDependencies.Init();
            PlayerLocalDependencies.PlayerUI.Init();
            PlayerLocalDependencies.PlayerCamera.Init();
            PlayerLocalDependencies.PlayerInput.Init();

            var unit = DIContainer.EntityManager.GetUnit(unitEnityId);
            PlayerLocalDependencies.PlayerCamera.CenterCameraOnUnit(unit);
            PlayerLocalDependencies.PlayerInput.Enable();
            PlayerLocalDependencies.BarsRenderer.Enable();
            PlayerLocalDependencies.PlayerInput.SelectUnits(new List<IUnit>() { unit });
        }
    }
}