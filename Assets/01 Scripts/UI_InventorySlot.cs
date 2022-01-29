using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Harpaesis.Inventory;


public class UI_InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public int index;

    public GameObject useButton;
    private bool isHovering;

    public void ActivateGameObject(int _index)
    {
        if (PartyInventory.inventory[_index] != null)
        {
            useButton.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && useButton.activeSelf && !isHovering)
        {
            useButton.SetActive(false);

        }


    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
