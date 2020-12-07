using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Conf;
using UnityEngine.Events;
using System;

namespace Core
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager instance = null;

        public Transform root;
        public QueueController queue;

        public GameObject prefDialog;
        public GameObject prefDialogWin;
        public GameObject prefDialogLose;

        public GameObject OKBtn;
        public GameObject SkipBtn;
        public GameObject ContinueBtn;
        public GameObject StartPanel;

        private List<GameObject> Windows = new List<GameObject>();

        private void Start()
        {
            if (instance == null)
            {                
                instance = this;
             
            }
            else if (instance == this)
            {
                Destroy(gameObject);
            }

            Init();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                queue.currentPassenger.GetComponent<Passenger>().Sad();
            }
        }

        private void Init()
        {
            if (root == null)
            {
                root = transform;
            }
            if (GameStateController.instance.statusGame == GameStatus.None)
            {
                StartPanel.SetActive(true);
                StartPanel.GetComponent<Button>().onClick.AddListener(ShowControllButton);
            }
            else
            {
                if (GameStateController.instance.statusGame == GameStatus.Win)
                {
                    CreateFinalDialogWin(QueueSaver.instance.reward);
                }
                else
                {
                    CreateFinalDialogLose();
                }
            }
            ContinueBtn.GetComponent<Button>().onClick.AddListener(ContinueAction);
            SkipBtn.GetComponent<Button>().onClick.AddListener(SkipAction);
            OKBtn.GetComponent<Button>().onClick.AddListener(OkAction);
        }

        public void CreateDialog(Country country, int reward)
        {
            GameObject go = Instantiate(prefDialog, root);
            go.transform.GetChild(1).GetComponentInChildren<Text>().text = reward.ToString();
            go.transform.GetChild(0).GetComponent<Image>().sprite = country.image;
            Windows.Add(go);

        }

        public void CreateFinalDialogWin(int reward)
        {
            ContinueBtn.SetActive(true);
            GameObject go = Instantiate(prefDialogWin, root);
            go.GetComponentInChildren<Text>().text = QueueSaver.instance.reward.ToString();
            Windows.Add(go);

            StartCoroutine("WinAction");            

        }

        public void CreateFinalDialogLose()
        {
            StartCoroutine("LoseAction");
        }

        public void CloseAllWindow()
        {            
            foreach (var window in Windows)
            {
                Destroy(window);
            }
            Windows.Clear();
        }      
        
        public void ShowControllButton()
        {
            StartPanel.SetActive(false);
            ContinueBtn.SetActive(false);
            OKBtn.SetActive(true);
            SkipBtn.SetActive(true);
            queue.TakeInfoPassenger();
            Passenger pas = queue.currentPassenger.GetComponent<Passenger>();
            CreateDialog(pas.Destination, pas.Reward);
        }

        private void OkAction()
        {
            CloseAllWindow();
            QueueSaver.instance.SaveData(queue);
            SceneLoader.instance.NextScene();
        }

        private void SkipAction()
        {
            OKBtn.SetActive(false);
            SkipBtn.SetActive(false);
            queue.currentPassenger.GetComponent<Passenger>().Sad();
            queue.NextPassenger(true);
            CloseAllWindow();
        }

        private void ContinueAction()
        {
            ScoreManager.instance.AddScore(QueueSaver.instance.reward);
            queue.NextPassenger(false);
            ContinueBtn.SetActive(false);
            CloseAllWindow();
        }

        IEnumerator WinAction()
        {
            yield return new WaitForSeconds(0.2f);
            queue.TakeInfoPassenger();
            queue.currentPassenger.GetComponent<Passenger>().Enjoy();
        }

        IEnumerator LoseAction()
        {
            Windows.Add(Instantiate(prefDialogLose, root));
            yield return new WaitForSeconds(0.2f);
            queue.TakeInfoPassenger();
            queue.currentPassenger.GetComponent<Passenger>().Sad();
            yield return new WaitForSeconds(1.5f);
            CloseAllWindow();
            queue.NextPassenger(true);
        }
    }
}

