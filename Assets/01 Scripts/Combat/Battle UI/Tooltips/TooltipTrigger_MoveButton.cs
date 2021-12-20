using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Harpaesis.UI.Tooltips
{
    public class TooltipTrigger_MoveButton : TooltipTrigger
    {
        TurnManager turnManager;

        protected override void SetHoverText()
        {
            if(turnManager == null)
            {
                turnManager = TurnManager.instance;
            }

            FriendlyUnit _unit = (FriendlyUnit)turnManager.activeTurn.unit;

            if (_unit.HasEffect(Combat.StatusEffectType.Root))
            {
                header = "Entangled!";
                body = "Unit cannot move while entangled.";
            }
            else if (_unit.turnData.ap == 0)
            {
                header = "No AP";
                body = "Unit does not enough Action Points to move.";
            }
            else
            {
                header = "Move";
                body = "Moves unit at the cost of one Action Point per space.";
            }
        }
    }
}