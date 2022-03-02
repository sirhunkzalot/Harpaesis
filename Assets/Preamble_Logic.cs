using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}
