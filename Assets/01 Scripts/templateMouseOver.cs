using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMouseOver : MonoBehaviour
{
    public Material unselected;
    public Material selected;
    public Material valid;

    Renderer rend;
    SkillTargetingTemplate parent;

    public void Init(SkillTargetingTemplate _parent)
    {
        parent = _parent;
        rend = GetComponent<Renderer>();
    }

    public void OnMouseOver()
    {
        rend.material = valid;
    }

    public void OnMouseExit()
    {
        rend.material = unselected;
    }
}
