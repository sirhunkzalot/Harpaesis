using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    [CreateAssetMenu(menuName = "Skills/New Skill")]
    public class Skill : ScriptableObject
    {
        public string skillName;
        public int apCost = 1;
        public TargetType validTargets;
        [TextArea(3, 5)] public string skillDescription;

        [Space]
        public List<SkillEffect> effects = new List<SkillEffect>();

        // Applies effects and costs of the skill
        public void UseSkill(Unit _user, Unit _target)
        {
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

        #region Set Dirty
        private void OnValidate()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        #endregion
    }

    public enum TargetType
    {
        Any,
        Self,
        Enemy,
        Ally
    }
}