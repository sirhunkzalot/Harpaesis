using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public struct Dialogue_Line
{
    public string lineDescription;
    public Character speaker;
    [TextArea(3, 5)] public string dialog;
    public UnityEvent OnFinishLine;
}