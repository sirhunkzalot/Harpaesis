using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Consumable Item")]
public class Item_Consumable : Item
{
    public override void UseItem(Unit _unit)
    {
        base.UseItem(_unit);
        PartyInventory.instance.RemoveItem(this);
    }
}
