using System;
using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using Harpaesis.UI;
using Harpaesis.GridAndPathfinding;
using UnityEngine;
using Harpaesis;

/**
 * @author Matthew Sommer
 * class Unit manages the generic basic data of each Unit, as well as connects the other
 * unit scripts together */
[RequireComponent(typeof(UnitMotor))]
public abstract class Unit : MonoBehaviour
{
    public UnitData unitData;
    [HideInInspector] public UnitMotor motor;

    public bool hasPath;

    [ReadOnly] public int currentHP;
    float HealthPercent { get { return (float)currentHP/(float)unitData.healthStat; } }
    [ReadOnly] public bool isAlive = true;
    [ReadOnly] public bool canMove = true;
    [ReadOnly] public bool canAct = true;

    [Header("Adjusted Stats")]
    [ReadOnly] public int currentAtkStat;
    [ReadOnly] public int currentDefStat;
    [ReadOnly] public int currentWilStat;
    [ReadOnly] public int currentApStat;

    [ReadOnly] public List<DamageType> currentWeaknesses = new List<DamageType>();
    [ReadOnly] public List<DamageType> currentResistances = new List<DamageType>();


    [ReadOnly] public List<StatusEffect> currentEffects = new List<StatusEffect>();

    [ReadOnly] public Turn turnData;

    protected UnitPassive unitPassive;

    protected GridManager grid;
    protected GridCamera gridCam;
    protected UIManager_Combat uiCombat;
    [HideInInspector] public ColorPaletteManager paletteManager;
    [HideInInspector] public Unit_UI unit_ui;

    bool inited = false;
    public void EarlyInit()
    {
        grid = GridManager.instance;
        gridCam = GridCamera.instance;
        uiCombat = UIManager_Combat.instance;
        paletteManager = GetComponentInChildren<ColorPaletteManager>();
        unit_ui = GetComponentInChildren<Unit_UI>();
        motor = GetComponent<UnitMotor>();
        motor.Init(this);

        currentHP = unitData.healthStat;
        currentAtkStat = unitData.attackStat;
        currentDefStat = unitData.defenseStat;
        currentWilStat = unitData.willpowerStat;
        currentApStat = unitData.apStat;

        currentWeaknesses =  unitData.weaknesses;
        currentResistances = unitData.resistances;

        AssignPassive();
        
        transform.position = grid.NodePositionFromWorldPoint(transform.position);
        grid.NodeFromWorldPoint(transform.position).hasUnit = true;

        Init();
        inited = true;
    }

    private void Update()
    {
        if (!inited)
            return;
        unit_ui.healthBar.Tick(HealthPercent);
        Tick();
    }

    protected virtual void OnAwake() { }
    protected virtual void Init() { }
    protected virtual void Tick()
    {
    }

    public void TakeDamage(int _damageAmount, DamageType _damageType, Unit _attacker = null)
    {
        if (!isAlive || _damageAmount <= 0) return;

        int _adjustedDamage = _damageAmount;

        if (currentWeaknesses.Contains(_damageType))
        {
            _adjustedDamage = Mathf.FloorToInt(_adjustedDamage * 1.5f);
            print($"Weak! Original Damage ({_damageAmount}) was amplified to {_adjustedDamage}");
        }
        if (currentResistances.Contains(_damageType))
        {
            _adjustedDamage = Mathf.FloorToInt(_adjustedDamage * .5f);
            print($"Resisted! Original Damage ({_damageAmount}) was reduced to {_adjustedDamage}");
        }

        unit_ui.DisplayDamageText(-_adjustedDamage);

        OnTakeDamage(_adjustedDamage, _attacker, _damageType);

        currentHP = Mathf.Clamp(currentHP - _adjustedDamage, 0, unitData.healthStat);

        isAlive = (currentHP == 0) ? false : isAlive;

        if (!isAlive)
        {
            Invoke(nameof(HandleUnitDeath), .05f);
        }
    }

    public void Heal(Unit _healer, int _healAmount)
    {
        if (isAlive)
        {
            unit_ui.DisplayDamageText(_healAmount);

            currentHP = Mathf.Clamp(currentHP + _healAmount, 0, unitData.healthStat);
        }
    }

    public void HandleUnitDeath()
    {
        TurnManager.instance.RemoveUnit(this);
        canMove = false;
        canAct = false;
        BattleLog.Log($"{unitData.unitName} has died!", BattleLogType.Combat);
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

    public bool HasEffect(StatusEffectType _effect)
    {
        int _index = GetEffectIndex(_effect);

        return _index != -1;
    }

    public int GetEffectIndex(StatusEffectType _effect)
    {
        if (currentEffects.Count > 0)
        {
            for (int i = 0; i < currentEffects.Count; i++)
            {
                if (currentEffects[i].effectType == _effect)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public void AssignPassive()
    {
        switch (unitData.unitPassive)
        {
            case UnitPassiveType.Alexander:
                unitPassive = new AlexanderPassive(this);
                break;
            case UnitPassiveType.Cori:
                unitPassive = new CoriPassive(this);
                break;
            case UnitPassiveType.Doran:
                unitPassive = new DoranPassive(this);
                break;
            case UnitPassiveType.Joachim:
                unitPassive = new JoachimPassive(this);
                break;
            case UnitPassiveType.Regina:
                unitPassive = new ReginaPassive(this);
                break;
            case UnitPassiveType.Vampire:
                unitPassive = new VampirePassive(this);
                break;
            case UnitPassiveType.Lycan:
                unitPassive = new LycanPassive(this);
                break;
            case UnitPassiveType.ElderGod:
                unitPassive = new ElderGodPassive(this);
                break;
            default:
                unitPassive = new EnemyUnitPassive(this);
                break;
        }
    }

    public void StartUnitTurn()
    {
        OnTurnStart();

        if (HasEffect(StatusEffectType.Sleep))
        {
            Invoke(nameof(ForceEndTurn), 1f);
        }
        else if (HasEffect(StatusEffectType.Fear))
        {

            Invoke(nameof(StartFearTurn), 1f);
        }
        else
        {
            canMove = !HasEffect(StatusEffectType.Root);
            Invoke(nameof(StartTurn), .05f);
        }
    }

    public abstract void StartTurn();
    public virtual void StartFearTurn() { }

    public void ForceEndTurn()
    {
        TurnManager.instance.NextTurn();
    }

    public void DestroyModel()
    {
        if(TurnManager.instance.activeTurn.unit == this)
        {
            TurnManager.instance.NextTurn();
        }
        grid.NodeFromWorldPoint(transform.position).hasUnit = false;
        Destroy(gameObject);
    }
    private void OnTurnStart()
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            currentEffects[i].OnTurnStart();
        }

        unitPassive.OnTurnStart();
    }
    public void OnTurnEnd()
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            currentEffects[i].OnTurnEnd();
        }

        unitPassive.OnTurnEnd();
    }
    public void OnAnyTurnEnd()
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            currentEffects[i].OnAnyTurnEnd();
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
    public void OnRoundEnd()
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            currentEffects[i].OnRoundEnd();
        }
    }
    public void OnDealDamage(int _damageAmount, Unit _damagedUnit, DamageType _damageType)
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            currentEffects[i].OnDealDamage(_damageAmount, _damagedUnit);
        }

        unitPassive.OnDealDamage(_damageAmount, _damagedUnit, _damageType);
    }
    public int OnTargeted(int _damageAmount)
    {
        int _modifiedDamageAmount = _damageAmount;

        for (int i = 0; i < currentEffects.Count; i++)
        {
            _modifiedDamageAmount = currentEffects[i].OnTargeted(_modifiedDamageAmount);
        }

        return _modifiedDamageAmount;
    }
    public void OnTakeDamage(int _damageAmount, Unit _damagingUnit, DamageType _damageType)
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            currentEffects[i].OnTakeDamage(_damageAmount, _damagingUnit);
        }

        unitPassive.OnTakeDamage(_damageAmount, _damagingUnit, _damageType);
    }
    public void CleanseNegativeEffects()
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            if (currentEffects[i].isNegativeEffect)
            {
                currentEffects[i].RemoveEffect();
                CleanseNegativeEffects();
                return;
            }
        }
    }
}
