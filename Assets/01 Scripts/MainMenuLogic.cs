using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject creditsUI;
    public GameObject levelUI;
    public GameObject settings;
    public AudioSource uiSFX;
    public void LoadLevel(int _buildIndex)
    {
        SceneManager.LoadScene(_buildIndex);
    }
    public void ForestIntro()
    {
        SceneManager.LoadScene("Forest Intro");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReturnToMenu()
    { 
            if(creditsUI.activeSelf || levelUI.activeSelf || settings.activeSelf)
            {
                creditsUI.SetActive(false);
                settings.SetActive(false);
                levelUI.SetActive(false);
                mainUI.SetActive(true);
                uiSFX.Play();
            }
    }

    public void PlayOverworld()
    {
        SceneManager.LoadScene(2);
    }
}
