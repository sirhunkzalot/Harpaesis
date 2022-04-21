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
        public int projectileRange = -1;
        public int aoeRadius = -1;
        [TextArea(3, 5)] public string skillDescription;
        public GameObject targetingTemplate;
        public bool implemented = true;
        public Sprite skillSprite;

        [Space]
        public List<SkillEffect> effects = new List<SkillEffect>();

        // Applies effects and costs of the skill
        public void UseSkill(Unit _user, Unit _target)
        {
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
                for (int i = 0; i < _targets.Length; i++)
                {
                    UseSkill(_user, _targets[i]);
                }
            }
            else
            {
                UseSkill(_user, _targets[0]);
            }
        } // end UseSkill

        // Temp
        public void UseProjectileSkill(Unit _user, List<Vector3> _positions)
        {
            SkillEffectLibrary.SkillEffect_SpawnHazardTile(_user, _user, effects[0].param1, _positions);
        }

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
        AOE,
        ProjectileAOE
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