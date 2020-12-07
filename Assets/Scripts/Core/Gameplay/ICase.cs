using UnityEngine;

namespace Core.Gameplay
{
    public interface ICase
    {
        bool PlaceItem(Vector3 pos, GameObject item);
        bool PlaceItem(int x, int y, GameObject item);
        bool PlaceItem(int x, int y, GameObject item, SuitcasePart part);
        GameObject TakeItem(Vector3 pos);
    }
}
