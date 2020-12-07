using UnityEngine;
using System.Collections.Generic;
using System;

namespace Core.Gameplay
{
    class StartSlots : MonoBehaviour, ICase
    {
        public List<GameObject> items = new List<GameObject>();

        public Transform[] slots = new Transform[3];

        private GameObject takeItem;

        public bool PlaceItem(Vector3 pos, GameObject item)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(pos);
            if (screenPoint.y < Screen.height * 0.3)
                return PlaceItem(item);            
            return false;
        }

        public bool PlaceItem(int x, int y, GameObject item)
        {
            return PlaceItem(item);
        }

        public bool PlaceItem(int x, int y, GameObject item, SuitcasePart part)
        {
            return PlaceItem(item);
        }

        public bool PlaceItem(GameObject item)
        {
            int placeNum = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if ( items[i].transform.position == slots[placeNum].transform.position)
                {
                    placeNum += 1;
                    i = 0;
                }
            }
            if(slots.Length-1 > placeNum)
            {
                items.Add(item);
                item.transform.position = slots[placeNum].transform.position;
                item.GetComponent<ItemData>().ClickDown += TakeControll;
                return true;
            }

            return false;
        }        

        public GameObject TakeItem(Vector3 pos)
        {
            if(takeItem != null)
            {
                GameObject t = takeItem;
                takeItem = null;
                items.Remove(t);
                return t;
            }
            return null;
        }

        private void TakeControll(GameObject obj)
        {
            takeItem = obj;
            obj.GetComponent<ItemData>().ClickDown -= TakeControll;
        }        
    }
}
