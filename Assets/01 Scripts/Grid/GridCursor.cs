using System.Collections;
using System.Collections.Generic;
using GridAndPathfinding;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class GridCursor handles all logic concerning the 3D grid cursor*/
public class GridCursor : MonoBehaviour
{
    public GridManager grid;
    public LayerMask layermask;
    Camera cam;
    bool sceneStarted;

    private void Awake()
    {
        cam = Camera.main;
        sceneStarted = true;
    }

    void FixedUpdate()
    {
        MoveCursor();
    }

    /**
     * MoveCursor moves the 3D Grid cursor along the surface of the grid according
     * to the mouse position*/
    private void MoveCursor()
    {
        Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, 100, layermask))
        {

            /* bool correctNode = false;
             Node _node = grid.NodeFromWorldPoint(_hit.point);

             /*while (correctNode == false)
             {


                 correctNode = _node.bounds.Contains(_hit.point);
             }*/

            transform.position = grid.NodePositionFromWorldPoint(_hit.point);
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (sceneStarted)
        {
            /*Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit, 100, layermask))
            {
                Gizmos.DrawSphere(_hit.point, .15f);
            }*/

            //Gizmos.DrawSphere(cam.ScreenToWorldPoint(Input.mousePosition), .15f);
        }
#endif
    }
}
