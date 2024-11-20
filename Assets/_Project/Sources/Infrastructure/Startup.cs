using Cysharp.Threading.Tasks;
using GOBA.Network;
using MapModCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOBA
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private SetupLevel _setupLevelTemplate;
        [SerializeField] private bool _loadDemo;

        private void Start()
        {
            StartAsync().Forget();
        }

        private async UniTaskVoid StartAsync()
        {
            UnitAssetProvider.Initialize();
            var entityManager = new EntityManager();
            DIContainer.EntityManager = entityManager;
            var projectileProvider = new ProjectileProvider();
            await projectileProvider.Initialize();
            DIContainer.ProjectileProvider = projectileProvider;

            if (_loadDemo)
                StartDemo().Forget();
            else
                StartGame();
        }

        private void StartGame()
        {
            SceneManager.LoadScene(CONSTANTS.MAIN_MENU_SCENE_INDEX);
        }

        private async UniTaskVoid StartDemo()
        {
            DontDestroyOnLoad(gameObject);
            new ConnectionManager().StartHost(CONSTANTS.DEFAULT_PORT);
            await SceneManager.LoadSceneAsync(CONSTANTS.GAME_SCENE_INDEX, LoadSceneMode.Single);

            var setupLevel = Instantiate(_setupLevelTemplate);
            setupLevel.NetworkObject.Spawn();

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

            var lobbyDataTest = new LobbyData(Guid.NewGuid(), 10, users, teams);
            setupLevel.SetupGame(lobbyDataTest, true).Forget();

            Destroy(gameObject);
        }
    }
}