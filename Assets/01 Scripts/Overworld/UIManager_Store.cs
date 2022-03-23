using System.Collections;
using System.Collections.Generic;
using Harpaesis.Inventory;
using Harpaesis.Overworld.Store;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Harpaesis.UI
{
    public class UIManager_Store : MonoBehaviour
    {
        public GameObject goldCanvas;
        public GameObject itemPanel;
        public GameObject inventoryPanel;
        public Button buyButton;


        [Header("Text Boxes")]
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemDescriptionText;
        public TextMeshProUGUI costText;

        PurchaseableItem currentItem;

        public static UIManager_Store instance;

        private void Awake()
        {
            instance = this;
            LeaveStore();
        }

        public void EnterStore()
        {
            goldCanvas.SetActive(true);
            goldText.text = $"Grots:\n{PartyInventory.partyGold}";
            inventoryPanel.SetActive(true);
        }

        public void LeaveStore()
        {
            goldCanvas.SetActive(false);
            itemPanel.SetActive(false);
            inventoryPanel.SetActive(false);
        }

        public void LookAtItem(PurchaseableItem _storeItem)
        {
            itemPanel.SetActive(true);

            currentItem = _storeItem;
            buyButton.interactable = PartyInventory.HasFreeInventorySpace() && PartyInventory.HasEnoughGold(currentItem.item.itemPrice);

            itemNameText.text = currentItem.item.item.itemName;
            itemDescriptionText.text = currentItem.item.item.itemDescription;
            costText.text = currentItem.item.itemPrice.ToString();
        }

        public void Button_Buy()
        {
            if (PartyInventory.AddItem(currentItem.item.item))
            {
                PartyInventory.RemoveGold(currentItem.item.itemPrice);
                currentItem.OnBuy();
                UIManager_PartyInventory.instance.UpdateInventoryDisplay();
                goldText.text = $"Grots:\n{PartyInventory.partyGold}";
                itemPanel.SetActive(false);
            }
        }

        public void Buy_Cancel()
        {
            itemPanel.SetActive(false);
            currentItem = null;
        }
    }
}