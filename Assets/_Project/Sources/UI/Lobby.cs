using Cysharp.Threading.Tasks;
using GOBA.Network;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOBA
{
    public class Lobby : NetworkBehaviour
    {
        [SerializeField] private SetupLevel _setupLevelTemplate;

        private LobbyData _lobbyData;

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                this.NetworkObject.Spawn();
            }
        }

        private void Update()
        {
            if (base.IsServer)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    LoadScene();
                }
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (base.NetworkManager.IsServer)
            {
                base.NetworkManager.OnClientConnectedCallback += OnClientConnected;
                base.NetworkManager.SceneManager.OnLoadComplete += OnLoadedScene;
                Init();
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            if (base.NetworkManager.IsServer)
            {
                base.NetworkManager.OnClientConnectedCallback -= OnClientConnected;
                base.NetworkManager.SceneManager.OnLoadComplete -= OnLoadedScene;
            }
        }

        private void AuthenticateClient(ulong clientId)
        {
            //var sessionUser = _lobbyData.SessionUsers.FirstOrDefault(x => x.User.ClientId == clientId);
            //sessionUser.State = UserState.Authenticated;
            //AddUserToTeam(sessionUser, _lobbyData.SessionTeams.FirstOrDefault(x => x.Id == CONSTANTS.DEFAULT_TEAM_ID));
            //ClientAuthenticated?.Invoke(clientId);

            var randomId = Random.Range(1, 4);
            //var team = _lobbyData.SessionTeams.FirstOrDefault(x => x.Id == randomId);
            var team = _lobbyData.SessionTeams.FirstOrDefault(x => x.Id == 1);
            MyLogger.Log(team.Id);
            var netUser = new NetworkUser(clientId, System.Guid.NewGuid().ToString());
            var sessionUser = new SessionUser()
            {
                UserId = (ulong)Random.Range(1, int.MaxValue),
                NetworkUser = netUser,
                State = UserState.Authenticated,
            };

            _lobbyData.SessionUsers.Add(sessionUser);

            team.Users.Add(sessionUser);
            sessionUser.Team = team;
        }

        private void OnClientConnected(ulong clientId)
        {
            AuthenticateClient(clientId);
            MyLogger.Log("client connected", LogLevel.Warning);
        }

        private void OnLoadedScene(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
        {
            if (sceneName != CONSTANTS.GAME_SCENE_NAME)
                return;

            var sessionUser = _lobbyData.SessionUsers.FirstOrDefault(x => x.NetworkUser.ClientId == clientId);
            sessionUser.State = UserState.LoadedScene;
        }


        public void Init()
        {
            var teams = new List<SessionTeam>()
            {
                new SessionTeam(1, 4),
                new SessionTeam(2, 4),
                new SessionTeam(3, 4),
                new SessionTeam(4, 4),
            };

            var users = new List<SessionUser>();
            _lobbyData = new LobbyData(System.Guid.NewGuid(), int.MaxValue, users, teams);

            if (NetworkManager.Singleton.IsHost)
            {
                AuthenticateClient(NetworkManager.ServerClientId);
            }
        }





        private void LoadScene()
        {
            base.NetworkManager.SceneManager.LoadScene(CONSTANTS.GAME_SCENE_NAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
            CheckLoadedScene().Forget();
        }

        private async UniTaskVoid CheckLoadedScene()
        {
            var waitSeconds = 60;
            while (waitSeconds > 0)
            {
                MyLogger.Log($"Remaining time: {waitSeconds}");
                if (_lobbyData.SessionUsers.All(x => x.State == UserState.LoadedScene))
                {
                    StartGame();
                    return;
                }

                waitSeconds -= 1;
                await UniTask.Delay(1000);
            }
        }
        private void StartGame()
        {
            var setLevel = Instantiate(_setupLevelTemplate);
            setLevel.NetworkObject.Spawn();
            setLevel.SetupGame(_lobbyData, false);
        }
    }
}
