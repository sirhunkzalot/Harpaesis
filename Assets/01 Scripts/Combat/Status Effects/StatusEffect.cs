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
        public virtual void OnTurnStart() { }
        public virtual void OnTurnEnd() { }
        public virtual void OnDealDamage() { }
        public virtual void OnTakeDamage() { }
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

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("dot");
        }
    }
    public class StatusEffect_Bleed : StatusEffect_DamageOverTime
    {
        public StatusEffect_Bleed(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} is bleeding!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("bleed");
        }

        public override void OnTakeStep()
        {
            ApplyDamage();
        }

        protected override void ApplyDamage()
        {
            effectedUnit.TakeDamage(StatusEffectSettings.bleedDamage);
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("bleed");
        }
    }
    public class StatusEffect_Burn : StatusEffect_DamageOverTime
    {
        public StatusEffect_Burn(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration)
        {
        }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has been burned!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("burn");
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
            effectedUnit.TakeDamage(StatusEffectSettings.burnDamage);
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("burn");
        }
    }
    public class StatusEffect_Fear : StatusEffect
    {
        public StatusEffect_Fear(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has been feared!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("fear");
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
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("fear");
        }
    }
    public class StatusEffect_Sleep : StatusEffect
    {
        public StatusEffect_Sleep(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has been put to sleep!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("sleep");
        }

        public override void OnTakeDamage()
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

            // Reimplement next sprint when enemies actually have turns:
            //effectedUnit.ForceEndTurn();
            BattleLog.Log($"{effectedUnit.unitData.unitName} is asleep.", BattleLogType.Combat);
        }

        void WakeUp()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has woken up!", BattleLogType.Combat);
            effectedUnit.canAct = true;
            effectedUnit.canMove = true;
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("sleep");
        }
    }
    public class StatusEffect_Root : StatusEffect
    {
        public StatusEffect_Root(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            effectedUnit.canMove = false;
            BattleLog.Log($"{effectedUnit.unitData.unitName} has been rooted!", BattleLogType.Combat);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("root");
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
            effectedUnit.canMove = true;
            BattleLog.Log($"{effectedUnit.unitData.unitName} has escaped the etanglement!", BattleLogType.Combat);
            RemoveEffect();
        }

        protected override void OnEffectRemoved()
        {
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("root");
        }
    }
    public class StatusEffect_BuffATK : StatusEffect
    {
        public StatusEffect_BuffATK(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} ATK stat has been temporarily raised by {amount}.", BattleLogType.Combat);
            BattleLog.Log($"BuffATK is not yet implemented", BattleLogType.System);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("atkUp");
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
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("atkUp");
        }
    }
    public class StatusEffect_BuffDEF : StatusEffect
    {
        public StatusEffect_BuffDEF(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} DEF stat has been temporarily raised by {amount}.", BattleLogType.Combat);
            BattleLog.Log($"BuffDEF is not yet implemented", BattleLogType.System);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("defUp");
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
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("defUp");
        }
    }
    public class StatusEffect_BuffAP : StatusEffect
    {
        public StatusEffect_BuffAP(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} AP stat has been temporarily raised by {amount}.", BattleLogType.Combat);
            BattleLog.Log($"BuffAP is not yet implemented", BattleLogType.System);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("apUp");
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
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("apUp");
        }
    }
    public class StatusEffect_DebuffATK : StatusEffect
    {
        public StatusEffect_DebuffATK(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} ATK stat has been temporarily lowered by {amount}.", BattleLogType.Combat);
            BattleLog.Log($"DebuffATK is not yet implemented", BattleLogType.System);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("atkDown");
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
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("atkDown");
        }
    }
    public class StatusEffect_DebuffDEF : StatusEffect
    {
        public StatusEffect_DebuffDEF(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} DEF stat has been temporarily lowered by {amount}.", BattleLogType.Combat);
            BattleLog.Log($"DebuffDEF is not yet implemented", BattleLogType.System);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("defDown");
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
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("defDown");
        }
    }
    public class StatusEffect_DebuffAP : StatusEffect
    {
        public StatusEffect_DebuffAP(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} AP stat has been temporarily lowered by {amount}.", BattleLogType.Combat);
            BattleLog.Log($"DebuffAP is not yet implemented", BattleLogType.System);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("apDown");
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
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("apDown");
        }
    }
    public class StatusEffect_ChangeAllegiance : StatusEffect
    {
        public StatusEffect_ChangeAllegiance(Unit _inflictingUnit, Unit _effectedUnit, int _amount, int _duration) : base(_inflictingUnit, _effectedUnit, _amount, _duration) { }

        protected override void OnEffectApplied()
        {
            BattleLog.Log($"{effectedUnit.unitData.unitName} has flipped sides!", BattleLogType.Combat);
            BattleLog.Log($"Change Allegiance is not yet implemented", BattleLogType.System);
            effectedUnit.unit_ui.effectsManager.ActivateEffect("changeAllegiance");
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
            effectedUnit.unit_ui.effectsManager.DeactivateEffect("changeAllegiance");
        }
    }
}