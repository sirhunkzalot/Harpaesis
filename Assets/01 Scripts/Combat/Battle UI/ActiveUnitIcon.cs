using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUnitIcon : MonoBehaviour
{
    GameObject child;

    public void Init()
    {
        child = transform.GetChild(0).gameObject;
        SetChildActive(false);
    }

    public void SetChildActive(bool _active)
    {
        child.SetActive(_active);
    }
}
