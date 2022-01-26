using System.Collections;
using System.Collections.Generic;
using Harpaesis.Inventory;
using UnityEngine.UI;
using UnityEngine;

namespace Harpaesis.UI
{
    public class UIManager_PartyInventory : MonoBehaviour
    {
        public Image[] images = new Image[4];

        private void Start()
        {
            UpdateInventoryDisplay();
        }

        private void OnEnable()
        {
            UpdateInventoryDisplay();
        }

        public void UpdateInventoryDisplay()
        {
            for (int i = 0; i < images.Length; i++)
            {
                if(PartyInventory.inventory[i] != null)
                {
                    images[i].sprite = PartyInventory.inventory[i].itemSprite;
                }
                else
                {
                    images[i].sprite = null;
                }
            }
        }

        public void Button_UseItem(int _index)
        {
            PartyInventory.UseItem(_index);
            UpdateInventoryDisplay();
            UpdateInventoryDisplay();
        }
    }
}