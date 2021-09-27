using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class UIManager_Combat handles basic UI logic in the combat scene */
public class UIManager_Combat : MonoBehaviour
{
    TurnManager turnManager;

    public static UIManager_Combat instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        turnManager = TurnManager.instance;
    }

    public void Button_Move()
    {
        FriendlyUnit _unit = (FriendlyUnit)turnManager.activeTurn.unit;
        _unit.MoveAction();
    }

    public void Button_Attack()
    {
        print("Attack");
    }

    public void Button_UseItem()
    {
        print("UseItem");
    }

    public void Button_EndTurn()
    {
        turnManager.NextTurn();
    }
}
