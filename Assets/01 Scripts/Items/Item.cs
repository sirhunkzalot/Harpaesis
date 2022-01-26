using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public List<SkillEffect> skillEffects = new List<SkillEffect>();

    public virtual void UseItem(Unit _unit)
    {
        SkillEffectLibrary.ResolveEffects(_unit, _unit, skillEffects);
    }
}
