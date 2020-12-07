using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader instance = null;
                
        public float transitionTime = 1f;


        private Animator transition;
        private int CurrentScene;
        private int CountScenes;

        void Awake()
        {           
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            Init();            
        }

        private void Init()
        {
            CountScenes = SceneManager.sceneCountInBuildSettings;
        }

        public void NextScene()
        {            
            if(CurrentScene != 0)
            {
                CurrentScene = 0;
            }
            else
            {
                CurrentScene = GetRandomScene();
            }
            PlayerPrefs.SetInt("CurrentScene", CurrentScene);
            transition = GameObject.Find("CrossFade")?.GetComponent<Animator>();
            StartCoroutine(LoadLevel(CurrentScene));
        }

        private int GetRandomScene()
        {
            return UnityEngine.Random.Range(1, CountScenes);
        }

        public void RestartGame()
        {
            transition = GameObject.Find("CrossFade")?.GetComponent<Animator>();
            StartCoroutine(LoadLevel(CurrentScene));
        }

        IEnumerator LoadLevel(int levelIndex)
        {            
            transition?.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(CurrentScene);            
        }
    }
}

