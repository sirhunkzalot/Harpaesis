using System.Collections.Generic;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class PathRenderer manages the logic required to display paths to the player */
public class PathRenderer : MonoBehaviour
{
    public LineRenderer reachablePath;
    public LineRenderer unreachablePath;
    public LineRenderer actualPath;

    public Vector3 pathOffset;

    public Waypoint[] path;
    public Unit unit;

    public bool renderActualPath;

    public static PathRenderer instance;
    private void Awake()
    {
        instance = this;
        DeactivateAllPaths();
    }

    private void Update()
    {
        if(unit != null && reachablePath.positionCount > 0)
        {
            if(!renderActualPath)
            {
                reachablePath.SetPosition(0, unit.transform.position + pathOffset);
            }
            else
            {
                actualPath.SetPosition(0, unit.transform.position + pathOffset);

                if (Vector3.Distance(actualPath.GetPosition(0), actualPath.GetPosition(1)) <= .05)
                {
                    if(actualPath.positionCount == 2)
                    {
                        DeactivateAllPaths();
                        return;
                    }

                    for (int i = 1; i < actualPath.positionCount - 1; i++)
                    {
                        actualPath.SetPosition(i, actualPath.GetPosition(i + 1));
                    }

                    actualPath.positionCount--;
                }
            }
        }
    }

    public void PathPreview(PathResult _result)
    {
        reachablePath.gameObject.SetActive(true);
        unreachablePath.gameObject.SetActive(true);

        unit = _result.unit;

        Waypoint[] _newPath = new Waypoint[_result.path.Length + 1];
        _newPath[0] = new Waypoint(unit.transform.position + pathOffset, 0);
        _result.path.CopyTo(_newPath, 1);

        path = _newPath;

        int _usedAP = 0;
        int i = 0;

        // Sets the positions for all of the reachableSpaces in the path
        if (unit.turnData.ap > 0)
        {
            List<Vector3> _reachablePositions = new List<Vector3>();

            do
            {
                _reachablePositions.Add(path[i].position + pathOffset);
                _usedAP += path[i].apCost;

            } while (++i < path.Length && _usedAP < unit.turnData.ap);

            reachablePath.positionCount = _reachablePositions.Count;
            reachablePath.SetPositions(_reachablePositions.ToArray());
        }
        else
        {
            reachablePath.gameObject.SetActive(false);
        }

        // Sets the positions for all of the unreachableSpaces in the path
        
        if(i < path.Length)
        {

            List<Vector3> _unreachablePositions = new List<Vector3>();

            if(i > 0)
            {
                i--;
            }


            while (i < path.Length)
            {
                _unreachablePositions.Add(path[i].position + pathOffset);

                i++;
            }

            unreachablePath.positionCount = _unreachablePositions.Count;
            unreachablePath.SetPositions(_unreachablePositions.ToArray());
        }
        else
        {
            unreachablePath.gameObject.SetActive(false);
        }
    }

    public void SwapToActualPath()
    {
        if(reachablePath.positionCount > 1 && reachablePath.gameObject.activeInHierarchy)
        {
            actualPath.gameObject.SetActive(true);
            actualPath.positionCount = reachablePath.positionCount;

            Vector3[] _actualPositions = new Vector3[reachablePath.positionCount];
            reachablePath.GetPositions(_actualPositions);
            actualPath.SetPositions(_actualPositions);

            renderActualPath = true;
        }

        reachablePath.gameObject.SetActive(false);
        unreachablePath.gameObject.SetActive(false);
    }

    void DeactivateAllPaths()
    {
        reachablePath.gameObject.SetActive(false);
        unreachablePath.gameObject.SetActive(false);
        actualPath.gameObject.SetActive(false);

        renderActualPath = false;
    }
}
