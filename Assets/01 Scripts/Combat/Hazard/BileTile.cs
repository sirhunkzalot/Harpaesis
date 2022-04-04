using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine;

public class BileTile : HazardTile
{
    public GameObject stage3, stage2, stage1;

    public int stageIndex = 3;
    bool startDepleting = false;

    private void Awake()
    {
        UpdateBile();
    }


    public void UpdateBile()
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

        if (unit.HasEffect(StatusEffectType.AP_Down))
        {
            int _effectIndex = unit.GetEffectIndex(StatusEffectType.AP_Down);
            unit.RemoveEffect(unit.currentEffects[_effectIndex]);
        }
        if (unit.HasEffect(StatusEffectType.DEF_Down))
        {
            int _effectIndex = unit.GetEffectIndex(StatusEffectType.DEF_Down);
            unit.RemoveEffect(unit.currentEffects[_effectIndex]);
        }
        if (unit.HasEffect(StatusEffectType.WIL_Down))
        {
            int _effectIndex = unit.GetEffectIndex(StatusEffectType.WIL_Down);
            unit.RemoveEffect(unit.currentEffects[_effectIndex]);
        }

        unit.ApplyEffect(new StatusEffect_DebuffAP(unit, unit, Mathf.CeilToInt(unit.currentApStat / 2), 3));
        unit.ApplyEffect(new StatusEffect_DebuffDEF(unit, unit, Mathf.CeilToInt(unit.currentDefStat / 2), 3));
        unit.ApplyEffect(new StatusEffect_DebuffWIL(unit, unit, Mathf.CeilToInt(unit.currentWilStat / 2), 3));
    }

    public override void OnRoundEnd()
    {
        if (startDepleting)
        {
            stageIndex--;

            UpdateBile();
        }
    }
}