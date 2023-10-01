using System;
using System.Collections;
using System.Collections.Generic;
//using CustomHeroArena;
using MadeNPlayShared;
using NUnit.Framework;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class NewTestScript
{
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        //Setup();
        //yield return LoadScene();
        //StartGame();
        Assert.IsTrue(true);
        //yield return new WaitForSeconds(10);
        yield break;
    }

    //private void Setup()
    //{
    //    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/NetworkManager.prefab");
    //    GameObject.Instantiate(prefab);

    //    AbilityProvider.Load();
    //    CreatedHeroProvider.Load(); 
    //    new ConnectionManager().StartHost(12345);

    //}

    //private IEnumerator LoadScene()
    //{
    //    var hand = SceneManager.LoadSceneAsync(2);
    //    while (hand.isDone == false)
    //    {
    //        yield return null;
    //    }
    //}

    //private void StartGame()
    //{
    //    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/GameHandler.prefab");
    //    var setLevel = GameObject.Instantiate(prefab);
    //    var setuplevel = setLevel.GetComponent<SetupLevel>();
    //    setuplevel.NetworkObject.Spawn();


    //    var teams = new List<SessionTeam>()
    //        {
    //            new SessionTeam(1, 4),
    //            new SessionTeam(2, 4),
    //            new SessionTeam(3, 4),
    //            new SessionTeam(4, 4),
    //        };

    //    var users = new List<SessionUser>()
    //        {
    //            new SessionUser()
    //            {
    //                UserId = 00001,
    //                State = UserState.LoadedScene,
    //                User = new NetworkUser(0, "zxcqwe"),
    //                Team = teams[0]
    //            },
    //            //new SessionUser()
    //            //{
    //            //    UserId = 00002,
    //            //    State = UserState.LoadedScene,
    //            //    User = new NetworkUser(1, "qweqweqwe"),
    //            //    Team = teams[0]
    //            //},
    //        };



    //    teams[0].Users.Add(users[0]);

    //    //teams[0].Users.Add(users[1]);

    //    var lobbyDataTest = new LobbyData(Guid.NewGuid(), 10, users, teams);
    //    setuplevel.SetupGame(lobbyDataTest);
    //}
}
