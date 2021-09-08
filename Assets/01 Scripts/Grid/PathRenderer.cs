using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    LineRenderer path;

    public static PathRenderer instance;

    private void Awake()
    {
        path = GetComponent<LineRenderer>();

        if(instance == null)
        {
            instance = this;
            Deactivate();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Activate(Vector3 _startPosition, Vector3[] _positions)
    {
        path.positionCount = _positions.Length + 1;
        path.SetPositions(OrganizePath(_startPosition, _positions));
    }

    public void Deactivate()
    {
        path.positionCount = 0;
    }

    public Vector3[] OrganizePath(Vector3 _startPosition, Vector3[] _originalPath)
    {
        Vector3[] _newPath = new Vector3[_originalPath.Length + 1];

        _newPath[0] = _startPosition + Vector3.up * .25f;

        for (int i = 1; i < _newPath.Length; i++)
        {
            _newPath[i] = _originalPath[i - 1] + Vector3.up * .25f;
        }

        return _newPath;
    }
}
