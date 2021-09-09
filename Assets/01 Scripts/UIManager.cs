using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Button_Move()
    {
        print("Move");
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
        print("End Turn");
    }
}
