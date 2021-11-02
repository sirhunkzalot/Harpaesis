using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    [CreateAssetMenu(menuName = "Unit/New Unit Data")]
    public class UnitData : ScriptableObject
    {
        public string unitName;

        [Header("Stats")]
        public int healthStat;
        public int inititiveStat;
        public int attackStat;
        public int defenseStat;
        public int apStat;

        [Header("Skills")]
        public Skill basicAttack;
        public Skill primarySkill;
        public Skill secondarySkill;
        public Skill tertiarySkill;
        public Skill signatureSkill;

        private void OnValidate()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
