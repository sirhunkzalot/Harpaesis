using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro_Wait : MonoBehaviour
{
    public float introTimer = 4.8f;


    // Update is called once per frame
    void Update()
    {
        introTimer -= Time.deltaTime;

        if(introTimer <= 0 )
        {
            SceneManager.LoadScene("Main menu idea");
        }
    }
}
