using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject Level1;
    public GameObject Level2;
    public void Start()
    {
        Level1.SetActive(false);
        Level2.SetActive(false);
    }
    public void PlayLevel1()
    {
        SceneManager.LoadScene("Test Map1");
    }
    public void PlayLevel2()
    {
        SceneManager.LoadScene("Test Map 2");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
