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
    [HideInInspector] public FriendlyUnitData friendlyUnitData;

    GridCursor gridCursor;
    Vector3 lastSelectorPosition;

    Waypoint[] previewPath;

    TargetingTemplate basicAttackTargetingTemplate, primarySkillTargetingTemplate, secondarySkillTargetingTemplate,
        tertiarySkillTargetingTemplate, signatureSkillTargetingTemplate;
    int activeTemplateIndex;

    RotateTemplates templateParent; 

    public enum FriendlyState { Inactive, Active, PreviewMove, Moving, Targeting_Single, Targeting_AOE, Attacking }
    [ReadOnly] public FriendlyState currentState = FriendlyState.Inactive;

    protected override void Init()
    {
        friendlyUnitData = (FriendlyUnitData)unitData;

        gridCursor = GridCursor.instance;
        templateParent = GetComponentInChildren<RotateTemplates>();
        templateParent.Init(this);

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
        basicAttackTargetingTemplate = Instantiate(friendlyUnitData.basicAttack.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();
        primarySkillTargetingTemplate = Instantiate(friendlyUnitData.primarySkill.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();
        secondarySkillTargetingTemplate = Instantiate(friendlyUnitData.secondarySkill.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();
        tertiarySkillTargetingTemplate = Instantiate(friendlyUnitData.tertiarySkill.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();
        signatureSkillTargetingTemplate = Instantiate(friendlyUnitData.signatureSkill.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();

        templateParent.InitTemplate(basicAttackTargetingTemplate);
        templateParent.InitTemplate(primarySkillTargetingTemplate);
        templateParent.InitTemplate(secondarySkillTargetingTemplate);
        templateParent.InitTemplate(tertiarySkillTargetingTemplate);
        templateParent.InitTemplate(signatureSkillTargetingTemplate);

        basicAttackTargetingTemplate.Init(friendlyUnitData.basicAttack);
        primarySkillTargetingTemplate.Init(friendlyUnitData.primarySkill);
        secondarySkillTargetingTemplate.Init(friendlyUnitData.secondarySkill);
        tertiarySkillTargetingTemplate.Init(friendlyUnitData.tertiarySkill);
        signatureSkillTargetingTemplate.Init(friendlyUnitData.signatureSkill);

        basicAttackTargetingTemplate.Disable();
        primarySkillTargetingTemplate.Disable();
        secondarySkillTargetingTemplate.Disable();
        tertiarySkillTargetingTemplate.Disable();
        signatureSkillTargetingTemplate.Disable();
    }

    public override void StartTurn()
    {
        base.StartTurn();

        if(uiCombat == null) { uiCombat = UIManager_Combat.instance; }

        uiCombat.IsPlayerTurn(true);
        gridCam.followUnit = null;
        currentState = FriendlyState.Active;
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
                _isAOE = friendlyUnitData.basicAttack.targetingStyle == TargetingStyle.AOE;
                break;
            case 1:
                primarySkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = friendlyUnitData.primarySkill.targetingStyle == TargetingStyle.AOE;
                break;
            case 2:
                secondarySkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = friendlyUnitData.secondarySkill.targetingStyle == TargetingStyle.AOE;
                break;
            case 3:
                tertiarySkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = friendlyUnitData.tertiarySkill.targetingStyle == TargetingStyle.AOE;
                break;
            case 4:
                signatureSkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = friendlyUnitData.signatureSkill.targetingStyle == TargetingStyle.AOE;
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
                BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.basicAttack.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                friendlyUnitData.basicAttack.UseSkill(this, _target);
                break;
            case 1:
                BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.primarySkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                friendlyUnitData.primarySkill.UseSkill(this, _target);
                break;
            case 2:
                BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.secondarySkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                friendlyUnitData.secondarySkill.UseSkill(this, _target);
                break;
            case 3:
                BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.tertiarySkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                friendlyUnitData.tertiarySkill.UseSkill(this, _target);
                break;
            case 4:
                BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.signatureSkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                friendlyUnitData.signatureSkill.UseSkill(this, _target);
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }
    }
}
