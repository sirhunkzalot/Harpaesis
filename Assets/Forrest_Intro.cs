using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Forrest_Intro : MonoBehaviour
{
    public float waitTime = 70f;
    // Update is called once per frame
    void Update()
    {
        waitTime -= Time.deltaTime;

        if(waitTime <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
}
