using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public Transform endPoint;
    LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position + (Vector3.up * .1f));
        line.SetPosition(1, endPoint.position + (Vector3.up * .1f));
    }

    private void FixedUpdate()
    {
        line.SetPosition(1, endPoint.position + (Vector3.up * .1f));
    }

    public void SetSecondaryPosition(Transform _worldPosition)
    {
        endPoint.position = _worldPosition.position;
    }
}