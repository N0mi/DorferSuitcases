using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Gameplay;
using Core;

namespace Conf
{
    public class SetupItem : MonoBehaviour
    {
        public SuitcasePart part = null;

        [SerializeField] private int width = 1;
        [SerializeField] private int height = 1;

        public Vector2 pos;

        private GameObject itemObj;
        private ICase CaseComp;

        private void Start()
        {
            itemObj = Instantiate(Resources.Load<GameObject>(Paths.Item));
            Item item = ResourceManager.instance.GetItem(width, height);
            itemObj.GetComponent<ItemData>().Config = item;
            if (item.sizeCorrect != Vector3.one)
            {
                itemObj.transform.localScale = item.sizeCorrect;
            }

            CaseComp = GetComponent<ICase>();

            if(!CaseComp.PlaceItem(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), itemObj, part))
            {
                Debug.Log("Can't place item");
                Destroy(itemObj);
            }
        }
    }

}
