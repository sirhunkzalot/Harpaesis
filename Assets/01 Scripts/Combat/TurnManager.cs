using System.Collections;
using Harpaesis.Combat;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class TurnManager handles all turn logic and enables other components to take action based
 * on whether it is a player's or enemy's turn */
public class TurnManager : MonoBehaviour
{
    int turnCounter = -1;
    public Turn activeTurn;

    public List<Unit> units = new List<Unit>();
    public List<FriendlyUnit> friendlyUnits = new List<FriendlyUnit>();
    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();

    public List<Turn> turnOrder;

    public static TurnManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Unit[] _units = FindObjectsOfType<Unit>();

        foreach (Unit _unit in _units)
        {
            units.Add(_unit);
            if(_unit.GetType() == typeof(FriendlyUnit))
            {
                friendlyUnits.Add((FriendlyUnit)_unit);
            }
            else
            {
                enemyUnits.Add((EnemyUnit)_unit);
            }
        }

        BuildTurnOrder();
        NextTurn();
    }

    void BuildTurnOrder()
    { 
        turnOrder = new List<Turn>();

        for (int i = 0; i < units.Count; i++)
        {
            GenerateTurns(units[i]);
        }
    }

    void GenerateTurns(Unit _unit)
    {
        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnOrder[i].unitData.inititiveStat < _unit.unitData.inititiveStat)
            {
                turnOrder.Insert(i, new Turn(_unit));
                return;
            }
        }

        turnOrder.Add(new Turn(_unit));
    }

    public void RemoveUnit(Unit _unit)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if(units[i] == _unit)
            {
                units.RemoveAt(i);

                if (_unit.GetType() == typeof(FriendlyUnit))
                {
                    friendlyUnits.Remove((FriendlyUnit)_unit);
                    if (friendlyUnits.Count == 0)
                    {
                        UIManager_EndCombatScreen.instance.OpenLoseScreen();
                    }
                }
                else
                {
                    enemyUnits.Remove((EnemyUnit)_unit);
                    if(enemyUnits.Count == 0)
                    {
                        UIManager_EndCombatScreen.instance.OpenVictoryScreen();
                    }

                    _unit.DestroyModel();
                }
            }
        }
        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnOrder[i].unit == _unit)
            {
                turnOrder.RemoveAt(i);
                return;
            }
        }
    }

    public void NextTurn()
    {
        if(activeTurn.unit != null)
        {
            activeTurn.unit.OnTurnEnd();
        }

        GridCamera.instance.followUnit = null;

        if (++turnCounter >= turnOrder.Count)
        {
            turnCounter = 0;
            BuildTurnOrder();
        }

        UIManager_TurnOrder.instance.UpdateImages(turnCounter, turnOrder);

        activeTurn = turnOrder[turnCounter];
        GridCamera.instance.JumpToPosition(activeTurn.unit.transform.position);

        activeTurn.unit.StartUnitTurn();
    }
}

/**
 * @author Matthew Sommer
 * struct Turn stores data about each individual turn */
[System.Serializable]
public struct Turn
{
    public Unit unit;
    public UnitData unitData;
    public bool hasAttacked;
    public int ap;

    public Turn(Unit _unit)
    {
        unit = _unit;
        unitData = unit.unitData;
        hasAttacked = false;
        ap = unit.currentApStat;

        unit.turnData = this;
    }
}
