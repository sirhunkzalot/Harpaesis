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
            int _amount = CombatUtility.TranslateFormula(_effect.amount);
            int _duration = CombatUtility.TranslateFormula(_effect.duration);

            switch (_effect.effectType)
            {
                case SkillEffectType.Heal:
                    SkillEffect_Heal(_user, _target, _amount);
                    break;
                case SkillEffectType.HealOverTime:
                    SkillEffect_HealOverTime(_user, _target, _amount, _duration);
                    break;
                case SkillEffectType.Damage:
                    SkillEffect_Damage(_user, _target,  _amount);
                    break;
                case SkillEffectType.DamageOverTime:
                    SkillEffect_DamageOverTime(_user, _target,  _amount, _duration);
                    break;
                case SkillEffectType.ApplyBleed:
                    SkillEffect_ApplyBleed(_user, _target,  _amount, _duration);
                    break;
                case SkillEffectType.ApplyBurn:
                    SkillEffect_ApplyBurn(_user, _target,  _amount, _duration);
                    break;
                case SkillEffectType.ApplyFear:
                    SkillEffect_ApplyFear(_user, _target,  _duration);
                    break;
                case SkillEffectType.ApplySleep:
                    SkillEffect_ApplySleep(_user, _target,  _duration);
                    break;
                case SkillEffectType.ApplyCharm:
                    SkillEffect_ApplyCharm(_user, _target,  _duration);
                    break;
                case SkillEffectType.ApplyRoot:
                    SkillEffect_ApplyEntangle(_user, _target,  _duration);
                    break;
                case SkillEffectType.ApplyKnockback:
                    SkillEffect_ApplyKnockback(_user, _target,  _amount);
                    break;
                case SkillEffectType.Lifesteal:
                    SkillEffect_Lifesteal(_user, _target,  _amount);
                    break;
                case SkillEffectType.BuffATK:
                    SkillEffect_BuffATK(_user, _target,  _amount, _duration);
                    break;
                case SkillEffectType.BuffDEF:
                    SkillEffect_BuffDEF(_user, _target,  _amount, _duration);
                    break;
                case SkillEffectType.BuffAP:
                    SkillEffect_BuffAP(_user, _target,  _amount, _duration);
                    break;
                case SkillEffectType.DebuffATK:
                    SkillEffect_DebuffATK(_user, _target,  _amount, _duration);
                    break;
                case SkillEffectType.DebuffDEF:
                    SkillEffect_DebuffDEF(_user, _target,  _amount, _duration);
                    break;
                case SkillEffectType.DebuffAP:
                    SkillEffect_DebuffAP(_user, _target,  _amount, _duration);
                    break;
                case SkillEffectType.Silvered:
                    SkillEffect_Silver(_effect);
                    break;
                case SkillEffectType.Holy:
                    SkillEffect_Holy(_effect);
                    break;
                case SkillEffectType.IgnoreArmor:
                    SkillEffect_IgnoreArmor(_effect);
                    break;
                case SkillEffectType.OverrideTargetToSelf:
                    SkillEffect_OverrideTargetToSelf(_effect);
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

        public static void SkillEffect_Damage(Unit _user, Unit _target,  int _damageAmount, bool _ignoreArmor = false, float _lifestealPercentage = 0)
        {
            int _damageToDeal = _damageAmount + _user.unitData.attackStat;

            if (!_ignoreArmor)
            {
                _damageToDeal -= Mathf.FloorToInt(_target.unitData.defenseStat * .75f);
            }

            _target.TakeDamage(_user, _damageToDeal);

            if (_lifestealPercentage > 0)
            {
                _user.Heal(_user, Mathf.CeilToInt(_damageToDeal * _lifestealPercentage));
            }
        }

        public static void SkillEffect_DamageOverTime(Unit _user, Unit _target,  int _damageAmount, int _duration)
        {

        }

        public static void SkillEffect_ApplyBleed(Unit _user, Unit _target,  int _bleedDamageAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_Bleed(_target, _bleedDamageAmount, _duration));
        }

        public static void SkillEffect_ApplyBurn(Unit _user, Unit _target,  int _burnDamageAmount, int _duration)
        {
            _target.ApplyEffect(new StatusEffect_Burn(_target, _burnDamageAmount, _duration));
        }

        public static void SkillEffect_ApplyFear(Unit _user, Unit _target,  int _duration)
        {

        }
        public static void SkillEffect_ApplySleep(Unit _user, Unit _target,  int _duration)
        {

        }

        public static void SkillEffect_ApplyCharm(Unit _user, Unit _target,  int _duration)
        {

        }

        public static void SkillEffect_ApplyEntangle(Unit _user, Unit _target,  int _duration)
        {
            _target.ApplyEffect(new StatusEffect_Root(_target, 0, _duration));
        }

        public static void SkillEffect_ApplyKnockback(Unit _user, Unit _target,  int _knockbackAmount)
        {

        }
        public static void SkillEffect_Lifesteal(Unit _user, Unit _target,  int _percentage)
        {

        }

        public static void SkillEffect_BuffATK(Unit _user, Unit _target,  int _buffAmount, int _duration)
        {

        }

        public static void SkillEffect_BuffDEF(Unit _user, Unit _target,  int _buffAmount, int _duration)
        {

        }

        public static void SkillEffect_BuffAP(Unit _user, Unit _target,  int _buffAmount, int _duration)
        {

        }

        public static void SkillEffect_DebuffATK(Unit _user, Unit _target,  int _debuffAmount, int _duration)
        {

        }

        public static void SkillEffect_DebuffDEF(Unit _user, Unit _target,  int _debuffAmount, int _duration)
        {

        }

        public static void SkillEffect_DebuffAP(Unit _user, Unit _target,  int _debuffAmount, int _duration)
        {

        }

        public static void SkillEffect_Silver(SkillEffect _effect)
        {

        }

        public static void SkillEffect_Holy(SkillEffect _effect)
        {

        }

        public static void SkillEffect_IgnoreArmor(SkillEffect _effect)
        {

        }

        public static void SkillEffect_OverrideTargetToSelf(SkillEffect _effect)
        {

        }
    }
}