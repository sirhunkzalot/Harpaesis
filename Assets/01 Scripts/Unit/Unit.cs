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
        currentHP = unitData.healthStat;
        Init();
    }

    private void Update()
    {
        Tick();
    }

    protected virtual void Init() { }
    protected virtual void Tick() { }

    public void TakeDamage(int _damageAmount, Unit _attacker = null)
    {
        if (!isAlive || _damageAmount <= 0) return;

        currentHP = Mathf.Clamp(currentHP - _damageAmount, 0, unitData.healthStat);

        if(_attacker != null)
        {
            Debug.Log($"{_attacker.unitData.unitName} deals {_damageAmount} damage to {unitData.unitName}!");
        }
        else
        {
            Debug.Log($"{unitData.unitName} took {_damageAmount} damage!");
        }

        isAlive = (currentHP == 0) ? false : isAlive;
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
        if(currentEffects.Count > 0)
        {
            for (int i = 0; i < currentEffects.Count; i++)
            {
                if (currentEffects[i].GetType() == _effect.GetType())
                {
                    currentEffects[i] += _effect;
                    return;
                }
            }
        }

        currentEffects.Add(_effect);
    }

    public void RemoveEffect(StatusEffect _effect)
    {
        if (currentEffects.Contains(_effect))
        {
            currentEffects.Remove(_effect);
        }
    }
}
