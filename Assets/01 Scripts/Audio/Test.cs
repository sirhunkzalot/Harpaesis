using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
    public AudioClip clipOne;
    public AudioClip clipTwo;

    public AudioClip musicOne;
    public AudioClip musicTwo;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.instance.PlaySFX(clipOne);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.instance.PlaySFX(clipTwo);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioManager.instance.PlayBGM(musicOne, 3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AudioManager.instance.PlayBGM(musicTwo, 3);
        }
    }
}
