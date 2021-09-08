using System.Collections;
using System.Collections.Generic;
using GridAndPathfinding;
using UnityEngine;

public class MatchMousePosition : MonoBehaviour
{
    GridManager grid;
    public LayerMask layermask;
    Camera cam;
    bool going;

    private void Awake()
    {
        grid = FindObjectOfType<GridManager>();
        cam = Camera.main;
        going = true;
    }

    void FixedUpdate()
    {
        Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, 100))
        {
            Debug.DrawLine(_ray.origin, _hit.point);

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
        if (going)
        {
            /*Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit, 100, layermask))
            {
                Gizmos.DrawSphere(_hit.point, .15f);
            }*/

            //Gizmos.DrawSphere(cam.ScreenToWorldPoint(Input.mousePosition), .15f);
        }
    }
}
