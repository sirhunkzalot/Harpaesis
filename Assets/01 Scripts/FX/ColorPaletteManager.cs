using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPaletteManager : MonoBehaviour
{
    Material material;
    [ReadOnly, SerializeField] float transitionSpeed = 2.7f; 

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        ResetSprite();
    }

    public void CycleColor(ColorCode _colorCode, float _duration = .4f)
    {
        float _code = ((float)_colorCode) / 100;

        StopAllCoroutines();
        StartCoroutine(CycleColor(_code, _duration));
    }

    private IEnumerator CycleColor(float _code, float _duration)
    {
        material.SetFloat("_Speed", transitionSpeed);
        material.SetFloat("_Palette", _code);
        material.SetInteger("_UsePalette", 1);

        yield return new WaitForSeconds(_duration);

        ResetSprite();
    }

    private void ResetSprite()
    {
        material.SetInteger("_UsePalette", 0);
        material.SetFloat("_Speed", 0);
    }
}
public enum ColorCode
{
    Damage = 3,
    Magic = 32,
    Burn = 41,
    Bleed = 61,
    Healing = 84,
}