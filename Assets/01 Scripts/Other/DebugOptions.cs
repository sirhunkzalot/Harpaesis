using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DebugOptions : MonoBehaviour
{
    public GameObject speedUpIcon;
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
            speedUpIcon.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            Time.timeScale = 1f;
            speedUpIcon.SetActive(false);
        }
#endif

    }
}
