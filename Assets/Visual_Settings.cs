using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Visual_Settings : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;

    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    public TMP_Text resolutionLabel;
    // Start is called before the first frame update
    void Start()
    {
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

    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if(selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " X " + resolutions[selectedResolution].vertical.ToString();
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

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }

}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
