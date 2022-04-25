using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DebugApril25Only : MonoBehaviour
{
    public static DebugApril25Only instance;

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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneManager.LoadScene("Main menu idea");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SceneManager.LoadScene("OverworldTesting");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SceneManager.LoadScene("Dia_City_Rework_New");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SceneManager.LoadScene("Dia_WolfDen");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SceneManager.LoadScene("Dia_Outside");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SceneManager.LoadScene("Dia_Cathedral Inside");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SceneManager.LoadScene("Dia_CityRuins");
            }
        }
    }
}
