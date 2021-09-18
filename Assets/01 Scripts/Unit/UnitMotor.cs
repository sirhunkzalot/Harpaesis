using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMotor : MonoBehaviour
{
    public float speed = 1.5f;

    public int targetIndex = 0;

    Unit unit;
    Vector3[] path;

    public void Init(Unit _unit)
    {
        unit = _unit;
    }

    public void Move(Vector3[] _path)
    {
        path = _path;

        StopCoroutine(FollowPath());
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        Vector3 _currentWaypoint = path[0];

        while (unit.turnData.ap > 0)
        {
            if (transform.position == _currentWaypoint)
            {
                unit.turnData.ap--;
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    ClearPath();
                    yield break;
                }
                _currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint, Time.deltaTime * speed);
            yield return null;
        }

        ClearPath();
    }

    void ClearPath()
    {
        unit.hasPath = false;
        path = null;

        targetIndex = 0;
    }
}
