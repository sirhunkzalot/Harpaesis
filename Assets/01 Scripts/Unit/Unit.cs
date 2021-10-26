using System;
using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using Harpaesis.GridAndPathfinding;
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
    public bool canMove = true;
    public bool canAct = true;

    public List<StatusEffect> currentEffects = new List<StatusEffect>();

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
        if (!isAlive || _damageAmount <= 0) return;

        currentHP = Mathf.Clamp(currentHP - _damageAmount, 0, unitData.healthStat);

        isAlive = (currentHP == 0) ? false : isAlive;
    }

    public void TakeDamage(Unit _attacker, int _damageAmount)
    {
        TakeDamage(_damageAmount);
    }

    public void Heal(Unit _healer, int _healAmount)
    {
        if (isAlive)
        {
            currentHP = Mathf.Clamp(currentHP + _healAmount, 0, unitData.healthStat);
        }
    }

    public void ApplyEffect(StatusEffect _effect)
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            if(currentEffects.GetType() == currentEffects.GetType())
            {
                currentEffects[i] += _effect;
            }
        }
    }

    public void RemoveEffect(StatusEffect _effect)
    {
        if (currentEffects.Contains(_effect))
        {
            currentEffects.Remove(_effect);
        }
    }
}
