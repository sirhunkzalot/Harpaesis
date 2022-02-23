using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemButton : MonoBehaviour
{
    public GameObject itemMenu;
    public void ToggleMenu()
    {
        itemMenu.SetActive(!itemMenu.activeSelf);
    }
}
