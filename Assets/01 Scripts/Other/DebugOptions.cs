using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DebugOptions : MonoBehaviour
{
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Break();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 5f;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            Time.timeScale = 1f;
        }
#endif

    }
}
