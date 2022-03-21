using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

public class FireTile : HazardTile
{
    public int stageIndex = 3;

    GameObject stage3, stage2, stage1;

    bool startDepleting = false;

    protected override void Init()
    {
        stage3 = transform.GetChild(0).gameObject;
        stage2 = transform.GetChild(1).gameObject;
        stage1 = transform.GetChild(2).gameObject;

        SwapPrefabs();
    }

    public override void OnRoundEnd()
    {
        if (startDepleting)
        {
            List<Node> _neighbors = grid.GetNeighbors(node);

            int _strongNeighbors = 0;

            foreach (Node _neighbor in _neighbors)
            {
                if(_neighbor.hazard != null)
                {
                    GameObject _hazardObject = _neighbor.hazard.gameObject;
                    FireTile _fireTile;

                    if(_hazardObject.TryGetComponent(out _fireTile))
                    {
                        if(_fireTile.stageIndex >= stageIndex - 1)
                        {
                            _strongNeighbors++;
                        }
                    }
                }
            }

            if(_strongNeighbors < 4)
            {
                stageIndex--;
                SwapPrefabs();
            }
        }
        else
        {
            startDepleting = true;
        }
    }

    void SwapPrefabs()
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
        if (unit.HasEffect(StatusEffectType.Burn))
        {
            int _effectIndex = unit.GetEffectIndex(StatusEffectType.Burn);
            unit.RemoveEffect(unit.currentEffects[_effectIndex]);
        }

        unit.ApplyEffect(new StatusEffect_Burn(unit, unit, GameSettings.statusEffectSettings.burnDamage, 3));
    }
}