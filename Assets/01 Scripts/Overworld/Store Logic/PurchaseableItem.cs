using System.Collections;
using System.Collections.Generic;
using Harpaesis.UI;
using Harpaesis.Inventory;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Harpaesis.Overworld.Store
{
    public class PurchaseableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        SpriteRenderer spriteRenderer;

        public StoreItem item;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = item.item.itemSprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            spriteRenderer.color = Color.yellow;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            spriteRenderer.color = Color.white;
        }

        public void OnPointerDown(PointerEventData eventData)
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