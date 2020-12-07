using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Conf
{
    [CreateAssetMenu(fileName = "Country", menuName = "SuitcaseSystem/Country")]
    public class Country : ScriptableObject
    {
        public string title;
        public Sprite image;
        public List<Item> items;
    }
}

