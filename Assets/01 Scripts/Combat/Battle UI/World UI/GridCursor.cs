using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class GridCursor handles all logic concerning the 3D grid cursor*/
public class GridCursor : MonoBehaviour
{
    GridManager grid;
    public LayerMask layermask;
    Camera cam;

    public Node currentNode;

    PlayerInput_Combat input;

    #region Singleton
    public static GridCursor instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    #endregion

    private void Start()
    {
        grid = GridManager.instance;
        cam = Camera.main;
        input = PlayerInput_Combat.instance;
    }

    void FixedUpdate()
    {
        MoveCursor();
    }


    /* MoveCursor moves the 3D Grid cursor along the surface of the grid according 
     * to the mouse position */
    private void MoveCursor()
    {
        Ray _ray = cam.ScreenPointToRay(input.mousePosition);
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, 100, layermask))
        {
            transform.position = grid.NodePositionFromWorldPoint(_hit.point, false);

            currentNode = grid.NodeFromWorldPoint(transform.position);
        }
    }
}
