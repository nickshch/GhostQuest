﻿using System.Collections.Generic;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.SkillSystem;
using HauntedCity.Utils;
using UnityEngine;

namespace HauntedCity.GameMechanics.Main
{
    public class GameController:MonoBehaviour
    {
        public static PlayerGameStats GameStats = new PlayerGameStats();

        private BattleStateController _battleStateController;
        private SceneAgregator _sceneAgregator;

        public string[] AllowableGhosts = { "shadow_skull", "devil_mask"};
    
        void Awake() {
            DontDestroyOnLoad(transform.gameObject);
            _battleStateController = GameObject.Find("BattleStateController").GetComponent<BattleStateController>();
            _sceneAgregator = GameObject.Find("SceneAgregator").GetComponent<SceneAgregator>();
            _sceneAgregator.OnSceneChange += OnSceneChange;
            _sceneAgregator.OnAllScenesLoad += OnAllScenesLoad;
            _battleStateController.OnWon += BattleWonHandle;
            _battleStateController.OnLose += BattleLoseHandle;
            //SceneManager.sceneLoaded += OnSceneLoad;

        }

        void Start()
        {


        }

        private void OnAllScenesLoad()
        {
            _sceneAgregator.switchToScene("map");
        } 
        
        private void OnSceneChange(string sceneName)
        {
            if (sceneName == "battle")
            {
                _battleStateController.StartBattle(RandomGhosts()); 
            }
        }

        public Dictionary<string, int> RandomGhosts()
        {
            var result = new Dictionary<string, int>();
            foreach (var ghost in AllowableGhosts)
            {
                result.Add(ghost, Random.Range(3,5));
            }
            return result;
        }


        public void StartBattle()
        {
            _sceneAgregator.switchToScene("battle");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public void InitStats()
        {
            GameStats  = new PlayerGameStats();
        }

        public void BattleWonHandle(int score)
        {
            GameStats.AddExp(score);
            _sceneAgregator.switchToScene("map");
            Debug.Log("Level: " + GameStats.Level + "  " + GameStats.CurrentExp + "/" + GameStats.ExpToLevel);
        }

        public void BattleLoseHandle()
        {
            _sceneAgregator.switchToScene("map");
        }

    }
}