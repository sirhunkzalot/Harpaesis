using GridAndPathfinding;
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

    public Vector3[] path;
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
        if(unit != null)
        {
            if(!renderActualPath && reachablePath.positionCount > 0)
            {
                reachablePath.SetPosition(0, unit.transform.position + pathOffset);
            }
            else if(renderActualPath && actualPath.positionCount > 0)
            {
                actualPath.SetPosition(0, unit.transform.position + pathOffset);

                if(Vector3.Distance(actualPath.GetPosition(0), actualPath.GetPosition(1)) < .15f)
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

    public void SwapToActualPath()
    {
        actualPath.gameObject.SetActive(true);
        actualPath.positionCount = reachablePath.positionCount;

        Vector3[] _actualPositions = new Vector3[reachablePath.positionCount];
        reachablePath.GetPositions(_actualPositions);
        actualPath.SetPositions(_actualPositions);

        reachablePath.gameObject.SetActive(false);
        unreachablePath.gameObject.SetActive(false);

        renderActualPath = true;
    }

    void DeactivateAllPaths()
    {
        reachablePath.gameObject.SetActive(false);
        unreachablePath.gameObject.SetActive(false);
        actualPath.gameObject.SetActive(false);

        renderActualPath = false;
    }
}
