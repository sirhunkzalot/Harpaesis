using Harpaesis.Combat;
using UnityEngine;

public class Item_ATKPotion : Item
{
    public override void UseEffect(Unit _unit)
    {
        _unit.ApplyEffect(new StatusEffect_BuffATK(_unit, _unit, GameSettings.itemSettings.atkPotionBuffAmount, GameSettings.itemSettings.atkPotionBuffDuration));
    }
}
