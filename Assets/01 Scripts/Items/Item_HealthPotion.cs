using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_HealthPotion : Item
{
    public override void UseEffect(Unit _unit)
    {
        _unit.Heal(_unit, GameSettings.itemSettings.potionHealAmount);
    }
}
