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
        [ReadOnly] public List<TargetingTemplateNode> allActiveNodes;

        [ReadOnly] public Skill skill;

        [ReadOnly] public bool isActive;

        public bool canRotate = false;

        public LayerMask layerMask;

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
            switch (skill.targetingStyle)
            {
                case TargetingStyle.AOE:
                    allWithTargets = new List<TargetingTemplateNode>();
                    allActiveNodes = new List<TargetingTemplateNode>();
                    break;
                case TargetingStyle.ProjectileAOE:
                    allWithTargets = new List<TargetingTemplateNode>();
                    allActiveNodes = new List<TargetingTemplateNode>();
                    break;
                default:
                    break;
            }

            foreach (TargetingTemplateNode node in templateNodes)
            {
                if (grid.WorldPointIsWalkable(node.transform.position) && !grid.LinecastToWorldPoint(transform.position, node.transform.position))
                {
                    node.gameObject.SetActive(true);
                    node.Enable();
                    allActiveNodes.Add(node);
                }
                else
                {
                    node.gameObject.SetActive(false);
                }
            }

            isActive = true;
        }

        public void AddToAOEList(TargetingTemplateNode _node)
        {
            if (allWithTargets.Count > 0)
            {
                float _newDis = Vector3.Distance(_node.transform.position, transform.position);

                for (int i = 0; i < allWithTargets.Count; i++)
                {
                    float _oldDis = Vector3.Distance(allWithTargets[i].transform.position, transform.position);

                    if (_newDis > _oldDis)
                    {
                        allWithTargets.Insert(i, _node);
                        return;
                    }
                }
            }

            allWithTargets.Add(_node);
        }

        public void Disable()
        {
            foreach (TargetingTemplateNode node in templateNodes)
            {
                node.Disable();
            }
            isActive = false;
        }
    }
}