using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public int sceneIndex = 0;

    public static LevelSelect instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
            sceneIndex = 1;
            SceneManager.LoadScene(sceneIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            sceneIndex = 2;
            SceneManager.LoadScene(sceneIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            sceneIndex = 3;
            SceneManager.LoadScene(sceneIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            sceneIndex = 4;
            SceneManager.LoadScene(sceneIndex);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            sceneIndex = 0;
            SceneManager.LoadScene(sceneIndex);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
