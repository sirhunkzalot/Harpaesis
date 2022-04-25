using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip_Cutscene : MonoBehaviour
{
    public string LoadLevelIndex;

    public void SkipCustscene()
    {
        SceneManager.LoadScene(LoadLevelIndex);
    }

}
