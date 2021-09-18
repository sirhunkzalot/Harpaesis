using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    int turnCounter;
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
        if(++turnCounter >= turnOrder.Count)
        {
            turnCounter = 0;
            BuildTurnOrder();
        }

        activeTurn = turnOrder[turnCounter];

        HandleTurn();
    }

    public void HandleTurn()
    {
        bool _isFriendlyUnit = activeTurn.unit.GetType() == typeof(FriendlyUnit);

        if (_isFriendlyUnit)
        {

        }
        else
        {
            Invoke(nameof(NextTurn), 2.5f);
        }
    }
}

[System.Serializable]
public class Turn
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
