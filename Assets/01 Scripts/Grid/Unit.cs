using System.Collections;
using System.Collections.Generic;
using GridAndPathfinding;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed;
    protected Vector3[] previewPath;
    protected Vector3[] path;
    protected int targetIndex;

    public int maxAP = 8;
    public int currentAP = 8;


    protected GridManager grid;


    private void Start()
    {
        grid = GridManager.instance;
        Init();
    }

    private void Update()
    {
        Tick();
    }

    protected virtual void Init() { }
    protected virtual void Tick() { }

    public void PreviewPath(PathResult _result)
    {
        if (_result.success)
        {
            previewPath = _result.path;
            PathRenderer.instance.Activate(transform.position, previewPath);
        }
    }

    public void OnPathFound(PathResult _result)
    {
        if (_result.success)
        {
            path = _result.path;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

    protected IEnumerator FollowPath()
    {
        Vector3 _currentWaypoint = path[0];

        while (true)
        {
            if(transform.position == _currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    ClearPath();
                    yield break;
                }
                _currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint, Time.deltaTime * speed);
            yield return null;
        }
    }

    protected void ClearPath()
    {
        targetIndex = 0;
        path = null;
    }

    public void OnDrawGizmos()
    {
        /*if(path != null)4
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * .06125f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
        /*if (previewPath != null)
        {
            for (int i = 1; i < previewPath.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(previewPath[i], Vector3.one * .06125f);
                Gizmos.DrawLine(previewPath[i - 1], previewPath[i]);
            }
        }*/
    }
}
