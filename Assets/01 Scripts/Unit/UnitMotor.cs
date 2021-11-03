using System.Collections;
using System.Collections.Generic;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class UnitMotor handles all movement logic for each individual unit */
public class UnitMotor : MonoBehaviour
{
    public float speed = 1.5f;

    public int targetIndex = 0;

    Unit unit;
    [SerializeField, ReadOnly] Waypoint[] path;

    public void Init(Unit _unit)
    {
        unit = _unit;
    }

    public void Move(Waypoint[] _path)
    {
        path = _path;

        StopCoroutine(FollowPath());
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        if (path.Length >= 1 && unit.canMove)
        {
            Waypoint _currentWaypoint = path[0];

            while (unit.turnData.ap > 0)
            {
                if (transform.position == _currentWaypoint.position)
                {
                    unit.OnTakeStep();
                    unit.turnData.ap -= _currentWaypoint.apCost;
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        ClearPath();
                        yield break;
                    }
                    _currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.position, Time.deltaTime * speed);
                yield return null;
            }
        }

        ClearPath();
    }

    public void RunAway(Vector3 _fromPosition)
    {
        Vector3 _dir = (transform.position - _fromPosition).normalized;
        Vector3 _targetPosition = transform.position + (_dir * 1000);

        PathRequestManager.RequestPath(new PathRequest(transform.position, _targetPosition, Run, unit));
    }

    void Run(PathResult _result)
    {
        Move(_result.path);
    }

void ClearPath()
    {
        unit.hasPath = false;
        path = null;

        targetIndex = 0;
    }
}

[System.Serializable]
public struct Waypoint
{
    public Vector3 position;
    public int apCost;

    public Waypoint(Vector3 _position, int _apCost = 1)
    {
        position = _position;
        apCost = _apCost;
    }
}