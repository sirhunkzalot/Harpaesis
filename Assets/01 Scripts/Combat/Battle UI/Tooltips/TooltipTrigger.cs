using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Harpaesis.UI.Tooltips
{
    public abstract class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        protected string header;
        protected string body;
        protected string footer;


        bool mouseIsHover;
        [Header("Tooltip Settings")]
        public float tooltipDelay = .5f;

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseIsHover = true;
            SetHoverText();
            StartCoroutine(ShowToolTip());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mouseIsHover = false;
            TooltipSystem.Hide();
            StopCoroutine(ShowToolTip());
        }

        protected abstract void SetHoverText();

        IEnumerator ShowToolTip()
        {
            yield return new WaitForSeconds(tooltipDelay);
            if (mouseIsHover)
            {
                TooltipSystem.Show(body, header, footer);
            }
        }
    }

}