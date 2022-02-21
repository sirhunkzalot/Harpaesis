using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Harpaesis.Combat
{
    [System.Serializable]
    public class StatusEffect
    {
        [ReadOnly] public Unit inflictingUnit;
        [ReadOnly] public Unit effectedUnit;
        [ReadOnly] public int amount;
        [ReadOnly] public int duration;

        public StatusEffectType effectType;

        public StatusEffect(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration)
        {
            inflictingUnit = _inflictingUnit;
            effectedUnit = _effectedUnit;
            amount = _amount;
            duration = _duration;

            OnEffectApplied();
        }

        protected void RemoveEffect()
        {
            OnEffectRemoved();
            effectedUnit.RemoveEffect(this);
        }

        protected virtual void OnEffectApplied() { }

        protected virtual void OnEffectRemoved() { }
        public virtual void OnRoundStart() { }
        public virtual void OnRoundEnd() { }
        public virtual int OnTargeted(int _damageAmount) { return _damageAmount;  }
        public virtual void OnTurnStart() { }
        public virtual void OnTurnEnd() { }
        public virtual void OnDealDamage(int _damageAmount, Unit _damagedUnit) { }
        public virtual void OnTakeDamage(int _damageAmount, Unit _damagingUnit) { }
        public virtual void OnTakeStep() { }

        public static StatusEffect operator +(StatusEffect a, StatusEffect b)
        {
            if (a.GetType() == b.GetType())
                return new StatusEffect(a.inflictingUnit, a.effectedUnit, a.amount + b.amount, a.duration + b.duration);

            throw new System.Exception("Error: Status Effects are not of the same type." +
                "Operations between status effects of different types is not valid.");
        }
    }
    public class StatusEffect_DamageOverTime : StatusEffect
    {
        public StatusEffect_DamageOverTime(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        public override void OnTurnStart()
        {
            ApplyDamage();
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                RemoveEffect();
            }
        }

        protected virtual void ApplyDamage()
        {
            effectedUnit.TakeDamage(amount);
        }
    }
    public class StatusEffect_Bleed : StatusEffect_DamageOverTime
    {
        public StatusEffect_Bleed(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.Bleed;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} is bleeding!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.Bleed);
        }

        public override void OnTakeStep()
        {
            ApplyDamage();
        }

        protected override void ApplyDamage()
        {
            effectedUnit.TakeDamage(GameSettings.statusEffectSettings.bleedDamage);
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.Bleed);
        }
    }
    public class StatusEffect_Burn : StatusEffect_DamageOverTime
    {
        public StatusEffect_Burn(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.Burn;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has been burned!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.Burn);
        }

        public override void OnTurnStart()
        {
            ApplyDamage();
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                RemoveEffect();
            }
        }

        protected override void ApplyDamage()
        {
            effectedUnit.TakeDamage(GameSettings.statusEffectSettings.burnDamage);
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.Burn);
        }
    }
    public class StatusEffect_Fear : StatusEffect
    {
        public StatusEffect_Fear(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.Fear;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has been feared!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.Fear);
        }

        public override void OnTurnStart()
        {
            //effectedUnit.motor.RunAway(inflictingUnit.transform.position);
            //effectedUnit.ForceEndTurn();
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                RemoveEffect();
            }
        }
        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.Fear);
        }
    }
    public class StatusEffect_Sleep : StatusEffect
    {
        public StatusEffect_Sleep(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.Sleep;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has been put to sleep!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.Sleep);
        }

        public override void OnTakeDamage(int _damageAmount, Unit _damagingUnit)
        {
            WakeUp();
        }

        public override void OnTurnStart()
        {
            if (--duration <= 0)
            {
                WakeUp();
                return;
            }

            BattleLog.Log($"{effectedUnit.unitData.unitName} is asleep.", BattleLogType.Combat);
        }

        void WakeUp()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has woken up!", BattleLogType.Combat);
            effectedUnit.canAct = true;
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.Sleep);
        }
    }
    public class StatusEffect_Root : StatusEffect
    {
        public StatusEffect_Root(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.Root;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has been rooted!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.Root);
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                EndEntanglement();
            }
        }

        void EndEntanglement()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has escaped the etanglement!", BattleLogType.Combat);
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.Root);
        }
    }
    public class StatusEffect_BuffATK : StatusEffect
    {
        public StatusEffect_BuffATK(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.ATK_Up;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} ATK stat has been temporarily raised by {amount}.", BattleLogType.Combat);
            effectedUnit.currentAtkStat += amount;
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.ATK_Up);
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                EndEffect();
            }
        }

        void EndEffect()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} ATK stat has returned to normal.", BattleLogType.Combat);
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.currentAtkStat -= amount;
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.ATK_Up);
        }
    }
    public class StatusEffect_BuffDEF : StatusEffect
    {
        public StatusEffect_BuffDEF(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.DEF_Up;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} DEF stat has been temporarily raised by {amount}.", BattleLogType.Combat);
            effectedUnit.currentDefStat += amount;
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.DEF_Up);
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                EndEffect();
            }
        }

        void EndEffect()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} DEF stat has returned to normal.", BattleLogType.Combat);
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.currentDefStat -= amount;
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.DEF_Up);
        }
    }
    public class StatusEffect_BuffAP : StatusEffect
    {
        public StatusEffect_BuffAP(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.AP_Up;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} AP stat has been temporarily raised by {amount}.", BattleLogType.Combat);
            effectedUnit.currentApStat += amount;
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.AP_Up);
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                EndEffect();
            }
        }

        void EndEffect()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} AP stat has returned to normal.", BattleLogType.Combat);
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.currentApStat -= amount;
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.AP_Up);
        }
    }
    public class StatusEffect_DebuffATK : StatusEffect
    {
        public StatusEffect_DebuffATK(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.ATK_Down;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} ATK stat has been temporarily lowered by {amount}.", BattleLogType.Combat);
            effectedUnit.currentAtkStat -= amount;
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.ATK_Down);
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                EndEffect();
            }
        }

        void EndEffect()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} ATK stat has returned to normal.", BattleLogType.Combat);
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.currentAtkStat += amount;
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.ATK_Down);
        }
    }
    public class StatusEffect_DebuffDEF : StatusEffect
    {
        public StatusEffect_DebuffDEF(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.DEF_Down;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} DEF stat has been temporarily lowered by {amount}.", BattleLogType.Combat);
            effectedUnit.currentDefStat -= amount;
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.DEF_Down);
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                EndEffect();
            }
        }

        void EndEffect()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} DEF stat has returned to normal.", BattleLogType.Combat);
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.currentDefStat += amount;
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.DEF_Down);
        }
    }
    public class StatusEffect_DebuffAP : StatusEffect
    {
        public StatusEffect_DebuffAP(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.DEF_Down;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} AP stat has been temporarily lowered by {amount}.", BattleLogType.Combat);
            effectedUnit.currentApStat -= amount;
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.AP_Down);
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                EndEffect();
            }
        }

        void EndEffect()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} AP stat has returned to normal.", BattleLogType.Combat);
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.currentApStat += amount;
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.AP_Down);
        }
    }
    public class StatusEffect_ChangeAllegiance : StatusEffect
    {
        public StatusEffect_ChangeAllegiance(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) 
        {
            effectType = StatusEffectType.ChangeAllegience;
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has flipped sides!", BattleLogType.Combat);
            BattleLog.Log($"Change Allegiance is not yet implemented", BattleLogType.System);
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.ChangeAllegience);
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                EndEffect();
            }
        }

        void EndEffect()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has returned to fight for it's side!", BattleLogType.Combat);
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.ChangeAllegience);
        }
    }
    public class StatusEffect_Defend : StatusEffect
    {
        public StatusEffect_Defend(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
            effectType = StatusEffectType.Defend;
        }

        protected override void OnEffectApplied()
        {
            effectedUnit.unit_ui.effectsManager.ActivateEffect(StatusEffectType.Defend);
        }

        public override int OnTargeted(int _damageAmount)
        {
            float _halfDamage = Mathf.Clamp(_damageAmount / 2, 1, float.MaxValue);

            return Mathf.CeilToInt(_halfDamage);
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                EndEffect();
            }
        }

        void EndEffect()
        {
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect(StatusEffectType.Defend);
        }
    }

    public enum StatusEffectType
    {
        Bleed,
        Burn,
        Fear,
        Sleep,
        Root,
        AP_Up,
        AP_Down,
        ATK_Up,
        ATK_Down,
        DEF_Up,
        DEF_Down,
        ChangeAllegience,
        Holy,
        Silver,
        Defend
    }
}