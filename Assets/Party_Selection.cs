using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party_Selection : MonoBehaviour
{
    public GameObject selectionOne;
    public GameObject selectionTwo;
    public GameObject selectionThree;
    public GameObject selectionFour;
    public GameObject selectionFive;

    public GameObject alexDesc;
    public GameObject doranDesc;
    public GameObject reginaDesc;
    public GameObject coriDesc;
    public GameObject joachimDesc;


    // Update is called once per frame
    void Update()
    {
        if (selectionOne.activeSelf)
        {
            selectionTwo.SetActive(false);
            selectionThree.SetActive(false);
            selectionFour.SetActive(false);
            selectionFive.SetActive(false);
            doranDesc.SetActive(false);
            coriDesc.SetActive(false);
            joachimDesc.SetActive(false);
            reginaDesc.SetActive(false);
        }
        if (selectionTwo.activeSelf)
        {
            selectionOne.SetActive(false);
            selectionThree.SetActive(false);
            selectionFour.SetActive(false);
            selectionFive.SetActive(false);
            doranDesc.SetActive(false);
            alexDesc.SetActive(false);
            joachimDesc.SetActive(false);
            reginaDesc.SetActive(false);
        }
        if (selectionThree.activeSelf)
        {
            selectionTwo.SetActive(false);
            selectionOne.SetActive(false);
            selectionFour.SetActive(false);
            selectionFive.SetActive(false);
            doranDesc.SetActive(false);
            coriDesc.SetActive(false);
            alexDesc.SetActive(false);
            reginaDesc.SetActive(false);
        }
        if (selectionFour.activeSelf)
        {
            selectionTwo.SetActive(false);
            selectionThree.SetActive(false);
            selectionOne.SetActive(false);
            selectionFive.SetActive(false);
            doranDesc.SetActive(false);
            coriDesc.SetActive(false);
            joachimDesc.SetActive(false);
            alexDesc.SetActive(false);
        }
        if (selectionFive.activeSelf)
        {
            selectionTwo.SetActive(false);
            selectionThree.SetActive(false);
            selectionFour.SetActive(false);
            selectionOne.SetActive(false);
            alexDesc.SetActive(false);
            coriDesc.SetActive(false);
            joachimDesc.SetActive(false);
            reginaDesc.SetActive(false);
        }

    }

        public void SwitchActiveCharacterOne()
        {
            selectionOne.SetActive(true);
            alexDesc.SetActive(true);
        }
    public void SwitchActiveCharacterTwo()
    {
        selectionTwo.SetActive(true);
        coriDesc.SetActive(true);
    }
    public void SwitchActiveCharacterThree()
    {
        selectionThree.SetActive(true);
        joachimDesc.SetActive(true);
    }
    public void SwitchActiveCharacterFour()
    {
        selectionFour.SetActive(true);
        reginaDesc.SetActive(true);
    }
    public void SwitchActiveCharacterFive()
    {
        selectionFive.SetActive(true);
        doranDesc.SetActive(true);
    }
}

