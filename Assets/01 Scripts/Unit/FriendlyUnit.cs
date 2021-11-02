using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using Harpaesis.GridAndPathfinding;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class FriendlyUnit handles the logic that pertains to every friendly unit 
 * such as ability to search for a player's input */
public class FriendlyUnit : Unit
{
    GridCursor gridCursor;
    Vector3 lastSelectorPosition;

    Waypoint[] previewPath;

    SkillTargetingTemplate basicAttackTargetingTemplate, primarySkillTargetingTemplate, secondarySkillTargetingTemplate,
        tertiarySkillTargetingTemplate, signatureSkillTargetingTemplate;
    int activeTemplateIndex;

    public enum FriendlyState { Inactive, Active, PreviewMove, Moving, Targeting, Attacking }
    public FriendlyState currentState = FriendlyState.Inactive;

    protected override void Init()
    {
        gridCursor = GridCursor.instance;

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
        basicAttackTargetingTemplate = Instantiate(unitData.basicAttack.targetingTemplate, transform).GetComponent<SkillTargetingTemplate>();
        primarySkillTargetingTemplate = Instantiate(unitData.primarySkill.targetingTemplate, transform).GetComponent<SkillTargetingTemplate>();
        secondarySkillTargetingTemplate = Instantiate(unitData.secondarySkill.targetingTemplate, transform).GetComponent<SkillTargetingTemplate>();
        tertiarySkillTargetingTemplate = Instantiate(unitData.tertiarySkill.targetingTemplate, transform).GetComponent<SkillTargetingTemplate>();
        signatureSkillTargetingTemplate = Instantiate(unitData.signatureSkill.targetingTemplate, transform).GetComponent<SkillTargetingTemplate>();

        basicAttackTargetingTemplate.Init(unitData.basicAttack);
        primarySkillTargetingTemplate.Init(unitData.primarySkill);
        secondarySkillTargetingTemplate.Init(unitData.secondarySkill);
        tertiarySkillTargetingTemplate.Init(unitData.tertiarySkill);
        signatureSkillTargetingTemplate.Init(unitData.signatureSkill);

        basicAttackTargetingTemplate.Disable();
        primarySkillTargetingTemplate.Disable();
        secondarySkillTargetingTemplate.Disable();
        tertiarySkillTargetingTemplate.Disable();
        signatureSkillTargetingTemplate.Disable();
    }

    public void MoveAction()
    {
        currentState = FriendlyState.PreviewMove;
    }

    public void PreviewMove()
    {
        if (gridCursor.transform.position != lastSelectorPosition)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, gridCursor.transform.position, DisplayPathPreview, this));
            lastSelectorPosition = gridCursor.transform.position;
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
                basicAttackTargetingTemplate.SetupTargetingTemplate();
                break;
            case 1:
                primarySkillTargetingTemplate.SetupTargetingTemplate();
                break;
            case 2:
                secondarySkillTargetingTemplate.SetupTargetingTemplate();
                break;
            case 3:
                tertiarySkillTargetingTemplate.SetupTargetingTemplate();
                break;
            case 4:
                signatureSkillTargetingTemplate.SetupTargetingTemplate();
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }

        activeTemplateIndex = _index;

        currentState = FriendlyState.Targeting;
    }

    public void Targeting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (activeTemplateIndex)
            {
                case 0:
                    if (basicAttackTargetingTemplate.currentlySelected.enemyUnit != null)
                    {
                        UseSkill(activeTemplateIndex, basicAttackTargetingTemplate.currentlySelected.enemyUnit);
                    }
                    break;
                case 1:
                    if (primarySkillTargetingTemplate.currentlySelected.allyUnit != null && unitData.primarySkill.validTargets == TargetMask.Ally)
                    {
                        UseSkill(activeTemplateIndex, primarySkillTargetingTemplate.currentlySelected.allyUnit);
                    }
                    else if (primarySkillTargetingTemplate.currentlySelected.enemyUnit != null && unitData.primarySkill.validTargets == TargetMask.Enemy)
                    {
                        UseSkill(activeTemplateIndex, primarySkillTargetingTemplate.currentlySelected.enemyUnit);
                    }
                    break;
                case 2:
                    if (secondarySkillTargetingTemplate.currentlySelected.allyUnit != null && unitData.secondarySkill.validTargets == TargetMask.Ally)
                    {
                        UseSkill(activeTemplateIndex, secondarySkillTargetingTemplate.currentlySelected.allyUnit);
                    }
                    else if (secondarySkillTargetingTemplate.currentlySelected.enemyUnit != null && unitData.secondarySkill.validTargets == TargetMask.Enemy)
                    {
                        UseSkill(activeTemplateIndex, secondarySkillTargetingTemplate.currentlySelected.enemyUnit);
                    }
                    break;
                case 3:
                    if (tertiarySkillTargetingTemplate.currentlySelected.allyUnit != null && unitData.tertiarySkill.validTargets == TargetMask.Ally)
                    {
                        UseSkill(activeTemplateIndex, tertiarySkillTargetingTemplate.currentlySelected.allyUnit);
                    }
                    else if (tertiarySkillTargetingTemplate.currentlySelected.enemyUnit != null && unitData.tertiarySkill.validTargets == TargetMask.Enemy)
                    {
                        UseSkill(activeTemplateIndex, tertiarySkillTargetingTemplate.currentlySelected.enemyUnit);
                    }
                    break;
                case 4:
                    if (signatureSkillTargetingTemplate.currentlySelected.allyUnit != null && unitData.signatureSkill.validTargets == TargetMask.Ally)
                    {
                        UseSkill(activeTemplateIndex, signatureSkillTargetingTemplate.currentlySelected.allyUnit);
                    }
                    else if (signatureSkillTargetingTemplate.currentlySelected.enemyUnit != null && unitData.signatureSkill.validTargets == TargetMask.Enemy)
                    {
                        UseSkill(activeTemplateIndex, signatureSkillTargetingTemplate.currentlySelected.enemyUnit);
                    }
                    break;
                default:
                    throw new System.Exception("Error: invalid skill index given.");
            }

            EndTargeting();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            EndTargeting();
        }
    }

    void EndTargeting()
    {
        currentState = FriendlyState.Active;

        basicAttackTargetingTemplate.Disable();
        primarySkillTargetingTemplate.Disable();
        secondarySkillTargetingTemplate.Disable();
        tertiarySkillTargetingTemplate.Disable();
        signatureSkillTargetingTemplate.Disable();
    }

    public void UseSkill(int _skillIndex, Unit _target)
    {
        switch (_skillIndex)
        {
            case 0:
                unitData.basicAttack.UseSkill(this, _target);
                break;
            case 1:
                unitData.primarySkill.UseSkill(this, _target);
                break;
            case 2:
                unitData.secondarySkill.UseSkill(this, _target);
                break;
            case 3:
                unitData.tertiarySkill.UseSkill(this, _target);
                break;
            case 4:
                unitData.signatureSkill.UseSkill(this, _target);
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }
    }
}
