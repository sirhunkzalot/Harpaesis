using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Forrest_Intro : MonoBehaviour
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
            SceneManager.LoadScene(2);
        }
    }

    public void SkipLines()
    {
        if(paragraphOne.activeSelf)
        {
            paragraphOne.SetActive(false);
        }
        if (paragraphTwo.activeSelf)
        {
            paragraphTwo.SetActive(false);
        }
        if (paragraphThree.activeSelf)
        {
            paragraphThree.SetActive(false);
        }

        SceneManager.LoadScene(2);
    }
}
