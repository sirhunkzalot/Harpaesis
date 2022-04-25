using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Forest_Intro : MonoBehaviour
{
    public float waitTime = 70f;

    public GameObject paragraphOne;
    public GameObject paragraphTwo;
    public GameObject paragraphThree;
    // Update is called once per frame
    void Update()
    {
        waitTime -= Time.deltaTime;

        if(waitTime <= 0)
        {
            SceneManager.LoadScene("OverworldTesting");
        }
    }


}
