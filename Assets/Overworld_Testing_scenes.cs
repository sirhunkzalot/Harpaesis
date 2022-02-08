using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overworld_Testing_scenes : MonoBehaviour
{
    //public GameObject settings;
    //public GameObject mainSetings;
    //public GameObject audioSettings;
    //public GameObject visualSettings;
    public GameObject tips;

    public float waitTime = 5f;
    public AudioSource sfx;

    // Update is called once per frame
    void Update()
    {
        waitTime -= Time.deltaTime;

        if(waitTime <= 0)
        {
            tips.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //settings.SetActive(!settings.activeInHierarchy);
            //audioSettings.SetActive(false);
            //visualSettings.SetActive(false);
            //mainSetings.SetActive(true);
            sfx.Play();
            returnToMenu();
        }
    }
    public void _RetrurnOverworld()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
