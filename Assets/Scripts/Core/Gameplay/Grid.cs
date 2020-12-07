using System;
using System.Collections.Generic;
using UnityEngine;
using Core.Gameplay;

namespace Core.Gameplay
{
    public class Grid
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int Capacity;

        private readonly Slot[] _items;

        public Grid(int x, int y)
        {
            Width = x;
            Height = y;
            Capacity = Width * Height;
            _items = new Slot[Capacity];
            for (int i = 0; i < _items.Length; i++)
            {
                _items[i] = new Slot();
            }
        }

        public Slot this[int x, int y]
        {
            get
            {
                if (x >= Width || y >= Height || x < 0 || y < 0)
                {
                    return null;
                }
                return _items[x + y * Width];
            }
            set
            {
                if (x >= Width || y >= Height || x < 0 || y < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _items[x + y * Width] = value;
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _items.Length; ++i)
            {
                _items[i] = null;
            }
        }

        public bool InBounds(int x, int y)
        {
            return x < Width && y < Height && x >= 0 && y >= 0;
        }
    }
}

