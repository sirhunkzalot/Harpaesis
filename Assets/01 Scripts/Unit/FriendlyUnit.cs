using System.Collections;
using System.Collections.Generic;
using GridAndPathfinding;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class FriendlyUnit handles the logic that pertains to every friendly unit 
 * such as ability to search for a player's input */
public class FriendlyUnit : Unit
{
    public Transform selector;
    public Vector3 lastSelectorPosition;

    public enum FriendlyState { Inactive, Active, PreviewMove, Moving }
    public FriendlyState currentState = FriendlyState.Inactive;

    protected override void Tick()
    {
        switch (currentState)
        {
            case FriendlyState.Active:
                break;
            case FriendlyState.PreviewMove:
                PreviewMove();
                break;
            case FriendlyState.Moving:
                break;
            default:
                break;
        }
    }

    public void MoveAction()
    {
        currentState = FriendlyState.PreviewMove;
    }

    public void PreviewMove()
    {
        if (selector.position != lastSelectorPosition)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, selector.position, DisplayPathPreview, this));
            lastSelectorPosition = selector.position;
        }

        if (Input.GetMouseButtonDown(0) && previewPath != null && !hasPath)
        {
            StartMove();
        }
    }

    public void DisplayPathPreview(PathResult _result)
    {
        if (_result.success)
        {
            previewPath = _result.path;
            PathRenderer.instance.PathPreview(_result);
        }
    }

    void StartMove()
    {
        currentState = FriendlyState.Moving;
        hasPath = true;
        motor.Move(previewPath);
        PathRenderer.instance.SwapToActualPath();
    }

    public void Moving()
    {

    }
}
