using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColorChange : MonoBehaviour
{
    public SpriteRenderer spr;
    public Color normalColor;
    public Color hitColorOne;
    public Color hitColorTwo;

    public void TakeDamage()
    {
        StartCoroutine(FlashDamage());
    }

    IEnumerator FlashDamage()
    {
        spr.material.color = hitColorOne;
        yield return new WaitForSeconds(.1f);
        spr.material.color = hitColorTwo;
        yield return new WaitForSeconds(.1f);
        spr.material.color = normalColor;
    }
}
