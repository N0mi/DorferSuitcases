using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Item", menuName = "SuitcaseSystem/Item")]
    public class Item : ScriptableObject
    {
        public Sprite logo;
        public int width;
        public int height;
    }
}
