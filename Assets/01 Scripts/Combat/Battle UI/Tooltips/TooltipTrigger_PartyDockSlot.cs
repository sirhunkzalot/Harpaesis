using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Harpaesis.UI.Tooltips
{
    public class TooltipTrigger_PartyDockSlot : TooltipTrigger
    {
        [Header("Party Dock")]
        public int slotIndex;
        FriendlyUnit myUnit;

        protected override void SetHoverText()
        {
            if (myUnit == null)
            {
                myUnit = TurnManager.instance.friendlyUnits[slotIndex];
            }

            body = $"HP: {myUnit.currentHP}/{myUnit.friendlyUnitData.healthStat}";
            header = myUnit.friendlyUnitData.nickname;
        }
    }
}
