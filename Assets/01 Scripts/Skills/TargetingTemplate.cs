using System.Collections;
using System.Collections.Generic;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

namespace Harpaesis.Combat
{
    public class TargetingTemplate : MonoBehaviour
    {
        GridManager grid;

        public List<TargetingTemplateNode> templateNodes = new List<TargetingTemplateNode>();

        [ReadOnly] public TargetingTemplateNode currentlySelected;
        [ReadOnly] public List<TargetingTemplateNode> allWithTargets;

        [ReadOnly] public Skill skill;

        public void Init(Skill _skill)
        {
            grid = GridManager.instance;

            skill = _skill;

            foreach (Transform child in transform)
            {
                TargetingTemplateNode _node = child.GetComponent<TargetingTemplateNode>();

                templateNodes.Add(_node);
                _node.Init(this);
                _node.Enable();
            }
        }

        public void SetupTargetingTemplate()
        {
            if (skill.targetingStyle == TargetingStyle.AOE)
            {
                allWithTargets = new List<TargetingTemplateNode>();
            }

            foreach (TargetingTemplateNode node in templateNodes)
            {
                if (grid.WorldPointIsWalkable(node.transform.position) && !LinecastToWorldPosition(node.transform.position))
                {
                    node.gameObject.SetActive(true);
                    node.Enable();
                }
                else
                {
                    node.gameObject.SetActive(false);
                }
            }
        }

        public void Disable()
        {
            foreach (TargetingTemplateNode node in templateNodes)
            {
                node.Disable();
            }
        }

        private bool LinecastToWorldPosition(Vector3 _worldPosition)
        {
            Vector3 _origin = transform.position + Vector3.up;
            Vector3 _targetPosition = _worldPosition + Vector3.up;

            return Physics.Linecast(_origin, _targetPosition);
        }
    }
}