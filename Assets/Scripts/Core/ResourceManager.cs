using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Gameplay;
using Conf;
using System;

namespace Core
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager instance = null;

        private List<Country> AllCountry = new List<Country>();
        private List<Item> AllItems = new List<Item>();
        private List<Material> PassangerMaterials = new List<Material>();

        private Country currentCountry = null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance == this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            Init();
        }

        private void Init()
        {
            foreach (var country in Resources.LoadAll<Country>(Paths.CountrySO))
            {
                AllCountry.Add(country);
            }
            foreach (var item in Resources.LoadAll<Item>(Paths.ItemSO))
            {
                AllItems.Add(item);
            }
            foreach (var material in Resources.LoadAll<Material>(Paths.PassangerMat))
            {
                PassangerMaterials.Add(material);
            }
        }

        public Country GetRandomCountry()
        {
            if (AllCountry.Count == 0) return null;
            int c = UnityEngine.Random.Range(0, AllCountry.Count);            
            return AllCountry[c];
        }

        public Material GetRandomPassangerMaterial()
        {
            if (PassangerMaterials.Count == 0) return null;
            int c = UnityEngine.Random.Range(0, PassangerMaterials.Count);
            return PassangerMaterials[c];
        }

        public Item GetItem(int width, int height)
        {
            List<Item> items = new List<Item>();
            if (currentCountry == null)
            {
                foreach (var i in AllItems)
                {
                    if (i.width == width && i.height == height)
                    {
                        items.Add(i);
                    }
                }
            }
            else
            {
                foreach (var i in currentCountry.items)
                {
                    if (i.width == width && i.height == height)
                    {
                        items.Add(i);
                    }
                }
            }
            if (items.Count > 0)
            {
                int s = UnityEngine.Random.Range(0, items.Count);
                return items[s];
            }
            else
            {
                Debug.Log($"Not found item with (width = {width}, height = {height})");
                return null;
            }
        }
    }
}

