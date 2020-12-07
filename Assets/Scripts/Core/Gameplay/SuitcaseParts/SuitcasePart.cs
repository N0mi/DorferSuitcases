using System.Collections.Generic;
using UnityEngine;
using Conf;

namespace Core.Gameplay
{
    public class SuitcasePart : MonoBehaviour
    {
        [Range(0.1f, 2f)]
        [SerializeField] private float GridSize = 0.5f;
        public float OffsetZ = -0.2f;

        public bool FlipX { get; set; }
        public bool FlipY { get; set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        [HideInInspector]public Grid slots; 
        
        private void Awake()
        {            
            CreateGrid();
        }

        protected virtual void CreateGrid()
        {
            Width = 5;
            Height = 5;
            slots = new Grid(Width, Height);
        }        

        public bool PlaceItem(Vector3 pos, ItemData data)
        {
            int x, y;
            AproximatePos(pos, out x, out y);
            Item config = data.Config;
            if (CanPlace(x, y, config))
            {
                for (int w = 0; w < config.width; w++)
                {
                    for (int h = 0; h < config.height; h++)
                    {
                        SetItem(x + w, y + h, CellCondition.Busy, data);                        
                    }
                }
                data.Anchor = new Vector2(x, y);
                return true;
            }
            else
            {
                return false;
            }            
        }
        public bool PlaceItem(int x, int y, ItemData data)
        {
            Item config = data.Config;
            if (CanPlace(x, y, config))
            {
                for (int w = 0; w < config.width; w++)
                {
                    for (int h = 0; h < config.height; h++)
                    {
                        SetItem(x + w, y + h, CellCondition.Lock, data);
                    }
                }
                data.Anchor = new Vector2(x, y);
                return true;
            }
            else
            {
                return false;
            }
        }

        public ItemData DeleteItem(Vector3 pos)
        {
            int x, y;
            AproximatePos(pos, out x, out y);
            ItemData data;
            Item config;
            if (slots.InBounds(x, y) && slots[x, y].itemData != null)
            {
                data = slots[x, y].itemData;
                config = data.Config; 
                x = (int)slots[x, y].itemData.Anchor.x;
                y = (int)slots[x, y].itemData.Anchor.y;                
            }
            else
            {
                return null;
            }

            if (slots.InBounds(x, y) && slots[x,y].condition == CellCondition.Busy)
            {
                for (int w = 0; w < config.width; w++)
                {
                    for (int h = 0; h < config.height; h++)
                    {
                        slots[x + w, y + h] = new Slot();
                    }
                }
                return data;
            }
            else
            {
                return null;
            }
            
        }

        public float GetGridSize()
        {
            return GridSize;
        }

        private void SetItem(int x, int y, CellCondition type, ItemData data)
        {
            slots[x, y].condition = type;
            slots[x, y].itemData = data;
        }

        private void AproximatePos(Vector3 pos, out int x, out int y)
        {
            pos -= transform.position + new Vector3(GridSize / 2, GridSize / 2, 0);

            x = Mathf.RoundToInt(pos.x / GridSize);
            y = Mathf.RoundToInt(pos.y / GridSize);
        }

        public void FillFields(Country country)
        {
            Item i = FindItem(2, 2, country);
            if (i != null)
            {
                GetComponent<SpriteRenderer>().sprite = i.image;
            }
        }

        private Item FindItem(int width, int height, Country country)
        {
            List<Item> items = new List<Item>();
            foreach (var i in country.items)
            {
                if (i.width == width && i.height == height)
                {
                    items.Add(i);
                }
            }
            if (items.Count > 0)
            {
                int s = UnityEngine.Random.Range(0, items.Count);
                Debug.Log(s);
                return items[s];
            }
            else
            {
                Debug.Log($"Not found item with (width = {width}, height = {height}) in \"{country.name}\"");
                return null;
            }
        }

        private bool CanPlace(int x, int y, Item i)
        {            
            for (int w = 0; w < i.width; w++)
            {
                for (int h = 0; h < i.height; h++)
                {
                    if (!slots.InBounds(x + w, y + h) ||
                        slots[x + w, y + h].condition == CellCondition.Busy ||
                        slots[x + w, y + h].condition == CellCondition.Lock)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void OnDrawGizmos()
        {
            if(slots == null)
            {
                CreateGrid();
            }
            Gizmos.color = Color.black;
            Vector3 offsetX, offsetY;

            for (int x = 0; x < Width; x++)
            {
                Gizmos.color = Color.black;
                offsetX = new Vector3(x * GridSize, 0, 0);
                Gizmos.DrawLine(transform.position + offsetX, transform.position + Vector3.up * GridSize * Height + offsetX);
                for (int y = 0; y < Height; y++)
                {
                    Gizmos.color = Color.black;
                    offsetY = new Vector3(0, y * GridSize, 0);
                    Gizmos.DrawLine(transform.position + offsetY, transform.position + Vector3.right * GridSize * Width + offsetY);
                    if (slots[x, y] != null)
                    {
                        if (slots[x, y].condition == CellCondition.Busy)
                            Gizmos.color = Color.green;
                        else if(slots[x, y].condition == CellCondition.Lock)
                            Gizmos.color = Color.red;
                        else
                            Gizmos.color = Color.white;
                    }
                    var point = transform.position + new Vector3(x * GridSize + GridSize / 2, y * GridSize + GridSize / 2, OffsetZ);
                    Gizmos.DrawSphere(point, 0.1f);
                }
            }
            offsetY = new Vector3(0, Height * GridSize, 0);
            offsetX = new Vector3(Width * GridSize, 0, 0);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position + offsetY, transform.position + Vector3.right * GridSize * Width + offsetY);
            Gizmos.DrawLine(transform.position + offsetX, transform.position + Vector3.up * GridSize * Height + offsetX);
        }

    }
}

