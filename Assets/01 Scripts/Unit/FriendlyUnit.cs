using System.Collections;
using System.Collections.Generic;
using GridAndPathfinding;
using UnityEngine;

public class FriendlyUnit : Unit
{
    public Transform selector;
    public Vector3 lastSelectorPosition;

    public Vector3 nodePosition;
    public Vector3 myLastNodePosition;


    protected override void Tick()
    {
        nodePosition = grid.NodeFromWorldPoint(transform.position).worldPosition;

        if (selector.position != lastSelectorPosition || nodePosition != myLastNodePosition)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, selector.position, PreviewPath, this));
            lastSelectorPosition = selector.position;
            myLastNodePosition = nodePosition;
        }

        if (Input.GetMouseButtonDown(0) && previewPath != null && !hasPath)
        {
            hasPath = true;
            motor.Move(previewPath);
        }
    }
}
