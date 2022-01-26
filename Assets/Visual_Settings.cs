using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Visual_Settings : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;

    private int selectedResolution;
    Resolution[] resolutions;

    public TMP_Text debugLabel;
    public TMP_Dropdown resolutionDropdown;
    // Start is called before the first frame update
    void Start()
    {
        InitDropdown();
        fullscreenTog.isOn = Screen.fullScreen;
        
        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void InitDropdown()
    {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData($"{resolutions[i].width} x {resolutions[i].height}"));

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                resolutionDropdown.value = i;
            }
        }
      
        
    }

 

   

    public void ApplyChanges()
    {
        //Screen.fullScreen = fullscreenTog.isOn;

        if(vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, fullscreenTog.isOn);
    }

}


