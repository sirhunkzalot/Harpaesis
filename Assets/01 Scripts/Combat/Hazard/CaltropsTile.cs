using System.Collections;
using System.Collections.Generic;
using Harpaesis.GridAndPathfinding;
using Harpaesis.Combat;
using UnityEngine;

public class CaltropsTile : HazardTile
{
    public GameObject stage3, stage2, stage1;

    public int stageIndex = 3;
    bool startDepleting = false;

    private void Awake()
    {
        UpdateCaltrops();
    }


    public void UpdateCaltrops()
    {
        switch (stageIndex)
        {
            case 3:
                stage3.SetActive(true);
                stage2.SetActive(false);
                stage1.SetActive(false);
                break;
            case 2:
                stage3.SetActive(false);
                stage2.SetActive(true);
                stage1.SetActive(false);
                break;
            case 1:
                stage3.SetActive(false);
                stage2.SetActive(false);
                stage1.SetActive(true);
                break;
            default:
                Invoke(nameof(RemoveHazard), .05f);
                break;
        }
    }


    protected override void ApplyHazardEffect()
    {
        if (unit == null)
        {
            return;
        }

        if (unit.HasEffect(StatusEffectType.Bleed))
        {
            int _effectIndex = unit.GetEffectIndex(StatusEffectType.Bleed);
            unit.RemoveEffect(unit.currentEffects[_effectIndex]);
        }

        unit.ApplyEffect(new StatusEffect_Bleed(unit, unit, GameSettings.statusEffectSettings.bleedDamage, 3));
    }

    public override void OnRoundEnd()
    {
        if (startDepleting)
        {
            stageIndex--;

            UpdateCaltrops();
        }
    }
}
