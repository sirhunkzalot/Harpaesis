using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine;

public class RotateTemplates : MonoBehaviour
{
    TargetingTemplate[] templates = new TargetingTemplate[5];
    int templateIndex;

    FriendlyUnit unit;

    public void Init(FriendlyUnit _unit)
    {
        unit = _unit;
    }

    public void InitTemplate(TargetingTemplate _template)
    {
        templates[templateIndex++] = _template;
    }

    private void FixedUpdate()
    {
        if (unit.currentState == FriendlyUnit.FriendlyState.Targeting_AOE || unit.currentState == FriendlyUnit.FriendlyState.Targeting_Single)
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if(Physics.Raycast(_ray, out _hit))
            {
                Vector3 _dir = (_hit.point - transform.position).normalized;
                float _angle = Vector3.Angle(_dir, transform.forward);
                if (_angle >= 45)
                {
                    transform.Rotate(Vector3.up, 90);
                    ReloadTemplates();

                }
                else if (_angle <= -45)
                {
                    transform.Rotate(Vector3.up, -90);
                    ReloadTemplates();
                }
            }
        }
    }

    void ReloadTemplates()
    {
        foreach (TargetingTemplate template in templates)
        {
            if (template.isActive)
            {
                template.SetupTargetingTemplate();
                return;
            }
        }
    }
}