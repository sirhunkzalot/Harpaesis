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

        public static void Show(string _body, string _header)
        {
            instance.tooltip.SetText(_body, _header);
            instance.tooltip.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            instance.tooltip.gameObject.SetActive(false);
        }
    }
}