using UnityEditor;
using UnityEngine;

namespace Harpaesis.Combat
{
    [System.Serializable]
    public class SkillEffect
    {
        public SkillEffectType effectType;
        public string amount;
        public string duration;
    }

    [CustomPropertyDrawer(typeof(SkillEffect))]
    public class SkillEffectEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var effectRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var secondRect = new Rect(position.x, position.y + 20f, position.width, EditorGUIUtility.singleLineHeight);
            var thirdRect = new Rect(position.x, position.y + 40f, position.width, EditorGUIUtility.singleLineHeight);

            var effect = property.FindPropertyRelative("effectType");
            var amount = property.FindPropertyRelative("amount");
            var duration = property.FindPropertyRelative("duration");

            effect.intValue = EditorGUI.Popup(effectRect, "Effect:", effect.intValue, effect.enumNames);

            switch ((SkillEffectType)effect.intValue)
            {
                case SkillEffectType.Heal:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Heal Amount:", amount.stringValue);
                    break;
                case SkillEffectType.HealOverTime:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Heal Amount:", amount.stringValue);
                    duration.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.Damage:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Damage Amount:", amount.stringValue);
                    break;
                case SkillEffectType.DamageOverTime:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Damage Amount:", amount.stringValue);
                    duration.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.ApplyBleed:
                    duration.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.ApplyBurn:
                    duration.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.ApplyFear:
                    duration.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.ApplySleep:
                    duration.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.ApplyCharm:
                    duration.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.ApplyRoot:
                    duration.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.ApplyKnockback:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Distance:", amount.stringValue);
                    break;
                case SkillEffectType.Lifesteal:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Lifesteal Percentage:", amount.stringValue);
                    break;
                case SkillEffectType.BuffATK:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Attack Buff Amount:", amount.stringValue);
                    duration.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.BuffDEF:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Defense Buff Amount:", amount.stringValue);
                    duration.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.BuffAP:
                    amount.stringValue = EditorGUI.TextField(secondRect, "AP Buff Amount:", amount.stringValue);
                    duration.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.DebuffATK:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Attack Debuff Amount:", amount.stringValue);
                    duration.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.DebuffDEF:
                    amount.stringValue = EditorGUI.TextField(secondRect, "Defense Debuff Amount:", amount.stringValue);
                    duration.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.DebuffAP:
                    amount.stringValue = EditorGUI.TextField(secondRect, "AP Debuff Amount:", amount.stringValue);
                    duration.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", duration.stringValue);
                    break;
                case SkillEffectType.Silvered:
                    break;
                case SkillEffectType.Holy:
                    break;
                case SkillEffectType.IgnoreArmor:
                    break;
                case SkillEffectType.OverrideTargetToSelf:
                    break;
                default:
                    break;
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (20 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight * 3.25f);
        }
    }

    public enum SkillEffectType
    {
        Heal,
        HealOverTime,
        Damage,
        DamageOverTime,

        ApplyBleed,
        ApplyBurn,
        ApplyFear,
        ApplySleep,
        ApplyCharm,
        ApplyRoot,
        ApplyKnockback,

        Lifesteal,

        BuffATK,
        BuffDEF,
        BuffAP,

        DebuffATK,
        DebuffDEF,
        DebuffAP,

        Silvered,
        Holy,
        IgnoreArmor,
        OverrideTargetToSelf
    }
}