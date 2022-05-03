using System.Collections;
using Harpaesis.Combat;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Harpaesis.UI.Tooltips
{
    public class TooltipTrigger_SkillSlot : TooltipTrigger
    {
        [Header("Party Dock")]
        public int slotIndex;
        FriendlyUnit myUnit;

        protected override void SetHoverText()
        {
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

            header = _skill.skillName;
            body = _skill.skillDescription;

        }
    }
}