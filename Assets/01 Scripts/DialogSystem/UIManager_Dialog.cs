using TMPro;
using System.Collections;
using UnityEngine;

namespace Harpaesis.UI
{
    public class UIManager_Dialog : MonoBehaviour
    {
        public GameObject dialogBox;

        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI dialogueText;

        Dialogue currentDialogue;

        [Header("Dialogue Display Settings")]
        public float timeBetweenCharacters = .06f;
        public float punctiationDelay = .25f;
        public float spaceDelay = .1f;

        bool skipLine;

        public static UIManager_Dialog instance;

        private void Awake()
        {
            instance = this;
            EndText();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                skipLine = true;
            }
        }

        public void StartText(Dialogue _currentDialogue)
        {
            currentDialogue = _currentDialogue;

            SetText(currentDialogue.lines[0].speaker.characterName);
            dialogBox.SetActive(true);
            StartCoroutine(Typewriter());
        }

        IEnumerator Typewriter()
        {
            int _lineIndex = 0;

            do
            {
                Dialogue_Line _currentLine = currentDialogue.lines[_lineIndex];
                _currentLine.OnStartLine.Invoke();
                int _characterIndex = 0;
                string _currentString = "";
                bool _lineFinished = false;
                float _currentDelay = 0;

                while (!(_lineFinished || skipLine))
                {
                    yield return new WaitForSeconds(_currentDelay);

                    char _nextChar = _currentLine.dialog[_characterIndex];

                    if (_nextChar == ' ')
                    {
                        _currentDelay = spaceDelay;
                    }
                    else if (_nextChar == '.' || _nextChar == '?' || _nextChar == '!' || _nextChar == ';' || _nextChar == ':' || _nextChar == ',')
                    {
                        _currentDelay = punctiationDelay;
                    }
                    else
                    {
                        _currentDelay = timeBetweenCharacters;
                    }

                    _currentString += _nextChar;
                    _characterIndex++;
                    SetText(_currentLine.speaker.characterName, _currentString);
                    _lineFinished = (_characterIndex >= _currentLine.dialog.Length);
                }

                _currentString = _currentLine.dialog;
                skipLine = false;
                SetText(_currentLine.speaker.characterName, _currentString);

                while (!skipLine)
                {
                    yield return new WaitForEndOfFrame();
                }
                skipLine = false;

            } while (++_lineIndex < currentDialogue.lines.Count);

            EndText();
        }

        public void SetText(string _speakerName = "", string _dialogue = "")
        {
            speakerNameText.text = _speakerName;
            dialogueText.text = _dialogue;
        }

        public void EndText()
        {
            SetText();
            dialogBox.SetActive(false);

            if(currentDialogue != null)
            {
                currentDialogue.OnEndDialog();
                currentDialogue = null;
            }
        }
    }
}