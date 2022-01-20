using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public Dialogue[] dialogues;

    public bool activateDialogOnStart = false;

    private void Start()
    {
        if (activateDialogOnStart)
        {
            dialogues[0].gameObject.SetActive(true);
            dialogues[0].StartDialog();
        }
    }
}
