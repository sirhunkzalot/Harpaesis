using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Harpaesis.AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        private static readonly string FirstPlay = "FirstPlay";
        private static readonly string BackgroundPref = "BackgroundPref";
        private static readonly string SoundEffectsPref = "SoundEffectsPref";
        private static readonly string MasterVolumePref = "MasterVolumePref";
        private int firstPlayInt;
        public Slider backgroundSlider, soundEffectsSlider, masterVolSlider;
        private float backgroundFloat, soundEffectsFloat;
        private float masterVolFloat;
        public AudioSource[] backgroundMusicAudio;
        public AudioSource[] soundEffectsAudio;


        void Start()
        {
            firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

            if (firstPlayInt == 0)
            {
                backgroundFloat = .50f;
                soundEffectsFloat = .50f;
                masterVolFloat = 1.0f;
                backgroundSlider.value = backgroundFloat;
                soundEffectsSlider.value = soundEffectsFloat;
                masterVolSlider.value = masterVolFloat;
                PlayerPrefs.SetFloat(BackgroundPref, backgroundFloat);
                PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
                PlayerPrefs.SetFloat(MasterVolumePref, masterVolFloat);
                PlayerPrefs.SetInt(FirstPlay, -1);
            }
            else
            {
                backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
                backgroundSlider.value = backgroundFloat;
                soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
                soundEffectsSlider.value = soundEffectsFloat;
                masterVolFloat = PlayerPrefs.GetFloat(MasterVolumePref);
                masterVolSlider.value = masterVolFloat;
            }
        }

        public void SaveSoundSettings()
        {
            PlayerPrefs.SetFloat(BackgroundPref, backgroundSlider.value);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
            PlayerPrefs.SetFloat(MasterVolumePref, masterVolSlider.value);
        }

        private void OnApplicationFocus(bool inFocus)
        {
            if (!inFocus)
            {
                SaveSoundSettings();
            }
        }

        public void UpdateSound()
        {
            for (int i = 0; i < backgroundMusicAudio.Length; i++)
            {
                if(backgroundMusicAudio[i] != null)
                {
                    backgroundMusicAudio[i].volume = backgroundSlider.value;
                }
            }

            for (int i = 0; i < soundEffectsAudio.Length; i++)
            {
                if (soundEffectsAudio[i] != null)
                {
                    soundEffectsAudio[i].volume = soundEffectsSlider.value;
                }
            }

            AudioListener.volume = masterVolSlider.value;
        }
    }
}

