using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine.UI;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    public int slotIndex;
    FriendlyUnit myUnit;
    public Image icon;

    public void UpdateSkillSlot()
    {
        if (TurnManager.instance.activeTurn.unit.GetType() != typeof(FriendlyUnit)) return;

        myUnit = (FriendlyUnit)TurnManager.instance.activeTurn.unit;

        Skill _skill = null;

        switch (slotIndex)
        {
            case 0:
                if (!myUnit.alternativeWeapon)
                {
                    _skill = myUnit.friendlyUnitData.basicAttack;
                }
                else
                {
                    _skill = myUnit.friendlyUnitData.alternativeAttack;
                }
                break;
            case 1:
                _skill = myUnit.friendlyUnitData.primarySkill;
                break;
            case 2:
                _skill = myUnit.friendlyUnitData.secondarySkill;
                break;
            case 3:
                _skill = myUnit.friendlyUnitData.tertiarySkill;
                break;
            case 4:
                _skill = myUnit.friendlyUnitData.signatureSkill;
                break;
        }

        icon.sprite = _skill.skillSprite;
    }
}
