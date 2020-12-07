using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Core
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance = null;

        public int Score { get; private set; }

        public delegate void ScoreHandler();
        public event ScoreHandler ChangeScore;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Score = PlayerPrefs.GetInt("Score");
            }
            else if (instance == this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void AddScore(int value)
        {
            if (value >= 0)
            {
                Score += value;
                PlayerPrefs.SetInt("Score", Score);
                ChangeScore?.Invoke();
            }

            else
            {
                Debug.LogError("Can't add to score negative value");
            }
        }
    }

}
