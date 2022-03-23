using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine;

public abstract class UnitPassive
{
    public virtual void OnCombatStart() { }
    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }
    public virtual void OnDealDamage(int _damageAmount, Unit _damagedUnit, DamageType _damageType) { }
    public virtual void OnTakeDamage(float _damageAmount, Unit _damagingUnit, DamageType _damageType) { }
}

#region Friendly Passives
public class FriendlyUnitPassive : UnitPassive
{
    protected FriendlyUnit unit;

    public FriendlyUnitPassive(Unit _unit)
    {
        unit = (FriendlyUnit)_unit;
    }

    public virtual void OnChangeWeapon() { }
}

public class AlexanderPassive : FriendlyUnitPassive
{
    public AlexanderPassive(Unit _unit) : base(_unit) { }

}
public class CoriPassive : FriendlyUnitPassive
{
    public CoriPassive(Unit _unit) : base(_unit) { }

}
public class DoranPassive : FriendlyUnitPassive
{
    public DoranPassive(Unit _unit) : base(_unit) { }

}
public class JoachimPassive : FriendlyUnitPassive
{
    public JoachimPassive(Unit _unit) : base(_unit) { }

    public override void OnCombatStart()
    {
        if (unit.alternativeWeapon)
        {
            // Adds Attack Bonus if Greatsword is Equipped
            unit.currentAtkStat += GameSettings.unitPassiveSettings.joachimBonusAtkAmount;
        }
        else
        {
            // Adds Defense Bonus if Sword and Shield is Equipped
            unit.currentDefStat += GameSettings.unitPassiveSettings.joachimBonusDefAmount;
        }
    }

    public override void OnChangeWeapon()
    {
        if (unit.alternativeWeapon)
        {
            // Removes bonus to Defense
            unit.currentDefStat -= GameSettings.unitPassiveSettings.joachimBonusDefAmount;

            // Adds bonus to Attack
            unit.currentAtkStat += GameSettings.unitPassiveSettings.joachimBonusAtkAmount;
        }
        else
        {
            // Removes bonus to Attack
            unit.currentAtkStat -= GameSettings.unitPassiveSettings.joachimBonusAtkAmount;

            // Adds bonus to Defense
            unit.currentDefStat += GameSettings.unitPassiveSettings.joachimBonusDefAmount;
        }
    }
}
public class ReginaPassive : FriendlyUnitPassive
{
    public ReginaPassive(Unit _unit) : base(_unit) { }

}

#endregion

#region Enemy Passives

public class EnemyUnitPassive : UnitPassive
{
    protected EnemyUnit unit;

    public EnemyUnitPassive(Unit _unit)
    {
        unit = (EnemyUnit)_unit;
    }
}

public class VampirePassive : EnemyUnitPassive
{


    public VampirePassive(Unit _unit) : base(_unit) { }

    public override void OnDealDamage(int _damageAmount, Unit _damagedUnit, DamageType _damageType)
    {
        // If unit does not have Holy Water Effect applied
        if (!unit.HasEffect(StatusEffectType.Holy))
        {
            // Heals for bonus health percent if target is bleeding
            float _healAmount = _damageAmount;
            _healAmount *= _damagedUnit.HasEffect(StatusEffectType.Bleed) ?
                GameSettings.unitPassiveSettings.vampireBoostedHealPercent : GameSettings.unitPassiveSettings.vampireBaseHealPercent;

            unit.Heal(unit, Mathf.CeilToInt(_healAmount));
        }
    }
}

public class LycanPassive : EnemyUnitPassive
{
    public LycanPassive(Unit _unit) : base(_unit) { }

    public override void OnTurnEnd()
    {
        // If unit does not have Silver effect applied
        if (!unit.HasEffect(StatusEffectType.Silver))
        {
            // Heals for bonus health percent if target is bleeding
            int _missingHP = unit.unitData.healthStat - unit.currentHP;
            float _healAmount = _missingHP * GameSettings.unitPassiveSettings.lycanHealPercentOfMissingHealth;

            if(_healAmount > 0)
            {
                unit.Heal(unit, Mathf.CeilToInt(_healAmount));
            }
        }
    }
}

public class ElderGodPassive : EnemyUnitPassive
{
    public ElderGodPassive(Unit _unit) : base(_unit) { }

    public override void OnCombatStart()
    {
        unit.currentResistances = new List<DamageType>();
        unit.currentWeaknesses = new List<DamageType>();
    }

    public override void OnTakeDamage(float _damageAmount, Unit _damagingUnit, DamageType _damageType)
    {

        if (unit.currentResistances.Count >= 1)
        {
            unit.currentResistances.RemoveAt(0);
        }
        unit.currentResistances.Add(_damageType);

    
        if (unit.currentWeaknesses.Count >= 1)
        {
            unit.currentWeaknesses.RemoveAt(0);
        }

        unit.StartCoroutine(AddNewRandomWeakness(_damageType));

        base.OnTakeDamage(_damageAmount, _damagingUnit, _damageType);
    }

    IEnumerator AddNewRandomWeakness(DamageType _damageType)
    {
        int _index = Random.Range(0, 8);
        DamageType _typeToReturn = DamageType.Piercing;
        bool _typeIsNotValid;

        do
        {
            switch (_index)
            {
                case 0:
                    _typeToReturn = DamageType.Piercing;
                    break;
                case 1:
                    _typeToReturn = DamageType.Bludgeoning;
                    break;
                case 2:
                    _typeToReturn = DamageType.Slashing;
                    break;
                case 3:
                    _typeToReturn = DamageType.Silver;
                    break;
                case 4:
                    _typeToReturn = DamageType.Holy;
                    break;
                case 5:
                    _typeToReturn = DamageType.Magic;
                    break;
                case 6:
                    _typeToReturn = DamageType.Fire;
                    break;
                case 7:
                    _typeToReturn = DamageType.Bleed;
                    break;
            }
            yield return new WaitForEndOfFrame();

            _typeIsNotValid = (_typeToReturn == _damageType) || (unit.currentResistances.Contains(_typeToReturn));
            _index = Random.Range(0, 8);

        } while (_typeIsNotValid);

        unit.currentWeaknesses.Add(_typeToReturn);

    }
}
#endregion

public enum UnitPassiveType
{
    // No Passive
    None = 0,

    // Character Passives 1-5
    Alexander = 1,
    Cori = 2,
    Doran = 3,
    Joachim = 4,
    Regina = 5,

    // Enemy Passives 10+
    Vampire = 10,
    Lycan = 11,

    ElderGod = 99
}