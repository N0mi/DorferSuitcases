using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conf;

namespace Core.Gameplay
{
    public class SuitcaseConnector : MonoBehaviour
    {
        public SuitcasePart ToPart;
        public SuitcasePart TargetPart;
        public int priority;
        public bool isOpen = true;

        private GameObject rotatePoint;
        private Transform OldParent;
        private GameObject PointPref;

        private void Awake()
        {
            PointPref = Resources.Load<GameObject>(Paths.ConnectorPoint);
            FindConnectorPoint();
        }

        public void Close()
        {
            isOpen = false;
            ChangePos();
        }

        private void ChangePos()
        {
            OldParent = TargetPart.transform.parent;
            Quaternion rotation;

            if (Mathf.Abs(ToPart.transform.position.x - TargetPart.transform.position.x) > Mathf.Abs(ToPart.transform.position.y - TargetPart.transform.position.y))
            {
                rotation = Quaternion.Euler(0f, 180f, 0f);
                TargetPart.GetComponent<SuitcasePart>().FlipX = true;
            }

            else
            {
                rotation = Quaternion.Euler(180f, 0f, 0f);
                TargetPart.GetComponent<SuitcasePart>().FlipY = true;
            }

            TargetPart.transform.position += new Vector3(0, 0, -0.01f * priority);
            TargetPart.transform.SetParent(rotatePoint.transform);
            StartCoroutine("Rotator", rotation);
        }

        IEnumerator Rotator(Quaternion end)
        {
            int i = 0;
            while (i < 50)
            {
                rotatePoint.transform.rotation = Quaternion.Slerp(rotatePoint.transform.rotation, end, 0.3f);
                i++;
                yield return new WaitForFixedUpdate();
            }
            rotatePoint.transform.rotation = end;
            TargetPart.transform.SetParent(OldParent);

            yield break;
        }

        private void FindConnectorPoint()
        {
            Vector3 TargetPartCenter = TargetPart.GetComponentsInChildren<Renderer>()[0].bounds.center;
            Vector3 ToPartCenter = ToPart.GetComponentsInChildren<Renderer>()[0].bounds.center;

            Vector3 pointPos = ToPartCenter + (TargetPartCenter - ToPartCenter) / 2;
            pointPos = new Vector3(pointPos.x, pointPos.y, -0.5f);
            rotatePoint = Instantiate(PointPref, pointPos, Quaternion.identity * Quaternion.Euler(0.01f, 0.01f, 0));
        }
    }
}

