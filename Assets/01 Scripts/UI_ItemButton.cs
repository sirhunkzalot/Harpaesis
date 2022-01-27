using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemButton : MonoBehaviour
{
    public GameObject itemMenu;
    public void CloseMenu()
    {
        if (itemMenu.activeSelf)
        {
            itemMenu.SetActive(false);
        }
        else
        {
            itemMenu.SetActive(true);
        }
    }
}
