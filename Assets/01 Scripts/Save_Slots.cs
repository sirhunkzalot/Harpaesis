using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class Save_Slots : MonoBehaviour
{
    public bool saveSlotOne;
    public bool saveSlotTwo;
    public bool saveSlotThree;
    public bool saveSlotFour;

    public TMP_Text txtSaveSlotOne;
    public TMP_Text txtSaveSlotTwo;
    public TMP_Text txtSaveSlotThree;
    public TMP_Text txtSaveSlotFour;

    public TMP_InputField saveSlotInput;

    public GameObject saveSlotSelectedOne;
    public GameObject saveSlotSelectedTwo;
    public GameObject saveSlotSelectedThree;
    public GameObject saveSlotSelectedFour;
    // Start is called before the first frame update

    public void Update()
    {
        if (saveSlotOne == false)
        {
            saveSlotSelectedOne.SetActive(false);
        }
        if (saveSlotTwo == false)
        {
            saveSlotSelectedTwo.SetActive(false);
        }
        if (saveSlotThree == false)
        {
            saveSlotSelectedThree.SetActive(false);
        }
        if (saveSlotFour == false)
        {
            saveSlotSelectedFour.SetActive(false);
        }
    }
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
        if (saveSlotOne == true)
        {
            saveSlotInput.text = txtSaveSlotOne.text;
        }
        saveSlotSelectedOne.SetActive(true);

        if(saveSlotTwo == true)
        {
            saveSlotTwo = false;
            saveSlotSelectedTwo.SetActive(false);
}
        if (saveSlotThree == true)
        {
            saveSlotThree = false;
            saveSlotSelectedThree.SetActive(false);
        }
        if (saveSlotFour == true)
        {
            saveSlotFour = false;
            saveSlotSelectedFour.SetActive(false);
        }
    }
    public void EnableSlotTwo()
    {
        saveSlotTwo = true;
        if(saveSlotTwo == true)
        { 
            saveSlotInput.text = txtSaveSlotTwo.text;
        }
        saveSlotSelectedTwo.SetActive(true);

        if (saveSlotOne == true)
        {
            saveSlotOne = false;
            saveSlotSelectedOne.SetActive(false);
        }
        if (saveSlotThree == true)
        {
            saveSlotThree = false;
            saveSlotSelectedThree.SetActive(false);
        }
        if (saveSlotFour == true)
        {
            saveSlotFour = false;
            saveSlotSelectedFour.SetActive(false);
        }
    }
    public void EnableSlotThree()
    {
        saveSlotThree = true;
        if (saveSlotThree == true)
        {
            saveSlotInput.text = txtSaveSlotThree.text;
        }
        saveSlotSelectedThree.SetActive(true);


        if (saveSlotTwo == true)
        {
            saveSlotTwo = false;
            saveSlotSelectedTwo.SetActive(false);
        }
        if (saveSlotOne == true)
        {
            saveSlotOne = false;
            saveSlotSelectedOne.SetActive(false);
        }
        if (saveSlotFour == true)
        {
            saveSlotFour = false;
            saveSlotSelectedFour.SetActive(false);
        }
    }
    public void EnableSlotFour()
    {
        saveSlotFour = true;
        if (saveSlotFour == true)
        {
            saveSlotInput.text = txtSaveSlotFour.text;
        }
        saveSlotSelectedFour.SetActive(true);

        if (saveSlotTwo == true)
        {
            saveSlotTwo = false;
            saveSlotSelectedTwo.SetActive(false);
        }
        if (saveSlotThree == true)
        {
            saveSlotThree = false;
            saveSlotSelectedThree.SetActive(false);
        }
        if (saveSlotOne == true)
        {
            saveSlotOne = false;
            saveSlotSelectedOne.SetActive(false);
        }
    }
    public void _Back()
    {
        saveSlotOne = false;
        saveSlotTwo = false;
        saveSlotThree = false;
        saveSlotFour = false;
        
    }

    public void SaveFile()
    {
        Game_Data data = new Game_Data();
        Save_Load.SaveGame(saveSlotInput.text, data);
    }
}
