using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conf;

namespace Core.Gameplay
{
    public delegate void ClickHandler(GameObject obj);
    public class ItemData : MonoBehaviour
    {
        public event ClickHandler ClickDown;

        public Item Config
        {
            get { return _config; }
            set
            {
                _config = value;
                UpdateView();
            }
        }
        public Vector2 Anchor
        {
            get { return _anchor; }
            set
            {
                _anchor = value;
                UpdateView();
            }
        }

        private SpriteRenderer sr;
        private Item _config;
        private Vector2 _anchor;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public Vector3 GetConnectWorldPos()
        {
            Vector3 offset = new Vector3(sr.sprite.bounds.size.x / 5 * transform.localScale.x, sr.sprite.bounds.size.y / 5 * transform.localScale.y, -3);
            return transform.position -= offset;
        }

        private void UpdateView()
        {
            sr.sprite = Config.image;
            transform.localPosition = new Vector3(Anchor.x * 0.5f + (0.5f / 2f), Anchor.y * 0.5f + 0.5f / 2f, 0f);
        }

        private void OnMouseDown()
        {
            ClickDown?.Invoke(gameObject);
        }

        private void OnMouseUp()
        {
            
        }
    }
}

