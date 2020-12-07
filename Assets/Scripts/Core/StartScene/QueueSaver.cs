using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conf;
using System;

namespace Core
{
    public class QueueSaver : MonoBehaviour
    {
        public static QueueSaver instance = null;
        public bool HasSaveData => reward != 0;

        public int reward = 0;

        private void Awake()
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
        }

        public void SaveData(QueueController controller)
        {
            reward = controller.currentPassenger.GetComponent<Passenger>().Reward;
        }
    }
}

