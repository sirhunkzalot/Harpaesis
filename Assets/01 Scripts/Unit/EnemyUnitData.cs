using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    [CreateAssetMenu(menuName = "Unit/New Enemy Unit")]
    public class EnemyUnitData : UnitData
    {
        public EnemySkill[] enemySkills;
        public int WeightTotal
        {
            get
            {
                int _weightTotal = 0;

                if(enemySkills.Length > 0)
                {
                    for (int i = 0; i < enemySkills.Length; i++)
                    {
                        _weightTotal += enemySkills[i].weight;
                    }
                }

                return _weightTotal;
            }
        }


    }

    [System.Serializable]
    public struct EnemySkill
    {
        public Skill skill;
        [Range(0,100)] public int weight;
    }
}