using UnityEngine.SceneManagement;
using UnityEngine;

public static class LevelLoadManager
{
    public static void LoadLevel(int _levelIndex)
    {
        SceneManager.LoadSceneAsync(_levelIndex);
    }

    public static void LoadLevel(string _levelName)
    {
        SceneManager.LoadSceneAsync(_levelName);
    }

    public static void LoadMainMenu()
    {
        LoadLevel("Main menu idea");
    }

    public static void ReloadLevel()
    {
        int _levelIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(_levelIndex);
    }

    public static void LoadOverworld()
    {
        LoadLevel("OverworldTesting");
    }
}
