using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public GameObject settings;

    public GameObject Items;
    //true = weapon 1, false = weapon 2
    public bool swapWeapon;
    public Text weaponDescription;

    public void Start()
    {
        settings.SetActive(false);
    }
    public void _Items()
    {
        if (Items.activeSelf)
        {
            Items.SetActive(false);
        }
        else Items.SetActive(true);

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Test Map1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Test Map 2");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settings.activeSelf)
            {
                settings.SetActive(false);
            }
            else settings.SetActive(true);
        }
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main menu idea");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void _SwapWeapon()
    {
        if(swapWeapon == true)
        {
            swapWeapon = false;
            weaponDescription.text = "weapon 1";
        }
        else
        {
            swapWeapon = true;
            weaponDescription.text = "weapon 2";
        }
    }
}