using System.Collections;
using System.Collections.Generic;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

namespace Harpaesis.Combat
{
    public class SkillTargetingTemplate : MonoBehaviour
    {
        GridManager grid;

        public TemplateMouseOver currentlySelected;

        public Skill skill;

        public void Init(Skill _skill)
        {
            grid = GridManager.instance;

            skill = _skill;

            foreach (Transform child in transform)
            {
                child.GetComponent<TemplateMouseOver>().Init(this);
            }
        }

        public void SetupTargetingTemplate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform _child = transform.GetChild(i);

                if (grid.WorldPointIsWalkable(_child.position) && !LinecastToWorldPosition(_child.position))
                {
                    _child.gameObject.SetActive(true);
                }
                else
                {
                    _child.gameObject.SetActive(false);
                }
            }
        }

        public void Disable()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
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