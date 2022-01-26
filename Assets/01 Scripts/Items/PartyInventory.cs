using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyInventory : MonoBehaviour
{
    public const int MAX_INVENTORY_SIZE = 4;
    [ReadOnly] public Item[] items = new Item[MAX_INVENTORY_SIZE];

    TurnManager turnManager;

    public static PartyInventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            CopyInstance();
        }

        instance = this;
    }

    private void Start()
    {
        turnManager = TurnManager.instance;
    }

    private void CopyInstance()
    {
        items = instance.items;
    }

    public void UseItem(int _itemIndex)
    {
        if (_itemIndex < 0 || _itemIndex >= MAX_INVENTORY_SIZE || items[_itemIndex] == null)
        {
            throw new System.Exception("Invalid Index Given. Either no item is present there, or the index was out of range.");
        }

        if(turnManager.activeTurn.unit.GetType() == typeof(FriendlyUnit))
        {
            items[_itemIndex].UseItem(turnManager.activeTurn.unit);
        }
    }

    public bool AddItem(Item _item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                items[i] = _item;
                return true;
            }
        }

        return false;
    }

    public void RemoveItem(int _itemIndex)
    {
        if (_itemIndex < 0 || _itemIndex >= MAX_INVENTORY_SIZE || items[_itemIndex] == null)
        {
            throw new System.Exception("Invalid Index Given. Either no item is present there, or the index was out of range.");
        }

        items[_itemIndex] = null;
    }

    public bool RemoveItem(Item _item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] == _item)
            {
                items[i] = null;
                return true;
            }
        }

        return false;
    }
}
