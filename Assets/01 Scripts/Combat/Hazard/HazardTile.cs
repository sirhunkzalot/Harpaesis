using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

public class HazardTile : MonoBehaviour
{
    protected Unit unit;
    protected GridManager grid;
    protected TurnManager turnManager;
    protected Node node;

    private void Start()
    {
        grid = GridManager.instance;
        turnManager = TurnManager.instance;

        turnManager.activeHazards.Add(this);

        node = grid.NodeFromWorldPoint(transform.position);
        node.ApplyHazard(this);


        Init();
    }

    protected virtual void Init() { }

    public virtual void OnRoundEnd() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out unit))
        {
            ApplyHazardEffect();
        }
    }

    protected virtual void ApplyHazardEffect() { }
    protected virtual void RemoveHazard()
    {
        turnManager.activeHazards.Remove(this);
        node.RemoveHazard();
        Destroy(gameObject);
    }
}
