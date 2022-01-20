using System.Collections;
using System.Collections.Generic;
using Harpaesis.UI;
using UnityEngine.Events;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public float timeBetweenCharacters = .05f;
    public bool skipLine;

    [Header("Before Dialog")]
    public UnityEvent OnStartDialog;

    [Space]
    public List<Dialogue_Line> lines = new List<Dialogue_Line>();

    [Header("After Dialogue")]
    public UnityEvent OnFinishDialog;

    public void StartDialog()
    {
        OnBeginDialog();
        UIManager_Dialog.StartText(lines[0].speaker.characterName);
        StartCoroutine(Typewriter());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipLine = true;
        }
    }

    IEnumerator Typewriter()
    {
        int _lineIndex = 0;

        do
        {
            Dialogue_Line _currentLine = lines[_lineIndex];
            _currentLine.OnStartLine.Invoke();
            int _characterIndex = 0;
            string _currentString = "";
            bool _lineFinished = false;

            while (!(_lineFinished || skipLine))
            {
                yield return new WaitForSeconds(timeBetweenCharacters);

                _currentString += _currentLine.dialog[_characterIndex++];
                UIManager_Dialog.SetText(_currentLine.speaker.characterName, _currentString);
                _lineFinished = (_characterIndex >= _currentLine.dialog.Length);
            }

            _currentString = _currentLine.dialog;
            skipLine = false;
            UIManager_Dialog.SetText(_currentLine.speaker.characterName, _currentString);

            while (!skipLine)
            {
                yield return new WaitForEndOfFrame();
            }
            skipLine = false;

        } while (++_lineIndex < lines.Count);

        OnEndDialog();
    }

    private void OnBeginDialog()
    {
        OnStartDialog.Invoke();
    }

    private void OnEndDialog()
    {
        OnFinishDialog.Invoke();
        gameObject.SetActive(false);
        UIManager_Dialog.EndText();
    }
}
