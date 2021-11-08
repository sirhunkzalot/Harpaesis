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

        activeTurn = turnOrder[turnCounter];
        GridCamera.instance.JumpToPosition(activeTurn.unit.transform.position);

        activeTurn.unit.StartTurn();
    }

    public void HandleTurn()
    {
        bool _isFriendlyUnit = activeTurn.unit.GetType() == typeof(FriendlyUnit);
        UIManager_Combat.instance.IsPlayerTurn(_isFriendlyUnit);

        activeTurn.unit.StartTurn();
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
    public int ap;

    public Turn(Unit _unit)
    {
        unit = _unit;
        unitData = unit.unitData;
        ap = unit.unitData.apStat;

        unit.turnData = this;
    }
}
