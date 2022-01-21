using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue startingDialogue;

    public bool activateDialogOnStart = false;

    private void Start()
    {
        if (activateDialogOnStart)
        {
            if(startingDialogue == null)
            {
                throw new System.Exception("Null Assignment Error: No Starting Dialogue Assigned in the Dialogue Manager");
            }
            ActivateDialog(startingDialogue);
        }
    }

    public void ActivateDialog(Dialogue _dialogue)
    {
        _dialogue.gameObject.SetActive(true);
        _dialogue.StartDialog();
    }
}
