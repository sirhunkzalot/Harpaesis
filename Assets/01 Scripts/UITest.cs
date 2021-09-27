using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{

    public GameObject Items;
    public void _Items()
    {
        if (Items.activeSelf)
        {
            Items.SetActive(false);
        }
        else Items.SetActive(true);
    }
}