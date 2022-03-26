using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip_Custscene : MonoBehaviour
{
    public int LoadLevelIndex;

    public void SkipCustscene()
    {
        SceneManager.LoadScene(LoadLevelIndex);
    }

}
