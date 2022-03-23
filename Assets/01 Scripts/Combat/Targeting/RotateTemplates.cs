using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine;

public class RotateTemplates : MonoBehaviour
{
    TargetingTemplate[] templates = new TargetingTemplate[6];
    int templateIndex;

    FriendlyUnit unit;
    GridCursor cursor;
    Transform cam;

    Vector3 lastPos;
    bool templateUnlocked;

    public void Init(FriendlyUnit _unit)
    {
        unit = _unit;
        cursor = GridCursor.instance;
        cam = Camera.main.transform;
    }

    public void InitTemplate(TargetingTemplate _template)
    {
        templates[templateIndex++] = _template;
    }

    public void UnlockTemplate()
    {
        transform.SetParent(GridCursor.instance.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        templateUnlocked = true;
    }

    public void LockTemplate()
    {
        transform.SetParent(unit.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        templateUnlocked = false;
    }

    private void FixedUpdate()
    {
        if ((unit.currentState == FriendlyUnit.FriendlyState.Targeting_AOE || unit.currentState == FriendlyUnit.FriendlyState.Targeting_Single) && unit.canRotateTemplates)
        {
            Vector3 _dir = (cursor.transform.position - transform.position).normalized;
            float _angle = Vector3.Angle(_dir, transform.forward);

            if (_angle >= 90)
            {
                transform.Rotate(Vector3.up, 90);
                ReloadTemplates();

            }
            else if (_angle <= -90)
            {
                transform.Rotate(Vector3.up, -90);
                ReloadTemplates();
            }
        }

        if(templateUnlocked && transform.position != lastPos)
        {
            lastPos = transform.position;
            ReloadTemplates();
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