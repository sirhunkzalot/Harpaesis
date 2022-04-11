using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilBloodFX : MonoBehaviour
{
    [SerializeField] ParticleSystem bloodEffect;

    private void Update()
    {
        if (!bloodEffect.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
