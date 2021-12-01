using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public static LevelSelect instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LevelLoadManager.LoadMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LevelLoadManager.LoadLevel(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LevelLoadManager.LoadLevel(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LevelLoadManager.LoadLevel(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LevelLoadManager.LoadLevel(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            LevelLoadManager.LoadLevel(5);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            LevelLoadManager.ReloadLevel();
        }
    }
}
