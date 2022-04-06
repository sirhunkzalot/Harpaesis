using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    [CreateAssetMenu(menuName = "Unit/New Enemy Unit")]
    public class EnemyUnitData : UnitData
    {
        public EnemySkill[] enemySkills;
    }

    [System.Serializable]
    public class EnemySkill
    {
        public Skill skill;
        public int rangeEstimate;

        [Space]
        [Range(1, 100)] public int shortRangeWeight = 1;
        [Range(1, 100)] public int mediumRangeWeight = 1;
        [Range(1, 100)] public int longRangeWeight = 1;

        [Space]
        [Range(1, 100)] public int highHPTargetWeight = 1;
        [Range(1, 100)] public int lowHPTargetWeight = 1;

        [Space]
        [Range(1, 100)] public int aoeTargetWeight = 1;
    }
}