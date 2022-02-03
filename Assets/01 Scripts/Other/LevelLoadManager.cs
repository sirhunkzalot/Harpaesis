using UnityEngine.SceneManagement;
using UnityEngine;

public static class LevelLoadManager
{
    public static void LoadLevel(int _levelIndex)
    {
        SceneManager.LoadSceneAsync(_levelIndex);
    }

    public static void LoadMainMenu()
    {
        LoadLevel(1);
    }

    public static void ReloadLevel()
    {
        int _levelIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(_levelIndex);
    }

    public static void LoadOverworld()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
