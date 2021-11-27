using UnityEngine;

namespace Harpaesis.Combat
{
    public class TargetingTemplateNode : MonoBehaviour
    {
        public Material unselected;
        public Material selected;
        public Material valid;
        public Material validAndSelected;

        [ReadOnly] public Unit unit;
        [ReadOnly] public bool isEnemyUnit;

        Renderer rend;
        Skill skill;
        TargetingTemplate parent;

        TargetMask targetMask;
        bool isAOE;


        public void Init(TargetingTemplate _parent)
        {
            rend = GetComponent<Renderer>();
            parent = _parent;
            skill = parent.skill;
            targetMask = parent.skill.validTargets;
            isAOE = skill.targetingStyle == TargetingStyle.AOE;
        }


        bool IsValidTarget()
        {
            return (unit != null) && ((targetMask.HasFlag(TargetMask.Ally) && !isEnemyUnit) || (targetMask.HasFlag(TargetMask.Enemy) && isEnemyUnit));
        }

        private void OnTriggerStay(Collider other)
        {
            if (unit == null && other.GetComponent<Unit>() != null)
            {
                unit = other.GetComponent<Unit>();
                isEnemyUnit = (unit.GetType() == typeof(EnemyUnit));
                Enable();
            }
            else if (other.GetComponent<GridCursor>() != null)
            {
                if (isAOE) return;
                print("OnExit");
                parent.currentlySelected = this;

                rend.material = (IsValidTarget()) ? validAndSelected : selected;
            }

            print(other.GetComponent<GridCursor>());
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Unit>() != null)
            {
                unit = null;
                Enable();
            }
            else if (other.GetComponent<GridCursor>() != null)
            {
                if (isAOE) return;

                if (parent.currentlySelected == this)
                {
                    parent.currentlySelected = null;
                }

                rend.material = (IsValidTarget()) ? valid : unselected;
            }
        }

        public void Enable()
        {
            if (isAOE)
            {
                if (IsValidTarget())
                {
                    parent.AddToAOEList(this);
                    rend.material = validAndSelected;
                    return;
                }
                else if (parent.allWithTargets.Contains(this))
                {
                    parent.allWithTargets.Remove(this);
                }

                rend.material = valid;
                return;
            }

            rend.material = (IsValidTarget()) ? valid : unselected;
        }

        public void Disable()
        {
            unit = null;
            isEnemyUnit = false;
            rend.material = unselected;

            gameObject.SetActive(false);
        }
    }
}