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
    public UnitMotor motor;

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
            BattleLog.Log($"{_attacker.unitData.unitName} deals {_damageAmount} damage to {unitData.unitName}!", BattleLogType.CombatLog);
        }
        else
        {
            BattleLog.Log($"{unitData.unitName} took {_damageAmount} damage!", BattleLogType.CombatLog);
        }

        isAlive = (currentHP == 0) ? false : isAlive;

        if (!isAlive)
        {
            HandleUnitDeath();
        }
    }

    public void Heal(Unit _healer, int _healAmount)
    {
        if (isAlive)
        {
            currentHP = Mathf.Clamp(currentHP + _healAmount, 0, unitData.healthStat);
        }
    }

    public void HandleUnitDeath()
    {
        TurnManager.instance.RemoveUnit(this);
        canMove = false;
        canAct = false;
        BattleLog.Log($"{unitData.unitName} has died!", BattleLogType.CombatLog);
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

    public void ForceEndTurn()
    {
        TurnManager.instance.NextTurn();
    }

    public void OnTurnStart()
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            currentEffects[i].OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            currentEffects[i].OnTurnEnd();
        }
    }

    public void OnTakeStep()
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            currentEffects[i].OnTakeStep();
        }
    }

    public void OnRoundStart() { }
    public void OnRoundEnd() { }
    public void OnDealDamage() { }
    public void OnTakeDamage() { }
}
