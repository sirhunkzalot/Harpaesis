using UnityEditor;
using UnityEngine;

namespace Harpaesis.Combat
{
    [System.Serializable]
    public class SkillEffect
    {
        public SkillEffectType effectType;
        public string param1;
        public string param2;
        public DamageType damageType;
    }

#if UNITY_EDITOR
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
            var fourthRect = new Rect(position.x, position.y + 60f, position.width, EditorGUIUtility.singleLineHeight);

            var effect = property.FindPropertyRelative("effectType");
            var param1 = property.FindPropertyRelative("param1");

            var param2 = property.FindPropertyRelative("param2");
            var damageType = property.FindPropertyRelative("damageType");

            effect.intValue = EditorGUI.Popup(effectRect, "Effect:", effect.intValue, effect.enumNames);

            switch ((SkillEffectType)effect.intValue)
            {
                case SkillEffectType.Heal:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Heal Amount:", param1.stringValue);
                    break;
                case SkillEffectType.HealOverTime:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Heal Amount:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", param2.stringValue);
                    break;
                case SkillEffectType.Damage:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Damage Amount:", param1.stringValue);
                    damageType.intValue = EditorGUI.Popup(thirdRect, "Damage Type:", damageType.intValue, damageType.enumNames);
                    break;
                case SkillEffectType.DamageWithLifesteal:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Damage Amount:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Lifesteal Percentage:", param2.stringValue);
                    damageType.intValue = EditorGUI.Popup(fourthRect, "Damage Type: :", damageType.intValue, damageType.enumNames);
                    break;
                case SkillEffectType.DamageOverTime:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Damage Amount:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", param2.stringValue);
                    break;
                case SkillEffectType.ApplyBleed:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.ApplyBurn:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.ApplyFear:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.ApplySleep:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.ApplyCharm:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.ApplyRoot:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.ApplyKnockback:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Distance:", param1.stringValue);
                    break;
                case SkillEffectType.BuffATK:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Attack Buff Amount:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", param2.stringValue);
                    break;
                case SkillEffectType.BuffDEF:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Defense Buff Amount:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", param2.stringValue);
                    break;
                case SkillEffectType.BuffAP:
                    param1.stringValue = EditorGUI.TextField(secondRect, "AP Buff Amount:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", param2.stringValue);
                    break;
                case SkillEffectType.DebuffATK:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Attack Debuff Amount:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", param2.stringValue);
                    break;
                case SkillEffectType.DebuffDEF:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Defense Debuff Amount:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", param2.stringValue);
                    break;
                case SkillEffectType.DebuffAP:
                    param1.stringValue = EditorGUI.TextField(secondRect, "AP Debuff Amount:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Duration in Turns:", param2.stringValue);
                    break;
                case SkillEffectType.ChangeAllegiance:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.SpawnPrefab:
                    param1.stringValue = EditorGUI.TextField(secondRect, "File Path:", param1.stringValue);
                    break;
                case SkillEffectType.BoilBlood:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Damage:", param1.stringValue);
                    param2.stringValue = EditorGUI.TextField(thirdRect, "Range:", param2.stringValue);
                    break;
                case SkillEffectType.Taunt:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.Bulwark:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.CleanseNegativeEffects:
                    break;
                case SkillEffectType.ResistNegativeEffects:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Duration in Turns:", param1.stringValue);
                    break;
                case SkillEffectType.DivineIntervention:
                    param1.stringValue = EditorGUI.TextField(secondRect, "Heal Amount:", param1.stringValue);
                    break;
                default:
                    break;
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (20 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight * 4.25f);
        }
    }
#endif
    public enum SkillEffectType
    {
        Heal,
        HealOverTime,
        Damage,
        DamageWithLifesteal,
        DamageOverTime,

        ApplyBleed,
        ApplyBurn,
        ApplyFear,
        ApplySleep,
        ApplyCharm,
        ApplyRoot,
        ApplyKnockback,

        BuffATK,
        BuffDEF,
        BuffAP,

        DebuffATK,
        DebuffDEF,
        DebuffAP,

        ChangeAllegiance,
        SpawnPrefab,
        BoilBlood,
        Taunt,
        Bulwark,
        CleanseNegativeEffects,
        ResistNegativeEffects,
        DivineIntervention
    }
}