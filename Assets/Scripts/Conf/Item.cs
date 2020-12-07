using UnityEngine;

namespace Conf
{
    [CreateAssetMenu(fileName = "Item", menuName = "SuitcaseSystem/Item")]
    public class Item : ScriptableObject
    {
        public Sprite image;
        public int width;
        public int height;
        [Tooltip("Если размер предмета не соответствует сетке, можно здесь его корректировать")]
        public Vector3 sizeCorrect = Vector3.one;
    }
}
