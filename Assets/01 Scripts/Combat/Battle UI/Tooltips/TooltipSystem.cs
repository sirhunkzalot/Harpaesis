using UnityEngine;

namespace Harpaesis.UI.Tooltips
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem instance;

        public Tooltip tooltip;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Hide();
        }

        public static void Show(string _body, string _header, string _footer)
        {
            instance.tooltip.SetText(_body, _header, _footer);
            instance.tooltip.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            instance.tooltip.gameObject.SetActive(false);
        }
    }
}