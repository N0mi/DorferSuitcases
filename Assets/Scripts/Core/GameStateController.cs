using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameStateController : MonoBehaviour
    {
        public static GameStateController instance = null;

        public static int WonRound = 0;
        public static int LoseRound = 0;

        public bool HasVibrate = true;
        public GameStatus statusGame = GameStatus.None;

        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance == this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            Init();
        }

        private void Init()
        {
           
        }

        public void WinGame()
        {
            WonRound++;
            statusGame = GameStatus.Win;           
            SceneLoader.instance.NextScene();
        }

        public void LoseGame()
        {
            LoseRound++;
            statusGame = GameStatus.Lose;
            SceneLoader.instance.NextScene();
        }
    }

    public enum GameStatus
    {
        None,
        Win,
        Lose
    }

}

