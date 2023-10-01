using Cysharp.Threading.Tasks;
using MadeNPlayShared;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOBA
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private SetupLevel _setupLevelTemplate;
        [SerializeField] private bool _loadFast;

        private async void Start()
        {
            UnitAssetProvider.Load();
            if (_loadFast)
            {
                DontDestroyOnLoad(gameObject);
                new ConnectionManager().StartHost(CONSTANTS.DEFAULT_PORT);
                await SceneManager.LoadSceneAsync(CONSTANTS.GAME_SCENE_INDEX, LoadSceneMode.Single);
                StartGame();
                Destroy(gameObject);
                return;
            }
            SceneManager.LoadScene(CONSTANTS.MAIN_MENU_SCENE_INDEX);
        }

        private void StartGame()
        {
            var setLevel = Instantiate(_setupLevelTemplate);
            setLevel.NetworkObject.Spawn();


            var teams = new List<SessionTeam>()
            {
                new SessionTeam(1, 4),
                new SessionTeam(2, 4),
                new SessionTeam(3, 4),
                new SessionTeam(4, 4),
            };

            var users = new List<SessionUser>()
            {
                new SessionUser()
                {
                    UserId = 00001,
                    State = UserState.LoadedScene,
                    NetworkUser = new NetworkUser(0, "zxcqwe"),
                    Team = teams[0]
                }
            };



            teams[0].Users.Add(users[0]);

            //teams[0].Users.Add(users[1]);

            var lobbyDataTest = new LobbyData(Guid.NewGuid(), 10, users, teams);
            setLevel.SetupGame(lobbyDataTest).Forget();
        }


    }
}