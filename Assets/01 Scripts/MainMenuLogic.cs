using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject creditsUI;
    public GameObject levelUI;
    public AudioSource uiSFX;
    public void LoadLevel(int _buildIndex)
    {
        SceneManager.LoadScene(_buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            if(creditsUI.activeSelf)
            {
                creditsUI.SetActive(false);
                mainUI.SetActive(true);
                uiSFX.Play();
            }
           
            if(levelUI.activeSelf)
            {
                levelUI.SetActive(false);
                mainUI.SetActive(true);
                uiSFX.Play();
            }
        }
    }

    public void PlayOverworld()
    {
        SceneManager.LoadScene(2);
    }
}
