using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    [CreateAssetMenu(menuName = "Unit/New Friendly Unit")]
    public class FriendlyUnitData : UnitData
    {
        [Space]
        public string nickname;

        [Header("Skills")]
        public Skill basicAttack;
        public Skill primarySkill;
        public Skill secondarySkill;
        public Skill tertiarySkill;
        public Skill signatureSkill;
    }
}