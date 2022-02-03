using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Inventory
{
    public static class PartyInventory
    {
        public const int MAX_INVENTORY_SIZE = 4;
        public static Item[] inventory = new Item[MAX_INVENTORY_SIZE];
        public static int partyGold = 50;

        public static void UseItem(int _itemIndex)
        {
            if (_itemIndex < 0 || _itemIndex >= MAX_INVENTORY_SIZE || inventory[_itemIndex] == null)
            {
                throw new System.Exception("Invalid Index Given. Either no item is present there, or the index was out of range.");
            }

            if (TurnManager.instance.activeTurn.unit.GetType() == typeof(FriendlyUnit))
            {
                inventory[_itemIndex].UseItem(TurnManager.instance.activeTurn.unit);
            }
        }
        public static bool AddItem(Item _item)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == null)
                {
                    inventory[i] = _item;
                    return true;
                }
            }

            return false;
        }
        public static void RemoveItem(int _itemIndex)
        {
            if (_itemIndex < 0 || _itemIndex >= MAX_INVENTORY_SIZE || inventory[_itemIndex] == null)
            {
                throw new System.Exception("Invalid Index Given. Either no item is present there, or the index was out of range.");
            }

            inventory[_itemIndex] = null;
        }
        public static bool RemoveItem(Item _item)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == _item)
                {
                    inventory[i] = null;
                    return true;
                }
            }

            return false;
        }
        public static void AddGold(int _goldToAdd)
        {
            partyGold += _goldToAdd;
        }
        public static bool HasEnoughGold(int _goldAmount)
        {
            return partyGold >= _goldAmount;
        }
        public static bool HasFreeInventorySpace()
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if(inventory[i] == null)
                {
                    return true;
                }
            }

            return false;
        }
        public static void RemoveGold(int _goldToRemove)
        {
            partyGold -= _goldToRemove;
        }
    }
}