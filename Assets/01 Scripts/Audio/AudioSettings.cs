using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.AudioSystem
{

    public class AudioSettings : MonoBehaviour
    {
        private static readonly string BackgroundPref = "BackgroundPref";
        private static readonly string SoundEffectsPref = "SoundEffectsPref";
        private static readonly string MasterVolumePref = "MasterVolumePref";
        private float backgroundFloat, soundEffectsFloat;
        private float masterVolFloat;
        public AudioSource[] backgroundMusicAudio;
        public AudioSource[] soundEffectsAudio;

        private void Awake()
        {
            ContinueSettings();
        }

        private void ContinueSettings()
        {
            backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
            soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            masterVolFloat = PlayerPrefs.GetFloat(MasterVolumePref);


            for (int i = 0; i < backgroundMusicAudio.Length; i++)
            {
                backgroundMusicAudio[i].volume = backgroundFloat;
            }

            for (int i = 0; i < soundEffectsAudio.Length; i++)
            {
                soundEffectsAudio[i].volume = soundEffectsFloat;
            }

            AudioListener.volume = masterVolFloat;

        }
    }
}