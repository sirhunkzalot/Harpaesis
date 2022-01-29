using System.Collections;
using System.Collections.Generic;
using Harpaesis.Inventory;
using UnityEngine;

namespace Harpaesis.Overworld.Store
{
    public class PurchaseableItem : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;

        [SerializeField] StoreItem item;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnMouseEnter()
        {
            spriteRenderer.color = Color.yellow;
        }

        private void OnMouseExit()
        {
            spriteRenderer.color = Color.white;
        }

        private void OnMouseDown()
        {
            Destroy(gameObject);
        }

        [System.Serializable]
        struct StoreItem
        {
            public Item item;
            public int itemPrice;
        }
    }
}