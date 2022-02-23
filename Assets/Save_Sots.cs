using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class Save_Sots : MonoBehaviour
{
    public bool saveSlotOne;
    public bool saveSlotTwo;
    public bool saveSlotThree;
    public bool saveSlotFour;

    public TMP_Text txtSaveSlotOne;
    public TMP_Text txtSaveSlotTwo;
    public TMP_Text txtSaveSlotThree;
    public TMP_Text txtSaveSlotFour;

    public TMP_Text saveSlotInput;
    // Start is called before the first frame update

   
    public void UpdateName()
    {
        if(saveSlotOne == true)
        {
            txtSaveSlotOne.text = saveSlotInput.text;
            saveSlotOne = false;
        }
        if (saveSlotTwo == true)
        {
            txtSaveSlotTwo.text = saveSlotInput.text;
            saveSlotTwo = false;
        }
        if (saveSlotThree == true)
        {
            txtSaveSlotThree.text = saveSlotInput.text;
            saveSlotThree = false;
        }
        if (saveSlotFour == true)
        {
            txtSaveSlotFour.text = saveSlotInput.text;
            saveSlotFour = false;
        }
    }

   public void EnableSlotOne()
    {
        saveSlotOne = true;
        saveSlotInput.text = txtSaveSlotOne.text;
        print("Save Slot 1");

        if(saveSlotTwo == true)
        {
            saveSlotTwo = false;
        }
        if (saveSlotThree == true)
        {
            saveSlotThree = false;
        }
        if (saveSlotFour == true)
        {
            saveSlotFour = false;
        }
    }
    public void EnableSlotTwo()
    {
        saveSlotTwo = true;
        saveSlotInput.text = txtSaveSlotTwo.text;


        if (saveSlotOne == true)
        {
            saveSlotOne = false;
        }
        if (saveSlotThree == true)
        {
            saveSlotThree = false;
        }
        if (saveSlotFour == true)
        {
            saveSlotFour = false;
        }
    }
    public void EnableSlotThree()
    {
        saveSlotThree = true;
        saveSlotInput.text = txtSaveSlotThree.text;


        if (saveSlotTwo == true)
        {
            saveSlotTwo = false;
        }
        if (saveSlotOne == true)
        {
            saveSlotOne = false;
        }
        if (saveSlotFour == true)
        {
            saveSlotFour = false;
        }
    }
    public void EnableSlotFour()
    {
        saveSlotFour = true;
        saveSlotInput.text = txtSaveSlotFour.text;


        if (saveSlotTwo == true)
        {
            saveSlotTwo = false;
        }
        if (saveSlotThree == true)
        {
            saveSlotThree = false;
        }
        if (saveSlotOne == true)
        {
            saveSlotOne = false;
        }
    }
    public void ApplyNameChanges()
    {
        saveSlotInput.text = "";
    }
}
