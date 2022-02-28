using System.Collections;
using System.Collections.Generic;
using Harpaesis.Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OverworldTestingScenes : MonoBehaviour
{
    public GameObject settings;
    public GameObject mainSetings;
    public GameObject audioSettings;
    public GameObject visualSettings;
    public GameObject tips;
    public GameObject optionsMenu;
    public GameObject controlsMenu;
    public GameObject loadSlots;
    public GameObject saveSlots;

    public float waitTime = 5f;
    public AudioSource sfx;

    public Save_Slots saveUI;

    public PlayerInput_Overworld input;

    public static OverworldTestingScenes instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        input = PlayerInput_Overworld.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime >= 0)
        {
            waitTime -= Time.deltaTime;
        }

        if(waitTime <= 0)
        {
            tips.SetActive(false);
            waitTime = 0f;
        }
    }

    public void Pause()
    {
        settings.SetActive(!settings.activeInHierarchy);
        audioSettings.SetActive(false);
        visualSettings.SetActive(false);
        mainSetings.SetActive(true);
        controlsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        loadSlots.SetActive(false);
        saveSlots.SetActive(false);
        saveUI.saveSlotOne = false;
        saveUI.saveSlotTwo = false;
        saveUI.saveSlotThree = false;
        saveUI.saveSlotFour = false;
        sfx.Play();
    }

    public void ReturnOverworld()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
