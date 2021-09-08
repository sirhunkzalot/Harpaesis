using System.Collections;
using System.Collections.Generic;
using GridAndPathfinding;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed;
    Vector3[] previewPath;
    Vector3[] path;
    int targetIndex;

    public Transform selector;
    public Vector3 lastSelectorPosition;

    public Vector3 nodePosition;
    public Vector3 myLastNodePosition;

    public int maxAP = 8;
    public int currentAP = 8;


    GridManager grid;


    private void Start()
    {
        grid = FindObjectOfType<GridManager>();
        //PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
    }

    private void Update()
    {
        nodePosition = grid.NodeFromWorldPoint(transform.position).worldPosition;

        if (selector.position != lastSelectorPosition || nodePosition != myLastNodePosition)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, selector.position, PreviewPath));
            lastSelectorPosition = selector.position;
            myLastNodePosition = nodePosition;
        }

        if(Input.GetMouseButtonDown(0) && previewPath != null && path == null)
        {
            path = previewPath;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

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

    IEnumerator FollowPath()
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

    void ClearPath()
    {
        targetIndex = 0;
        path = null;
    }

    public void OnDrawGizmos()
    {
        /*if(path != null)
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
