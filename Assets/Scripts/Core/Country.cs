using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    [CreateAssetMenu(fileName = "Country", menuName = "SuitcaseSystem/Country")]
    public class Country : ScriptableObject
    {
        public string title;
        public Image logo;
        public List<Item> items;
    }
}

