using Harpaesis.Combat;
using UnityEngine;

public class Item_DEFPotion : Item
{
    public override void UseEffect(Unit _unit)
    {
        _unit.ApplyEffect(new StatusEffect_BuffDEF(_unit, _unit, GameSettings.itemSettings.defPotionBuffAmount, GameSettings.itemSettings.defPotionBuffDuration));
    }
}