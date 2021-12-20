using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine;

public class Unit_UI_EffectsManager : MonoBehaviour
{
    public GameObject bleedEffect, burnEffect, fearEffect, sleepEffect, holyEffect,
        apUpEffect, atkUpEffect, atkDownEffect, defUpEffect, defDownEffect, entangleEffect,
        defendEffect;

    private void Start()
    {
        bleedEffect.SetActive(false);
        burnEffect.SetActive(false);
        fearEffect.SetActive(false);
        sleepEffect.SetActive(false);
        holyEffect.SetActive(false);
        apUpEffect.SetActive(false);
        atkUpEffect.SetActive(false);
        atkDownEffect.SetActive(false);
        defUpEffect.SetActive(false);
        defDownEffect.SetActive(false);
        entangleEffect.SetActive(false);
        defendEffect.SetActive(false);
    }

    public void ActivateEffect(StatusEffectType _effect)
    {
        switch (_effect)
        {
            case StatusEffectType.Bleed:
                bleedEffect.SetActive(true);
                break;
            case StatusEffectType.Burn:
                burnEffect.SetActive(true);
                break;
            case StatusEffectType.Fear:
                fearEffect.SetActive(true);
                break;
            case StatusEffectType.Sleep:
                sleepEffect.SetActive(true);
                break;
            case StatusEffectType.Holy:
                holyEffect.SetActive(true);
                break;
            case StatusEffectType.AP_Up:
                apUpEffect.SetActive(true);
                break;
            case StatusEffectType.ATK_Up:
                atkUpEffect.SetActive(true);
                break;
            case StatusEffectType.ATK_Down:
                atkDownEffect.SetActive(true);
                break;
            case StatusEffectType.DEF_Up:
                defUpEffect.SetActive(true);
                break;
            case StatusEffectType.DEF_Down:
                defDownEffect.SetActive(true);
                break;
            case StatusEffectType.Root:
                entangleEffect.SetActive(true);
                break;
            case StatusEffectType.Defend:
                defendEffect.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DeactivateEffect(StatusEffectType _effect)
    {
        switch (_effect)
        {
            case StatusEffectType.Bleed:
                bleedEffect.SetActive(false);
                break;
            case StatusEffectType.Burn:
                burnEffect.SetActive(false);
                break;
            case StatusEffectType.Fear:
                fearEffect.SetActive(false);
                break;
            case StatusEffectType.Sleep:
                sleepEffect.SetActive(false);
                break;
            case StatusEffectType.Holy:
                holyEffect.SetActive(false);
                break;
            case StatusEffectType.AP_Up:
                apUpEffect.SetActive(false);
                break;
            case StatusEffectType.ATK_Up:
                atkUpEffect.SetActive(false);
                break;
            case StatusEffectType.ATK_Down:
                atkDownEffect.SetActive(false);
                break;
            case StatusEffectType.DEF_Up:
                defUpEffect.SetActive(false);
                break;
            case StatusEffectType.DEF_Down:
                defDownEffect.SetActive(false);
                break;
            case StatusEffectType.Root:
                entangleEffect.SetActive(false);
                break;
            case StatusEffectType.Defend:
                defendEffect.SetActive(false);
                break;
            default:
                break;
        }
    }
}