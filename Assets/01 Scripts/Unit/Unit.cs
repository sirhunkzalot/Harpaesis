using System.Collections;
using System.Collections.Generic;
using GridAndPathfinding;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class Unit manages the generic basic data of each Unit, as well as connects the other
 * unit scripts together */
[RequireComponent(typeof(UnitMotor))]
public abstract class Unit : MonoBehaviour
{
    public UnitData unitData;
    protected UnitMotor motor;

    public bool hasPath;

    public int currentHP;
    public bool isAlive = true;

    public Turn turnData;

    protected GridManager grid;


    private void Start()
    {
        grid = GridManager.instance;
        motor = GetComponent<UnitMotor>();
        motor.Init(this);
        Init();
    }

    private void Update()
    {
        Tick();
    }

    protected virtual void Init() { }
    protected virtual void Tick() { }

    public void TakeDamage(int _damageAmount)
    {
        currentHP = Mathf.Clamp(currentHP -_damageAmount, 0, unitData.healthStat);

        isAlive = (currentHP == 0) ? false : isAlive;
    }
}
