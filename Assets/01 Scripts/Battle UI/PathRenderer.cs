using System.Collections;
using System.Collections.Generic;
using GridAndPathfinding;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    public LineRenderer reachablePath;
    public LineRenderer unreachablePath;

    public Vector3 pathOffset;

    public Vector3[] path;
    public Unit unit;

    public static PathRenderer instance;
    private void Awake()
    {
        instance = this;
        Deactivate();
    }

    private void Update()
    {
        if(unit != null && reachablePath.positionCount > 0)
        {
            reachablePath.SetPosition(0, unit.transform.position + pathOffset);
        }
    }

    public void Activate(PathResult _result)
    {
        reachablePath.gameObject.SetActive(true);
        unreachablePath.gameObject.SetActive(true);

        unit = _result.unit;

        Vector3[] _newPath = new Vector3[_result.path.Length + 1];
        _newPath[0] = unit.transform.position + pathOffset;
        _result.path.CopyTo(_newPath, 1);

        path = _newPath;


        int _reachableSpaces = Mathf.Min(path.Length, unit.turnData.ap + 1);
        int _unreachableSpaces = Mathf.Max(0, path.Length - _reachableSpaces) + 1;


        // Sets the positions for all of the reachableSpaces in the path
        reachablePath.positionCount = _reachableSpaces;
        if (reachablePath.positionCount > 0)
        {
            Vector3[] _reachablePositions = new Vector3[_reachableSpaces];

            for (int i = 0; i < _reachableSpaces; i++)
            {
                _reachablePositions[i] = path[i] + pathOffset;
            }

            reachablePath.SetPositions(_reachablePositions);
        }
        
        // Sets the positions for all of the unreachableSpaces in the path
        unreachablePath.positionCount = _unreachableSpaces;
        if(unreachablePath.positionCount > 0)
        {
            Vector3[] _unreachablePositions = new Vector3[_unreachableSpaces + 1];

            for (int i = 0; i < _unreachableSpaces; i++)
            {
                _unreachablePositions[i] = path[i + reachablePath.positionCount - 1] + pathOffset;
            }

            unreachablePath.SetPositions(_unreachablePositions);
        }
    }

    void Deactivate()
    {
        reachablePath.gameObject.SetActive(false);
        unreachablePath.gameObject.SetActive(false);
    }
}
