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

    public void ActivateEffect(string _effectName)
    {
        string _effectNameLowerCase = _effectName.ToLower();

        switch (_effectNameLowerCase)
        {
            case "bleed":
                bleedEffect.SetActive(true);
                break;
            case "burn":
                burnEffect.SetActive(true);
                break;
            case "fear":
                fearEffect.SetActive(true);
                break;
            case "sleep":
                sleepEffect.SetActive(true);
                break;
            case "holy":
                holyEffect.SetActive(true);
                break;
            case "apup":
                apUpEffect.SetActive(true);
                break;
            case "atkup":
                atkUpEffect.SetActive(true);
                break;
            case "atkdown":
                atkDownEffect.SetActive(true);
                break;
            case "defup":
                defUpEffect.SetActive(true);
                break;
            case "defdown":
                defDownEffect.SetActive(true);
                break;
            case "root":
                entangleEffect.SetActive(true);
                break;
            case "defend":
                defendEffect.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DeactivateEffect(string _effectName)
    {
        string _effectNameLowerCase = _effectName.ToLower();

        switch (_effectNameLowerCase)
        {
            case "bleed":
                bleedEffect.SetActive(false);
                break;
            case "burn":
                burnEffect.SetActive(false);
                break;
            case "fear":
                fearEffect.SetActive(false);
                break;
            case "sleep":
                sleepEffect.SetActive(false);
                break;
            case "holy":
                holyEffect.SetActive(false);
                break;
            case "apUp":
                apUpEffect.SetActive(false);
                break;
            case "atkUp":
                atkUpEffect.SetActive(false);
                break;
            case "atkDown":
                atkDownEffect.SetActive(false);
                break;
            case "defUp":
                defUpEffect.SetActive(false);
                break;
            case "defDown":
                defDownEffect.SetActive(false);
                break;
            case "root":
                entangleEffect.SetActive(false);
                break;
            case "defend":
                defendEffect.SetActive(false);
                break;
            default:
                break;
        }
    }
}