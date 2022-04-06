using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugApril06Only : MonoBehaviour
{
    public static DebugApril06Only instance;

    int currentIndex = 11;

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentIndex = 10;
            UnityEngine.SceneManagement.SceneManager.LoadScene(10);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentIndex = 4;
            UnityEngine.SceneManagement.SceneManager.LoadScene(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentIndex = 11;
            UnityEngine.SceneManagement.SceneManager.LoadScene(11);
        }
    }
}
