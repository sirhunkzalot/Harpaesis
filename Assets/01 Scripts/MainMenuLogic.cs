using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public void LoadLevel(int _buildIndex)
    {
        SceneManager.LoadScene(_buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
