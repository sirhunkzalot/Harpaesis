using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [Header("Before Dialog")]
    public UnityEvent OnStartDialog;

    [Space]
    public List<Dialogue_Line> dialogue = new List<Dialogue_Line>();

    [Header("After Dialogue")]
    public UnityEvent OnFinishDialog;

    private void OnBeginDialog()
    {
        OnStartDialog.Invoke();
    }

    private void OnEndDialog()
    {
        OnFinishDialog.Invoke();
    }
}
