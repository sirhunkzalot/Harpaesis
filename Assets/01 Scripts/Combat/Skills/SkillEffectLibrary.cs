using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    public static class SkillEffectLibrary
    {
       public static void ResolveEffects(Unit _user, Unit _target, List<SkillEffect> skillEffects)
        {
            for (int i = 0; i < skillEffects.Count; i++)
            {
                ResolveEffect(_user, _target, skillEffects[i]);
            }
        }

        public static void ResolveEffect(Unit _user, Unit _target, SkillEffect _effect)
        {
            int _params1 = 0;
            int _params2 = 0;

            if(_effect.param1 != "")
            {
                _params1 = CombatUtility.TranslateFormula(_effect.param1);
            }
            if(_effect.param2 != "")
            {
                _params2 = CombatUtility.TranslateFormula(_effect.param2);
            }

            bool _option = _effect.option;

            switch (_effect.effectType)
            {
                case SkillEffectType.Heal:
                    SkillEffect_Heal(_user, _target, _params1);
                    break;
                case SkillEffectType.HealOverTime:
                    SkillEffect_HealOverTime(_user, _target, _params1, _params2);
                    break;
                case SkillEffectType.Damage:
                    SkillEffect_Damage(_user, _target,  _params1, _option);
                    break;
                case SkillEffectType.DamageOverTime:
                    SkillEffect_DamageOverTime(_user, _target,  _params1, _params2);
                    break;
                case SkillEffectType.ApplyBleed:
                    SkillEffect_ApplyBleed(_user, _target,  _params1, _params2);
                    break;
                case SkillEffectType.ApplyBurn:
                    SkillEffect_ApplyBurn(_user, _target,  _params1, _params2);
                    break;
                case SkillEffectType.ApplyFear:
                    SkillEffect_ApplyFear(_user, _target, _params1);
                    break;
                case SkillEffectType.ApplySleep:
                    SkillEffect_ApplySleep(_user, _target, _params1);
                    break;
                case SkillEffectType.ApplyCharm:
                    SkillEffect_ApplyCharm(_user, _target, _params1);
                    break;
                case SkillEffectType.ApplyRoot:
                    SkillEffect_ApplyRoot(_user, _target, _params1);
                    break;
                case SkillEffectType.ApplyKnockback:
                    SkillEffect_ApplyKnockback(_user, _target,  _params1);
                    break;
                case SkillEffectType.DamageWithLifesteal:
                    SkillEffect_DamageWithLifesteal(_user, _target, _params1, _params2, _option);
                    break;
                case SkillEffectType.BuffATK:
                    SkillEffect_BuffATK(_user, _target,  _params1, _params2);
                    break;
                case SkillEffectType.BuffDEF:
                    SkillEffect_BuffDEF(_user, _target,  _params1, _params2);
                    break;
                case SkillEffectType.BuffAP:
                    SkillEffect_BuffAP(_user, _target,  _params1, _params2);
                    break;
                case SkillEffectType.DebuffATK:
                    SkillEffect_DebuffATK(_user, _target,  _params1, _params2);
                    break;
                case SkillEffectType.DebuffDEF:
                    SkillEffect_DebuffDEF(_user, _target,  _params1, _params2);
                    break;
                case SkillEffectType.DebuffAP:
                    SkillEffect_DebuffAP(_user, _target,  _params1, _params2);
                    break;
                case SkillEffectType.Silvered:
                    SkillEffect_Silver(_effect);
                    break;
                case SkillEffectType.Holy:
                    SkillEffect_Holy(_effect);
                    break;
                case SkillEffectType.OverrideTargetToSelf:
                    SkillEffect_OverrideTargetToSelf(_effect);
                    break;
                case SkillEffectType.ChangeAllegiance:
                    SkillEffect_ChangeAllegiance(_user, _target, _params1);
                    break;
                default:
                    break;
            }
        }

        public static void SkillEffect_Heal(Unit _user, Unit _target,  int _healAmount)
        {
            _target.Heal(_user, _healAmount);
        }

        public static void SkillEffect_HealOverTime(Unit _user, Unit _target,  int _healAmount, int _duration)
        {

        }

        public static void SkillEffect_Damage(Unit _user, Unit _target,  int _damageAmount, bool _ignoreArmor = false)
        {
            int _damageToDeal = _damageAmount + _user.currentAtkStat;

            if (!_ignoreArmor)
            {
                _damageToDeal -= Mathf.FloorToInt(_target.currentDefStat * .75f);
            }

            _target.TakeDamage(_damageToDeal, _user);
        }

        public static void SkillEffect_DamageWithLifesteal(Unit _user, Unit _target, int _damageAmount, int _lifestealPercentage, bool _ignoreArmor)
        {
            int _damageToDeal = _damageAmount + _user.currentAtkStat;

            if (!_ignoreArmor)
            {
                _damageToDeal -= Mathf.FloorToInt(_target.currentDefStat * .75f);
            }

            _target.TakeDamage(_damageToDeal, _user);

            _user.Heal(_user, Mathf.CeilToInt(_damageAmount * _lifestealPercentage * .01f));
        }

        public static void SkillEffect_DamageOverTime(Unit _user, Unit _target,  int _damageAmount, int _duration)
        {

        }

        public static void SkillEffect_ApplyBleed(Unit _user, Unit _target,  int _bleedDamageAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_Bleed(_user, _target, _bleedDamageAmount, _duration));
        }

        public static void SkillEffect_ApplyBurn(Unit _user, Unit _target,  int _burnDamageAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_Burn(_user, _target, _burnDamageAmount, _duration));
        }

        public static void SkillEffect_ApplyFear(Unit _user, Unit _target,  int _duration)
        {
            _target.ApplyEffect(new StatusEffect_Fear(_user, _target, 0, _duration));
        }
        public static void SkillEffect_ApplySleep(Unit _user, Unit _target,  int _duration)
        {
            _target.ApplyEffect(new StatusEffect_Sleep(_user, _target, 0, _duration));
        }

        public static void SkillEffect_ApplyCharm(Unit _user, Unit _target,  int _duration)
        {

        }

        public static void SkillEffect_ApplyRoot(Unit _user, Unit _target,  int _duration)
        {
            _target.ApplyEffect(new StatusEffect_Root(_user, _target, 0, _duration));
        }

        public static void SkillEffect_ApplyKnockback(Unit _user, Unit _target,  int _knockbackAmount)
        {
            _target.motor.Knockback(_knockbackAmount, _user.transform.position);
        }

        public static void SkillEffect_BuffATK(Unit _user, Unit _target,  int _buffAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_BuffATK(_user, _target, _buffAmount, _duration));
        }

        public static void SkillEffect_BuffDEF(Unit _user, Unit _target,  int _buffAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_BuffDEF(_user, _target, _buffAmount, _duration));
        }

        public static void SkillEffect_BuffAP(Unit _user, Unit _target,  int _buffAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_BuffAP(_user, _target, _buffAmount, _duration));
        }

        public static void SkillEffect_DebuffATK(Unit _user, Unit _target,  int _debuffAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_DebuffATK(_user, _target, _debuffAmount, _duration));
        }

        public static void SkillEffect_DebuffDEF(Unit _user, Unit _target,  int _debuffAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_DebuffDEF(_user, _target, _debuffAmount, _duration));
        }

        public static void SkillEffect_DebuffAP(Unit _user, Unit _target,  int _debuffAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_DebuffAP(_user, _target, _debuffAmount, _duration));
        }

        public static void SkillEffect_Silver(SkillEffect _effect)
        {

        }

        public static void SkillEffect_Holy(SkillEffect _effect)
        {

        }

        public static void SkillEffect_OverrideTargetToSelf(SkillEffect _effect)
        {

        }

        public static void SkillEffect_ChangeAllegiance(Unit _user, Unit _target, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_ChangeAllegiance(_user, _target, 0, _duration));
        }
    }
}