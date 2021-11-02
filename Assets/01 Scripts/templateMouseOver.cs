using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    public class TemplateMouseOver : MonoBehaviour
    {
        public Material unselected;
        public Material selected;
        public Material valid;

        [HideInInspector] public Unit allyUnit, enemyUnit;

        Renderer rend;
        SkillTargetingTemplate parent;

        TargetMask targetMask;

        public void Init(SkillTargetingTemplate _parent)
        {
            parent = _parent;
            targetMask = _parent.skill.validTargets;

            rend = GetComponent<Renderer>();
        }

        public void OnMouseOver()
        {
            parent.currentlySelected = this;

            if (targetMask == TargetMask.Ally && allyUnit != null)
            {
                rend.material = valid;
                return;
            }
            else if (targetMask == TargetMask.Enemy && enemyUnit != null)
            {
                rend.material = valid;
                return;
            }
            if (targetMask == TargetMask.Self)
            {
                print("ss");
                return;
            }


            rend.material = selected;
        }

        public void OnMouseExit()
        {
            if(parent.currentlySelected == this)
            {
                parent.currentlySelected = null;
            }

            rend.material = unselected;
        }

        private void OnTriggerStay(Collider other)
        {
            if (allyUnit == null && enemyUnit == null && other.GetComponent<Unit>() != null)
            {
                Unit _unit = other.GetComponent<Unit>();
                if(_unit.GetType() == typeof(FriendlyUnit))
                {
                    allyUnit = (FriendlyUnit)_unit;
                }
                else if (_unit.GetType() == typeof(EnemyUnit))
                {
                    enemyUnit = (EnemyUnit)_unit;
                }
            }
        }



        private void OnTriggerExit(Collider other)
        {
            allyUnit = null;
            enemyUnit = null;
        }
    }
}