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
            footer = "";

            int _numSpaces = Mathf.Min(80, _skill.skillDescription.Length);

            for (int i = 0; i < _numSpaces; i++)
            {
                footer += " ";
            }

            if (_skill.IsOnCooldown)
            {
                body += $"\nOn cooldown for {_skill.cooldownTimer} turn";
                body += (_skill.cooldownTimer > 1) ? "s": "";
            }

            if(myUnit.turnData.ap < _skill.apCost)
            {
                footer += $"<color=\"red\">AP: {_skill.apCost} </color=\"red\">";
            }
            else
            {
                footer += $"AP: {_skill.apCost} ";
            }
        }
    }
}