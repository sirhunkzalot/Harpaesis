using System.Collections;
using System.Collections.Generic;
using Harpaesis.GridAndPathfinding;
using Harpaesis.Combat;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class EnemyUnit handles the logic that pertains to every enemy unit */
public class EnemyUnit : Unit
{
    [HideInInspector] public EnemyUnitData enemyUnitData;

    protected override void Init()
    {
        enemyUnitData = (EnemyUnitData)unitData;
    }

    public override void StartTurn()
    {
        base.StartTurn();

        uiCombat.IsPlayerTurn(false);
        gridCam.followUnit = this;

        if (enemyUnitData.enemySkills.Length == 0)
        {
            TurnManager.instance.NextTurn();
        }
    }
}

