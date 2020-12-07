using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Gameplay
{
    public class TakeController : MonoBehaviour
    {
        public GameObject[] casesObjs;
        public ICase[] cases;
        private Vector3 oldPos;
        public GameObject gettingItem;
        private Camera cam;


        private void Awake()
        {
            cam = Camera.main;
            cases = new ICase[casesObjs.Length];
            for (int i = 0; i < casesObjs.Length; i++)
            {
                cases[i] = casesObjs[i].GetComponent<ICase>();
            }
        }
        
        void Update()
        {
            if (Input.GetMouseButtonUp(0) && gettingItem != null)
            {
                if (!TryPlace(GetXY(gettingItem.GetComponent<ItemData>().GetConnectWorldPos())))
                {
                    if (!TryPlace(GetXY(oldPos)))
                    {
                        gettingItem.transform.position = oldPos;
                        gettingItem = null;
                    }
                    
                }             
            }

            if (Input.GetMouseButtonDown(0))
            {
                gettingItem = TryTake(cam.ScreenToWorldPoint(Input.mousePosition));
                if(gettingItem!=null)
                {
                    oldPos = gettingItem.transform.position;
                }
            }

            if (gettingItem != null)
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                gettingItem.transform.position = new Vector3(mousePos.x, mousePos.y, -1);
            }
        }

        private bool TryPlace(Vector3 pos)
        {
            foreach (var c in cases)
            {
                if (c.PlaceItem(pos, gettingItem))
                {
                    gettingItem = null;
                    return true;
                }
            }
            return false;
        }

        private GameObject TryTake(Vector3 pos)
        {
            foreach (var c in cases)
            {
                GameObject go = c.TakeItem(pos);
                if(go!=null)
                {
                    return go;
                }                
            }
            return null;
        }

        private Vector3 GetXY(Vector3 vec)
        {
            return new Vector3(vec.x, vec.y, 0);
        }
    }

}
