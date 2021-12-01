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
        weaponDescription.text = "Current Weapon: Weapon 1";

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
    public void _SwapWeapon()
    {
        if(swapWeapon == true)
        {
            swapWeapon = false;
            weaponDescription.text = "Current Weapon: weapon 1";
        }
        else
        {
            swapWeapon = true;
            weaponDescription.text = "Current Weapon: weapon 2";
        }
        
    }

}