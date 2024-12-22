using Cysharp.Threading.Tasks;
using GOBA.Network;
using GOBA.CORE;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOBA
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private SetupLevel _setupLevelTemplate;

        private void Start()
        {
            StartAsync().Forget();
        }

        private async UniTaskVoid StartAsync()
        {
            var entityManager = new EntityManager();
            DIContainer.EntityManager = entityManager;

            StartGame();
        }

        private void StartGame()
        {
            SceneManager.LoadScene(CONSTANTS.MAIN_MENU_SCENE_INDEX);
        }
    }
}