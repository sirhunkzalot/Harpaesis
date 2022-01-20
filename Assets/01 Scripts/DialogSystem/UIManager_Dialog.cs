using TMPro;
using UnityEngine;

namespace Harpaesis.UI
{
    public class UIManager_Dialog : MonoBehaviour
    {
        public GameObject dialogBox;

        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI dialogueText;

        public static UIManager_Dialog instance;

        private void Awake()
        {
            instance = this;
            EndText();
        }

        public static void StartText(string _speakerName = "")
        {
            SetText(_speakerName);
            instance.dialogBox.SetActive(true);
        }

        public static void SetText(string _speakerName = "", string _dialogue = "")
        {
            instance.speakerNameText.text = _speakerName;
            instance.dialogueText.text = _dialogue;
        }

        public static void EndText()
        {
            SetText();
            instance.dialogBox.SetActive(false);
        }
    }
}