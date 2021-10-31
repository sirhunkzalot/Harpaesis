using System.Collections;
using System.Collections.Generic;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class FriendlyUnit handles the logic that pertains to every friendly unit 
 * such as ability to search for a player's input */
public class FriendlyUnit : Unit
{
    Transform selector;
    Vector3 lastSelectorPosition;

    Waypoint[] previewPath;

    SkillTargetingTemplate primarySkillTargetingTemplate, secondarySkillTargetingTemplate, tertiarySkillTargetingTemplate, specialSkillTargetingTemplate;

    public enum FriendlyState { Inactive, Active, PreviewMove, Moving, Targeting, Attacking }
    public FriendlyState currentState = FriendlyState.Inactive;

    protected override void Init()
    {
        selector = GridCursor.instance.transform;

        SetupTargetingTemplates();
    }

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
                Moving();
                break;
            case FriendlyState.Targeting:
                Targeting();
                break;
            case FriendlyState.Attacking:
                break;
            default:
                break;
        }
    }

    void SetupTargetingTemplates()
    {
        primarySkillTargetingTemplate = Instantiate(unitData.primarySkill.targetingTemplate, transform).GetComponent<SkillTargetingTemplate>();
        secondarySkillTargetingTemplate = Instantiate(unitData.secondarySkill.targetingTemplate, transform).GetComponent<SkillTargetingTemplate>();
        tertiarySkillTargetingTemplate = Instantiate(unitData.tertiarySkill.targetingTemplate, transform).GetComponent<SkillTargetingTemplate>();
        specialSkillTargetingTemplate = Instantiate(unitData.specialSkill.targetingTemplate, transform).GetComponent<SkillTargetingTemplate>();

        primarySkillTargetingTemplate.Disable();
        secondarySkillTargetingTemplate.Disable();
        tertiarySkillTargetingTemplate.Disable();
        specialSkillTargetingTemplate.Disable();
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

        if (Input.GetMouseButtonDown(0) && previewPath != null && !hasPath && TurnManager.instance.activeTurn.ap > 0)
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

    public void BeginTargeting(int _index)
    {
        switch (_index)
        {
            case 0:
                primarySkillTargetingTemplate.SetupTargetingTemplate();
                break;
            case 1:
                secondarySkillTargetingTemplate.SetupTargetingTemplate();
                break;
            case 3:
                tertiarySkillTargetingTemplate.SetupTargetingTemplate();
                break;
            case 4:
                specialSkillTargetingTemplate.SetupTargetingTemplate();
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }

        currentState = FriendlyState.Targeting;
    }

    public void Targeting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        }
    }

    public void UseSkill(int _skillIndex)
    {
        switch (_skillIndex)
        {
            case 0:
                unitData.primarySkill.UseSkill(this, this);
                break;
            case 1:
                unitData.secondarySkill.UseSkill(this, this);
                break;
            case 3:
                unitData.tertiarySkill.UseSkill(this, this);
                break;
            case 4:
                unitData.specialSkill.UseSkill(this, this);
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }
    }
}
