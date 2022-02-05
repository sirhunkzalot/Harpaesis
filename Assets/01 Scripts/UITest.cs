using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public GameObject settings;
    public GameObject mainSetings;
    public GameObject audioSettings;
    public GameObject Items;

    public AudioSource sfx;

    public void Start()
    {
        settings.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settings.SetActive(!settings.activeInHierarchy);
            audioSettings.SetActive(false);
            mainSetings.SetActive(true);
            sfx.Play();
        }
    }

    public void _Items()
    {
        if (Items.activeSelf)
        {
            Items.SetActive(false);
        }
        else
        {
            Items.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingsMenu()
    {
        if (settings.activeSelf)
        {
            settings.SetActive(false);
        }
        else settings.SetActive(true);
    }

}