using TMPro;
using Harpaesis.Combat;
using UnityEngine.UI;
using UnityEngine;

namespace Harpaesis.UI.Tooltips
{
    public class Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI headerText;
        public TextMeshProUGUI bodyText;
        public TextMeshProUGUI footerText;

        LayoutElement layoutElement;

        RectTransform rectTransform;
        PlayerInput_Combat input;

        public int characterWrapLimit = 80;

        private void Awake()
        {
            layoutElement = GetComponent<LayoutElement>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            input = PlayerInput_Combat.instance;
        }

        public void SetText(string _body, string _header, string _footerText)
        {
            if (string.IsNullOrEmpty(_header))
            {
                headerText.gameObject.SetActive(false);
            }
            else
            {
                headerText.gameObject.SetActive(true);
                headerText.text = _header;
            }

            if (string.IsNullOrEmpty(_body))
            {
                bodyText.gameObject.SetActive(false);
            }
            else
            {
                bodyText.gameObject.SetActive(true);
                bodyText.text = _body;
            }

            if (string.IsNullOrEmpty(_footerText))
            {
                footerText.gameObject.SetActive(false);
            }
            else
            {
                footerText.gameObject.SetActive(true);
                footerText.text = _footerText;
            }


            int _headerLength = headerText.text.Length;
            int _bodyLength = bodyText.text.Length;
            int _footerLength = footerText.text.Length;

            layoutElement.enabled = (_headerLength > characterWrapLimit || _bodyLength > characterWrapLimit || _footerLength > characterWrapLimit);
        }

        private void Update()
        {
            Vector2 _position = input.mousePosition;

            float _pivotX = _position.x / Screen.width * 1.1f;
            float _pivotY = _position.y / Screen.height * 1.1f;

            rectTransform.pivot = new Vector2(_pivotX, _pivotY);

            transform.position = _position;
        }
    }
}