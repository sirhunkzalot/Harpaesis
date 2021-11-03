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

    TargetingTemplate basicAttackTargetingTemplate, primarySkillTargetingTemplate, secondarySkillTargetingTemplate,
        tertiarySkillTargetingTemplate, signatureSkillTargetingTemplate;
    int activeTemplateIndex;

    public enum FriendlyState { Inactive, Active, PreviewMove, Moving, Targeting_Single, Targeting_AOE, Attacking }
    [ReadOnly] public FriendlyState currentState = FriendlyState.Inactive;

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
            case FriendlyState.Targeting_Single:
                TargetingSingleTarget();
                break;
            case FriendlyState.Targeting_AOE:
                TargetingAOE();
                break;
            case FriendlyState.Attacking:
                break;
            default:
                break;
        }
    }

    void SetupTargetingTemplates()
    {
        basicAttackTargetingTemplate = Instantiate(unitData.basicAttack.targetingTemplate, transform).GetComponent<TargetingTemplate>();
        primarySkillTargetingTemplate = Instantiate(unitData.primarySkill.targetingTemplate, transform).GetComponent<TargetingTemplate>();
        secondarySkillTargetingTemplate = Instantiate(unitData.secondarySkill.targetingTemplate, transform).GetComponent<TargetingTemplate>();
        tertiarySkillTargetingTemplate = Instantiate(unitData.tertiarySkill.targetingTemplate, transform).GetComponent<TargetingTemplate>();
        signatureSkillTargetingTemplate = Instantiate(unitData.signatureSkill.targetingTemplate, transform).GetComponent<TargetingTemplate>();

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
        bool _isAOE = false;

        switch (_index)
        {
            case 0:
                basicAttackTargetingTemplate.SetupTargetingTemplate();
                _isAOE = unitData.basicAttack.targetingStyle == TargetingStyle.AOE;
                break;
            case 1:
                primarySkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = unitData.primarySkill.targetingStyle == TargetingStyle.AOE;
                break;
            case 2:
                secondarySkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = unitData.secondarySkill.targetingStyle == TargetingStyle.AOE;
                break;
            case 3:
                tertiarySkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = unitData.tertiarySkill.targetingStyle == TargetingStyle.AOE;
                break;
            case 4:
                signatureSkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = unitData.signatureSkill.targetingStyle == TargetingStyle.AOE;
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }

        activeTemplateIndex = _index;

        if (_isAOE)
        {
            currentState = FriendlyState.Targeting_AOE;
        }
        else
        {
            currentState = FriendlyState.Targeting_Single;
        }
    }

    public void TargetingSingleTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (activeTemplateIndex)
            {
                case 0:
                    if (basicAttackTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, basicAttackTargetingTemplate.currentlySelected.unit);
                    }
                    break;

                case 1:
                    if(primarySkillTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, primarySkillTargetingTemplate.currentlySelected.unit);
                    }
                    break;

                case 2:
                    if (secondarySkillTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, secondarySkillTargetingTemplate.currentlySelected.unit);
                    }
                    break;

                case 3:
                    if (tertiarySkillTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, tertiarySkillTargetingTemplate.currentlySelected.unit);
                    }
                    break;

                case 4:
                    if (signatureSkillTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, signatureSkillTargetingTemplate.currentlySelected.unit);
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

    public void TargetingAOE()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (activeTemplateIndex)
            {
                case 0:
                    if (basicAttackTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, basicAttackTargetingTemplate.allWithTargets);
                    }
                    break;

                case 1:
                    if (primarySkillTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, primarySkillTargetingTemplate.allWithTargets);
                    }
                    break;

                case 2:
                    if (secondarySkillTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, secondarySkillTargetingTemplate.allWithTargets);
                    }
                    break;

                case 3:
                    if (tertiarySkillTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, tertiarySkillTargetingTemplate.allWithTargets);
                    }
                    break;

                case 4:
                    if (signatureSkillTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, signatureSkillTargetingTemplate.allWithTargets);
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

    public void UseSkill(int _skillIndex, List<TargetingTemplateNode> _nodesWithTargets)
    {
        foreach (TargetingTemplateNode node in _nodesWithTargets)
        {
            UseSkill(_skillIndex, node.unit);
        }
    }

    public void UseSkill(int _skillIndex, Unit _target)
    {
        switch (_skillIndex)
        {
            case 0:
                BattleLog.Log($"{unitData.unitName} uses {unitData.basicAttack.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                unitData.basicAttack.UseSkill(this, _target);
                break;
            case 1:
                BattleLog.Log($"{unitData.unitName} uses {unitData.primarySkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                unitData.primarySkill.UseSkill(this, _target);
                break;
            case 2:
                BattleLog.Log($"{unitData.unitName} uses {unitData.secondarySkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                unitData.secondarySkill.UseSkill(this, _target);
                break;
            case 3:
                BattleLog.Log($"{unitData.unitName} uses {unitData.tertiarySkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                unitData.tertiarySkill.UseSkill(this, _target);
                break;
            case 4:
                BattleLog.Log($"{unitData.unitName} uses {unitData.signatureSkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                unitData.signatureSkill.UseSkill(this, _target);
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }
    }
}
