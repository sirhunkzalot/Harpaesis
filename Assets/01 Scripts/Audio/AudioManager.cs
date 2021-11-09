using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private int _sfxSourceLength;

    private AudioSource _bgm;
    private AudioSource[] _sfxSources;
    private int _curSFXIndex = 0;

    private void Awake()
    {
        //Destroys duplicate audioManagers
        if(AudioManager.instance == null)
        {
            AudioManager.instance = this;
        }
        else if (AudioManager.instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _bgm = gameObject.AddComponent<AudioSource>();
        //makes the index length the same length as the length of the sfx source, and sets the spacial blend to 0?
        _sfxSources = new AudioSource[_sfxSourceLength];

        for (int i = 0; i < _sfxSourceLength; i++)
        {
            _sfxSources[i] = gameObject.AddComponent<AudioSource>();
            _sfxSources[i].spatialBlend = 0;
        }
    }

    public void PlaySFX(AudioClip clipToPlay)
    {
        //plays the sfx and sets the volume according to player prefs
        _sfxSources[_curSFXIndex].clip = clipToPlay;
        _sfxSources[_curSFXIndex].volume = PlayerPreferences.instance.SfxVolume;
        _sfxSources[_curSFXIndex].Play();

        _curSFXIndex++;
        if(_curSFXIndex > _sfxSourceLength-1)
        {
            _curSFXIndex = 0;
        }
    }

    public void PlayBGM(AudioClip musicToPlay, float fadeDuration)
    {
        //starts the Play BGM coroutine
        StartCoroutine(PlayBGMCo(musicToPlay, fadeDuration));
    }

    private IEnumerator PlayBGMCo(AudioClip musicToPlay, float fadeDuration)
    {
        //tracks how much time has passed inside this coroutine
        float t = 0;

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = musicToPlay;
        newSource.Play();

        //fades out one music clip and fades in the next
        while(t < fadeDuration)
        {
            t += Time.deltaTime;
            _bgm.volume = Mathf.Lerp(1, 0, t / fadeDuration);
            _bgm.volume = Mathf.Lerp(0, 1, t / fadeDuration);
            newSource.volume = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        Destroy(_bgm);
        _bgm = newSource;
    }
}
