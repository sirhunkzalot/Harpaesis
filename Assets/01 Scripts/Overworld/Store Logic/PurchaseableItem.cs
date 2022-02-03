using System.Collections;
using System.Collections.Generic;
using Harpaesis.UI;
using Harpaesis.Inventory;
using UnityEngine;

namespace Harpaesis.Overworld.Store
{
    public class PurchaseableItem : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;

        public StoreItem item;

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
            UIManager_Store.instance.LookAtItem(this);
        }

        public void OnBuy()
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class StoreItem
    {
        public Item item;
        public int itemPrice;
    }
}