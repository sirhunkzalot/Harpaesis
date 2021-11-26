using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    [CreateAssetMenu(menuName = "Unit/New Enemy Unit")]
    public class EnemyUnitData : UnitData
    {
        public EnemyTargetingStyle enemyTargetingStyle = EnemyTargetingStyle.Closest;
        public EnemySkill[] enemySkills;
    }

    [System.Serializable]
    public class EnemySkill
    {
        public Skill skill;
        public int rangeEstimate;

        [Range(0, 100)] public int shortRangeWeight;
        [Range(0, 100)] public int mediumRangeWeight;
        [Range(0, 100)] public int longRangeWeight;
    }

    public enum EnemyTargetingStyle
    {
        Closest,
        Furthest,
        HighestHP,
        LowestHP
    }
}