using System.Collections;
using System.Collections.Generic;
using Harpaesis.Inventory;
using Harpaesis.Combat;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    //public int index;

    public GameObject useButton;
    private bool isHovering;

    PlayerInput_Combat input;

    private void Start()
    {
        input = PlayerInput_Combat.instance;
    }

    public void ActivateGameObject(int _index)
    {
        if (PartyInventory.inventory[_index] != null)
        {
            useButton.SetActive(true);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (useButton.activeSelf && !isHovering)
        {
            useButton.SetActive(false);
        }
    }
}
