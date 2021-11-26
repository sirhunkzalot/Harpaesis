using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public List<GameObject> dialogs = new List<GameObject>();

    private void Start()
    {
        SetActive(0);
    }

    public void SetActive(int _index)
    {
        // Throws error if the index is outside the expected range
        if(_index >= dialogs.Count)
        {
            throw new System.Exception($"DialogManager's list of dialogs does not contain an index of {_index}." +
                $" Maximum allowed index is {dialogs.Count - 1}.");
        }

        for (int i = 0; i < dialogs.Count; i++)
        {
            if(i == _index)
            {
                dialogs[i].SetActive(true);
            }
            else
            {
                dialogs[i].SetActive(false);
            }
        }
    }
}
