using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreferences : MonoBehaviour
{
    public static PlayerPreferences instance;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private float _musicVolume;
    private float _sfxVolume;

    //not really sure why he is doing this instead of making it public but idk
    public float MusicVolume { get { return _musicVolume; } }
    public float SfxVolume { get { return _sfxVolume; } }

    private void Awake()
    {
        //destroys duplicates
        if (PlayerPreferences.instance == null)
        {
            PlayerPreferences.instance = this;
        }
        else if (PlayerPreferences.instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        _sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);
    }


    //THIS NEEDS TO BE CHANGED HOLY SHIT BRO WHY IS HE PUTTING THIS IN UPDATE
    private void Update()
    {
        _musicVolume = _musicSlider.value;
        _sfxVolume = _sfxSlider.value;

        PlayerPrefs.SetFloat("musicVolume", _musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", _sfxVolume);
    }
}
