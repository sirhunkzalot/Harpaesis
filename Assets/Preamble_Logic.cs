using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Preamble_Logic : MonoBehaviour
{
    public float waitTime = 20f;

    public void Update()
    {
        waitTime -= Time.deltaTime;

        if(waitTime <= 0)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void I_Skip(InputAction.CallbackContext _ctx)
    {
        SceneManager.LoadScene(1);
    }
}
