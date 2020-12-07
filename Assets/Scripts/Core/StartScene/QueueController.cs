using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conf;

namespace Core
{
    public class QueueController : MonoBehaviour
    {
        [SerializeField] private float SpeedRotate = 2;
        [SerializeField] private float SpeedAwayPasssenger = 0.2f;
        [SerializeField] private float SpeedRotatePassenger = 0.2f;
        [SerializeField] private Transform receptionPos;        
        [SerializeField] private int CountPassenger = 5;

        [SerializeField] private float SpeedCase = 0.2f;
        [SerializeField] private GameObject casePrefab;
        [SerializeField] private Transform startCasePos;

        public GameObject currentPassenger;

        private Queue<GameObject> passengers = new Queue<GameObject>();

        private GameObject PassengerPrefab;
        private GameObject CaseObj;
        private bool isRotate;        

        private void Awake()
        {
            PassengerPrefab = Resources.Load<GameObject>(Paths.Passenger);           
        }

        private void Start()
        {
            for (int i = 0; i < CountPassenger; i++)
            {
                CreatePassenger();
                transform.rotation *= Quaternion.Euler(0f, -180f / CountPassenger, 0f);
            }            

            if (QueueSaver.instance.HasSaveData)
            {
                Vector3 center = new Vector3(0, startCasePos.position.y, startCasePos.position.z);
                CaseObj = Instantiate(casePrefab, center, Quaternion.identity);
            }
            else
            {
                StartCoroutine("CaseToCenter");
            }
        }

        private Vector3 CalculatePosition()
        {
            if (receptionPos == null)
            {
                Debug.LogError("Not set position reception");
                return Vector3.zero;
            }

            Vector3 pos = (receptionPos.position - transform.position) * -1f;
            pos = pos + transform.position;
            return new Vector3(pos.x, receptionPos.position.y, pos.z);
        }

        private void CreatePassenger()
        {
            GameObject go = Instantiate(PassengerPrefab, CalculatePosition(), Quaternion.identity);
            go.transform.SetParent(transform);
            passengers.Enqueue(go);
        }
        
        public void TakeInfoPassenger()
        {
            currentPassenger = passengers.Dequeue();
        }  

        public bool NextPassenger(bool skip)
        {
            bool direction;
            if (skip)
            {
                direction = false;
            }
            else
            {
                if (GameStateController.instance.statusGame == GameStatus.Win)
                    direction = true;
                else
                    direction = false;
            }

            if (!isRotate)
            {
                if (currentPassenger == null) TakeInfoPassenger();
                CreatePassenger();                
                isRotate = true;
                StartCoroutine("PassengerTakeAway", direction);
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator CaseToCenter()
        {
            CaseObj = Instantiate(casePrefab, startCasePos);
            Vector3 center = new Vector3(0, startCasePos.position.y, startCasePos.position.z);
            while (CaseObj.transform.position != center && SpeedCase > 0)
            {
                CaseObj.transform.position = Vector3.MoveTowards(CaseObj.transform.position, center, SpeedCase);
                yield return new WaitForFixedUpdate();
            }
            yield break;
        }

        IEnumerator CaseTakeAway(bool left)
        {
            Vector3 currentPos;
            if (left)
            {
               currentPos = new Vector3(-3, startCasePos.position.y, startCasePos.position.z);
            }
            else
            {
                currentPos = startCasePos.transform.position;
            }

            while (CaseObj.transform.position != currentPos && SpeedCase > 0)
            {
                CaseObj.transform.position = Vector3.MoveTowards(CaseObj.transform.position, currentPos, SpeedCase);
                yield return new WaitForFixedUpdate();
            }
            Destroy(CaseObj);
            yield return CaseToCenter();
        }

        IEnumerator PassengerTakeAway(bool left)
        {
            Quaternion NewRotate;
            Vector3 NewPos;
            if (left)
            {
                NewRotate = currentPassenger.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
                NewPos = new Vector3(-4, currentPassenger.transform.position.y, currentPassenger.transform.position.z);
            }
            else
            {
                NewRotate = currentPassenger.transform.rotation * Quaternion.Euler(0f, -90f, 0f);
                NewPos = new Vector3(4, currentPassenger.transform.position.y, currentPassenger.transform.position.z);
            }


            while (currentPassenger.transform.rotation != NewRotate && SpeedRotatePassenger > 0)
            {
                currentPassenger.transform.rotation = Quaternion.RotateTowards(currentPassenger.transform.rotation, NewRotate, SpeedRotatePassenger);
                yield return new WaitForFixedUpdate();
            }
            StartCoroutine("CaseTakeAway", left);

            
            while (currentPassenger.transform.position != NewPos && SpeedCase > 0)
            {
                currentPassenger.transform.position = Vector3.MoveTowards(currentPassenger.transform.position, NewPos, SpeedCase);
                yield return new WaitForFixedUpdate();
            }
            Destroy(currentPassenger);
            yield return StartCoroutine("Rotate");
        }

        IEnumerator Rotate()
        {
            Quaternion NewRotate = transform.rotation * Quaternion.Euler(0f, -180f / CountPassenger, 0f);

            while (transform.rotation != NewRotate && SpeedRotate > 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, NewRotate, 2f);
                yield return new WaitForFixedUpdate();
            }
            isRotate = false;
            DialogManager.instance.ShowControllButton();
        }
    }
}

