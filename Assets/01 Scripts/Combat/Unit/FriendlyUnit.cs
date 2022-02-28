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

    TargetingTemplate basicAttackTargetingTemplate, alternativeSkillTargetingTemplate, primarySkillTargetingTemplate, secondarySkillTargetingTemplate,
        tertiarySkillTargetingTemplate, signatureSkillTargetingTemplate;
    int activeTemplateIndex;

    [ReadOnly] public bool alternativeWeapon;
    [ReadOnly] public FriendlyUnitPassive passive;
    [ReadOnly] public bool canRotateTemplates;

    RotateTemplates templateParent;
    ActiveUnitIcon activeUnitIcon;
    PlayerInput_Combat input;

    public enum FriendlyState { Inactive, Active, PreviewMove, Moving, Targeting_Single, Targeting_AOE, Attacking }
    [ReadOnly] public FriendlyState currentState = FriendlyState.Inactive;

    protected override void Init()
    {
        friendlyUnitData = (FriendlyUnitData)unitData;
        gridCursor = GridCursor.instance;
        input = PlayerInput_Combat.instance;

        templateParent = GetComponentInChildren<RotateTemplates>();
        templateParent.Init(this);

        activeUnitIcon = GetComponentInChildren<ActiveUnitIcon>();
        activeUnitIcon.Init();

        SetupTargetingTemplates();

        passive = (FriendlyUnitPassive)unitPassive;
        passive.OnCombatStart();
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
        alternativeSkillTargetingTemplate = Instantiate(friendlyUnitData.alternativeAttack.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();
        primarySkillTargetingTemplate = Instantiate(friendlyUnitData.primarySkill.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();
        secondarySkillTargetingTemplate = Instantiate(friendlyUnitData.secondarySkill.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();
        tertiarySkillTargetingTemplate = Instantiate(friendlyUnitData.tertiarySkill.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();
        signatureSkillTargetingTemplate = Instantiate(friendlyUnitData.signatureSkill.targetingTemplate, templateParent.transform).GetComponent<TargetingTemplate>();

        templateParent.InitTemplate(basicAttackTargetingTemplate);
        templateParent.InitTemplate(alternativeSkillTargetingTemplate);
        templateParent.InitTemplate(primarySkillTargetingTemplate);
        templateParent.InitTemplate(secondarySkillTargetingTemplate);
        templateParent.InitTemplate(tertiarySkillTargetingTemplate);
        templateParent.InitTemplate(signatureSkillTargetingTemplate);

        basicAttackTargetingTemplate.Init(friendlyUnitData.basicAttack);
        alternativeSkillTargetingTemplate.Init(friendlyUnitData.alternativeAttack);
        primarySkillTargetingTemplate.Init(friendlyUnitData.primarySkill);
        secondarySkillTargetingTemplate.Init(friendlyUnitData.secondarySkill);
        tertiarySkillTargetingTemplate.Init(friendlyUnitData.tertiarySkill);
        signatureSkillTargetingTemplate.Init(friendlyUnitData.signatureSkill);

        basicAttackTargetingTemplate.Disable();
        alternativeSkillTargetingTemplate.Disable();
        primarySkillTargetingTemplate.Disable();
        secondarySkillTargetingTemplate.Disable();
        tertiarySkillTargetingTemplate.Disable();
        signatureSkillTargetingTemplate.Disable();
    }

    public override void StartTurn()
    {
        uiCombat.ShowPlayerUI(true);

        gridCam.JumpToPosition(transform.position);
        gridCam.followUnit = null;

        activeUnitIcon.SetChildActive(true);

        currentState = FriendlyState.Active;
    }

    public void MoveAction()
    {
        currentState = FriendlyState.PreviewMove;
    }

    public void DefendAction()
    {
        ApplyEffect(new StatusEffect_Defend(this, this, 1, 1));
    }

    public void SwapWeapon()
    {
        alternativeWeapon = !alternativeWeapon;

        passive.OnChangeWeapon();

        if (basicAttackTargetingTemplate.isActive || alternativeSkillTargetingTemplate.isActive)
        {
            EndTargeting();
            BeginTargeting(0);
        }
    }

    public void PreviewMove()
    {
        if (gridCursor.transform.position != lastSelectorPosition)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, gridCursor.transform.position, DisplayPathPreview, this));
            lastSelectorPosition = gridCursor.transform.position;
        }

        if (input.mouseDownLeft && previewPath != null && !hasPath && turnData.ap > 0)
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
        uiCombat.ShowPlayerUI(false);
        gridCam.followUnit = this;
        PathRenderer.instance.SwapToActualPath();
    }

    public void Moving()
    {
        if (!motor.isMoving)
        {
            currentState = FriendlyState.Active;
            uiCombat.ShowPlayerUI(true);
            gridCam.followUnit = null;
            return;
        }
    }

    public void BeginTargeting(int _index)
    {
        bool _isAOE = false;

        switch (_index)
        {
            case 0:
                if (!alternativeWeapon)
                {
                    basicAttackTargetingTemplate.SetupTargetingTemplate();
                    _isAOE = friendlyUnitData.basicAttack.targetingStyle == TargetingStyle.AOE;
                    canRotateTemplates = basicAttackTargetingTemplate.canRotate;
                }
                else
                {
                    alternativeSkillTargetingTemplate.SetupTargetingTemplate();
                    _isAOE = friendlyUnitData.alternativeAttack.targetingStyle == TargetingStyle.AOE;
                    canRotateTemplates = alternativeSkillTargetingTemplate.canRotate;
                }
                break;
            case 1:
                primarySkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = friendlyUnitData.primarySkill.targetingStyle == TargetingStyle.AOE;
                canRotateTemplates = primarySkillTargetingTemplate.canRotate;
                break;
            case 2:
                secondarySkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = friendlyUnitData.secondarySkill.targetingStyle == TargetingStyle.AOE;
                canRotateTemplates = secondarySkillTargetingTemplate.canRotate;
                break;
            case 3:
                tertiarySkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = friendlyUnitData.tertiarySkill.targetingStyle == TargetingStyle.AOE;
                canRotateTemplates = tertiarySkillTargetingTemplate.canRotate;
                break;
            case 4:
                signatureSkillTargetingTemplate.SetupTargetingTemplate();
                _isAOE = friendlyUnitData.signatureSkill.targetingStyle == TargetingStyle.AOE;
                canRotateTemplates = signatureSkillTargetingTemplate.canRotate;
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
        if (input.mouseDownLeft)
        {
            switch (activeTemplateIndex)
            {
                case 0:
                    if (!alternativeWeapon && basicAttackTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, basicAttackTargetingTemplate.currentlySelected.unit);
                        turnData.hasAttacked = true;
                    }
                    else if(alternativeWeapon && alternativeSkillTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, alternativeSkillTargetingTemplate.currentlySelected.unit);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 1:
                    if(primarySkillTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, primarySkillTargetingTemplate.currentlySelected.unit);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 2:
                    if (secondarySkillTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, secondarySkillTargetingTemplate.currentlySelected.unit);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 3:
                    if (tertiarySkillTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, tertiarySkillTargetingTemplate.currentlySelected.unit);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 4:
                    if (signatureSkillTargetingTemplate.currentlySelected?.unit != null)
                    {
                        UseSkill(activeTemplateIndex, signatureSkillTargetingTemplate.currentlySelected.unit);
                        turnData.hasAttacked = true;
                    }
                    break;

                default:
                    throw new System.Exception("Error: invalid skill index given.");
            }

            EndTargeting();
        }
        else if (input.mouseDownRight)
        {
            EndTargeting();
        }
    }

    public void TargetingAOE()
    {
        if (input.mouseDownLeft)
        {
            switch (activeTemplateIndex)
            {
                case 0:
                    if (basicAttackTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, basicAttackTargetingTemplate.allWithTargets);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 1:
                    if (primarySkillTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, primarySkillTargetingTemplate.allWithTargets);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 2:
                    if (secondarySkillTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, secondarySkillTargetingTemplate.allWithTargets);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 3:
                    if (tertiarySkillTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, tertiarySkillTargetingTemplate.allWithTargets);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 4:
                    if (signatureSkillTargetingTemplate.allWithTargets.Count > 0)
                    {
                        UseSkill(activeTemplateIndex, signatureSkillTargetingTemplate.allWithTargets);
                        turnData.hasAttacked = true;
                    }
                    break;

                default:
                    throw new System.Exception("Error: invalid skill index given.");
            }

            EndTargeting();
        }
        else if (input.mouseDownRight)
        {
            EndTargeting();
        }
    }

    void EndTargeting()
    {
        currentState = FriendlyState.Active;

        basicAttackTargetingTemplate.Disable();
        alternativeSkillTargetingTemplate.Disable();
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
                if (!alternativeWeapon)
                {
                    BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.basicAttack.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                    friendlyUnitData.basicAttack.UseSkill(this, _target);
                }
                else
                {
                    BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.alternativeAttack.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                    friendlyUnitData.alternativeAttack.UseSkill(this, _target);
                }
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

    public void EndTurn()
    {
        activeUnitIcon.SetChildActive(false);
    }
}
