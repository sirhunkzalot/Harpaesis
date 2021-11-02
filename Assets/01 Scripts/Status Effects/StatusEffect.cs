using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Harpaesis.Combat
{
    [System.Serializable]
    public class StatusEffect
    {
        protected Unit effectedUnit;
        protected int amount;
        protected int duration;

        public StatusEffect(Unit _effectedUnit, int _amount, int _duration)
        {
            effectedUnit = _effectedUnit;
            amount = _amount;
            duration = _duration;

            OnEffectApplied();
        }

        protected void RemoveEffect()
        {
            effectedUnit.RemoveEffect(this);
        }

        protected virtual void OnEffectApplied() { }
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
                return new StatusEffect(a.effectedUnit, a.amount + b.amount, a.duration + b.duration);

            throw new System.Exception("Error: Status Effects are not of the same type." +
                "Operations between status effects of different types is not valid.");
        }
    }

    public class StatusEffect_DamageOverTime : StatusEffect
    {
        public StatusEffect_DamageOverTime(Unit _effectedUnit, int _amount, int _duration) : base(_effectedUnit, _amount, _duration) { }

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
        public StatusEffect_Bleed(Unit _effectedUnit, int _amount, int _duration) : base(_effectedUnit, _amount, _duration) { }

        public override void OnTakeStep()
        {
            ApplyDamage();
        }

        protected override void ApplyDamage()
        {
            effectedUnit.TakeDamage(StatusEffectSettings.bleedDamage);
        }
    }

    public class StatusEffect_Burn : StatusEffect_DamageOverTime
    {
        public StatusEffect_Burn(Unit _effectedUnit, int _amount, int _duration) : base(_effectedUnit, _amount, _duration)
        {
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
    }

    public class StatusEffect_Fear : StatusEffect
    {
        public StatusEffect_Fear(Unit _effectedUnit, int _amount, int _duration) : base(_effectedUnit, _amount, _duration) { }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                RemoveEffect();
            }
        }
    }
    public class StatusEffect_Sleep : StatusEffect
    {
        public StatusEffect_Sleep(Unit _effectedUnit, int _amount, int _duration) : base(_effectedUnit, _amount, _duration) { }

        public override void OnTakeDamage()
        {
            WakeUp();
        }

        public override void OnTurnEnd()
        {
            if (--duration <= 0)
            {
                WakeUp();
            }
        }

        void WakeUp()
        {
            effectedUnit.canAct = true;
            RemoveEffect();
        }
    }
    public class StatusEffect_Root : StatusEffect
    {
        public StatusEffect_Root(Unit _effectedUnit, int _amount, int _duration) : base(_effectedUnit, _amount, _duration) { }

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
            RemoveEffect();
        }
    }
}