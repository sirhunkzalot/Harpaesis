using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

public class FireTile : HazardTile
{
    public int stageIndex = 3;
    public int count;

    public LayerMask layerMask;

    public GameObject stage3, stage2, stage1;

    bool startDepleting = false;

    private void Awake()
    {
        Collider[] _colls = Physics.OverlapSphere(transform.position, .25f, layerMask, QueryTriggerInteraction.Collide);
        
        if(_colls.Length > 0)
        {
            foreach (Collider _coll in _colls)
            {
                FireTile _tile;

                if (_coll.TryGetComponent(out _tile))
                {
                    if(_tile != this)
                    {
                        _tile.UpdateFire();
                        Invoke(nameof(RemoveHazard), .05f);
                        return;
                    }
                }
            }
        }

        UpdateNearbyFire();
        UpdateFire();
    }

    public void UpdateFire()
    {
        Collider[] _otherHazards = Physics.OverlapSphere(transform.position, 1.5f, layerMask, QueryTriggerInteraction.Collide);

        count = _otherHazards.Length;
        switch (count)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                stageIndex = 1;
                break;
            case 5:
            case 6:
                stageIndex = 2;
                break;
            default:
                stageIndex = 3;
                break;
        }

        SwapPrefabs();
    }

    public void UpdateNearbyFire()
    {
        Collider[] _otherFireTiles = Physics.OverlapSphere(transform.position, 1.5f, layerMask, QueryTriggerInteraction.Collide);


        if (_otherFireTiles.Length > 0)
        {
            foreach (Collider _col in _otherFireTiles)
            {
                _col.GetComponent<FireTile>()?.UpdateFire();
            }
        }
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

                    if(_hazardObject != null && _hazardObject.TryGetComponent(out _fireTile))
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

            ApplyHazardEffect();
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
        if(unit == null)
        {
            return;
        }

        if (unit.HasEffect(StatusEffectType.Burn))
        {
            int _effectIndex = unit.GetEffectIndex(StatusEffectType.Burn);
            unit.RemoveEffect(unit.currentEffects[_effectIndex]);
        }

        unit.ApplyEffect(new StatusEffect_Burn(unit, unit, GameSettings.statusEffectSettings.burnDamage, 3));
    }
}