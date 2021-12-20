using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.UI.Tooltips
{
    public class TooltipTrigger_Standard : TooltipTrigger
    {
        [Header("Tooltip Contents")]
        public string headerText;
        [TextArea(3, 5)] public string bodyText;

        protected override void SetHoverText()
        {
            header = headerText;
            body = bodyText;
        }
    }
}
