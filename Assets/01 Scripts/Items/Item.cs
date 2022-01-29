using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine;

namespace Harpaesis.Inventory
{
    public abstract class Item : ScriptableObject
    {
        public string itemName;
        public int itemCost = 5;
        [TextArea(3,5)] public string itemDescription;

        [Space]
        public Sprite itemSprite;
        public GameObject itemShopPrefab;
        public List<SkillEffect> itemEffect = new List<SkillEffect>();

        public virtual void UseItem(Unit _unit)
        {
            SkillEffectLibrary.ResolveEffects(_unit, _unit, itemEffect);
        }
    }

}