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
    public GameObject visualSettings;
    public GameObject Items;
    public GameObject controlsMenu;
    public GameObject optionsMenu;
    public GameObject loadSlots;
    public GameObject saveSlots;

    public AudioSource sfx;

    public Save_Sots saveUI;
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
            visualSettings.SetActive(false);
            mainSetings.SetActive(true);
            controlsMenu.SetActive(false);
            optionsMenu.SetActive(false);
            saveSlots.SetActive(false);
            loadSlots.SetActive(false);
            saveUI.saveSlotOne = false;
            saveUI.saveSlotTwo = false;
            saveUI.saveSlotThree = false;
            saveUI.saveSlotFour = false;

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