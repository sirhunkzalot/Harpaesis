using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Harpaesis.Combat
{
    [CreateAssetMenu(menuName = "Skills/New Skill")]
    public class Skill : ScriptableObject
    {
        public string skillName;
        public int apCost = 1;
        [EnumFlags] public TargetMask validTargets;
        public TargetingStyle targetingStyle;
        [TextArea(3, 5)] public string skillDescription;
        public GameObject targetingTemplate;
        public bool implemented = true;
        public Sprite skillSprite;

        [Space]
        public List<SkillEffect> effects = new List<SkillEffect>();

        // Applies effects and costs of the skill
        public void UseSkill(Unit _user, Unit _target, bool _useAP = true)
        {
            if (_useAP)
            {
                _user.turnData.ap -= apCost;
            }

            BattleLog.Log($"{_user} uses {skillName} on {_target}!", BattleLogType.Combat);
            if (effects.Count > 0)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    SkillEffectLibrary.ResolveEffect(_user, _target, effects[i]);
                }
            }
        } // end UseSkill

        // Applies effects and costs of the skill
        public void UseSkill(Unit _user, Unit[] _targets)
        {
            if (_targets.Length > 0)
            {
                bool _useAP = true;
                for (int i = 0; i < _targets.Length; i++)
                {
                    UseSkill(_user, _targets[i], _useAP);
                    _useAP = false;
                }
            }
            else
            {
                UseSkill(_user, _targets[0]);
            }
        } // end UseSkill

        #region Set Dirty
        private void OnValidate()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        #endregion
    }

    public enum TargetingStyle
    {
        SingleTarget,
        AOE
    }

    [Flags]
    public enum TargetMask
    {
        Self = 1 << 0,
        Ally = 1 << 1,
        Enemy = 1 << 2,
    }

    public class EnumFlagsAttribute : PropertyAttribute
    {
        public EnumFlagsAttribute() { }
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            _property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
        }
    }
#endif
}