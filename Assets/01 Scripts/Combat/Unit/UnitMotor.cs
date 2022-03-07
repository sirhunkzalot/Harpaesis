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

    public bool isMoving;

    Unit unit;
    [SerializeField, ReadOnly] Waypoint[] path;

    GridManager grid;

    public void Init(Unit _unit)
    {
        unit = _unit;
        grid = GridManager.instance;
    }

    public void Move(Waypoint[] _path)
    {
        path = _path;

        StopCoroutine(FollowPath());
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        isMoving = true;

        if (path.Length >= 1 && unit.canMove)
        {
            grid.NodeFromWorldPoint(transform.position).hasUnit = false;

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
                        isMoving = false;
                        Node _nope = grid.NodeFromWorldPoint(transform.position);
                        transform.position = _nope.worldPosition;
                        _nope.hasUnit = true;
                        yield break;
                    }
                    _currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.position, Time.deltaTime * speed * (1 + (unit.unitData.inititiveStat / 10f)));
                yield return null;
            }
        }
        isMoving = false;
        Node _node = grid.NodeFromWorldPoint(transform.position);
        transform.position = _node.worldPosition;
        _node.hasUnit = true;
        ClearPath();
    }

    IEnumerator ForceMovement(Vector3 _targetPosition, Node _node)
    {
        print($"{transform.position} -> {_targetPosition} = {Vector3.Distance(transform.position, _targetPosition)}");
        while (Vector3.Distance(transform.position, _targetPosition) > .05f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, GameSettings.statusEffectSettings.knockbackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        transform.position = _targetPosition;

    }

    public void RunAway(Vector3 _fromPosition)
    {
        Vector3 _dir = (transform.position - _fromPosition).normalized;
        Vector3 _targetPosition = transform.position + (_dir * 1000);

        PathRequestManager.RequestPath(new PathRequest(transform.position, _targetPosition, Run, unit));
    }

    public void Knockback(int _distance, Vector3 _fromPosition)
    {
        Vector3 _dir = (transform.position - _fromPosition).normalized;

        Node _desiredNode = grid.NodeFromWorldPoint(transform.position);
        Vector3 _targetNodePosition = _desiredNode.worldPosition;

        for (int i = 1; i < _distance + 1; i++)
        {
            Node _node = grid.NodeFromWorldPoint(transform.position + (_dir * i));

            if (!_node.hasUnit && grid.NodeIsWalkable(_node))
            {
                _targetNodePosition = _node.worldPosition;
                _desiredNode = _node;
            }
            else
            {
                break;
            }
        }

        grid.NodeFromWorldPoint(transform.position).hasUnit = false;
        _desiredNode.hasUnit = true;


        StartCoroutine(ForceMovement(_targetNodePosition, _desiredNode));
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