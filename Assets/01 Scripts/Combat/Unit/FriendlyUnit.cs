using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using Harpaesis.GridAndPathfinding;
using Harpaesis.UI.Tooltips;
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

    [HideInInspector] public int primarySkillCooldown, secondarySkillCooldown, tertiarySkillCooldown, signatureSkillCooldown;
    public bool PrimarySkillOnCooldown { get { return primarySkillCooldown > 0; } }
    public bool SecondarySkillOnCooldown { get { return secondarySkillCooldown > 0;} }
    public bool TertiarySkillOnCooldown { get { return tertiarySkillCooldown > 0;} }
    public bool SignatureSkillOnCooldown { get { return signatureSkillCooldown > 0;} }


    RotateTemplates templateParent;
    ActiveUnitIcon activeUnitIcon;
    PlayerInput_Combat input;

    public enum FriendlyState { Inactive, Active, PreviewMove, Moving, Targeting_Single, Targeting_AOE, Targeting_Projectile, Attacking }
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
                if(turnData.ap <= 0)
                {
                    EndTurn();
                    TurnManager.instance.NextTurn();
                }
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
            case FriendlyState.Targeting_Projectile:
                TargetingProjectile();
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

        primarySkillCooldown--;
        secondarySkillCooldown--;
        tertiarySkillCooldown--;
        signatureSkillCooldown--;

        EndTargeting();
    }

    public void MoveAction()
    {
        currentState = FriendlyState.PreviewMove;
        uiCombat.ActivateCancelPopup(true);
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
        if (input.mouseDownRight)
        {
            currentState = FriendlyState.Active;
            PathRenderer.instance.DeactivateAllPaths();
            uiCombat.ActivateCancelPopup(false);
            return;
        }
        if (gridCursor.transform.position != lastSelectorPosition)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, gridCursor.transform.position, DisplayPathPreview, this));
            lastSelectorPosition = gridCursor.transform.position;
        }

        if (input.mouseDownLeft  && !uiCombat.buttonPressedThisFrame && previewPath != null && !hasPath && turnData.ap > 0)
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
        uiCombat.ActivateCancelPopup(false);
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
        uiCombat.ActivateCancelPopup(true);
        TooltipSystem.Hide();

        TargetingStyle targetingStyle = TargetingStyle.SingleTarget;

        switch (_index)
        {
            case 0:
                if (!alternativeWeapon)
                {
                    basicAttackTargetingTemplate.SetupTargetingTemplate();
                    targetingStyle = friendlyUnitData.basicAttack.targetingStyle;
                    canRotateTemplates = basicAttackTargetingTemplate.canRotate;
                }
                else
                {
                    alternativeSkillTargetingTemplate.SetupTargetingTemplate();
                    targetingStyle = friendlyUnitData.alternativeAttack.targetingStyle;
                    canRotateTemplates = alternativeSkillTargetingTemplate.canRotate;
                }
                break;
            case 1:
                primarySkillTargetingTemplate.SetupTargetingTemplate();
                targetingStyle = friendlyUnitData.primarySkill.targetingStyle;
                canRotateTemplates = primarySkillTargetingTemplate.canRotate;
                break;
            case 2:
                secondarySkillTargetingTemplate.SetupTargetingTemplate();
                targetingStyle = friendlyUnitData.secondarySkill.targetingStyle;
                canRotateTemplates = secondarySkillTargetingTemplate.canRotate;
                break;
            case 3:
                tertiarySkillTargetingTemplate.SetupTargetingTemplate();
                targetingStyle = friendlyUnitData.tertiarySkill.targetingStyle;
                canRotateTemplates = tertiarySkillTargetingTemplate.canRotate;
                break;
            case 4:
                signatureSkillTargetingTemplate.SetupTargetingTemplate();
                targetingStyle = friendlyUnitData.signatureSkill.targetingStyle;
                canRotateTemplates = signatureSkillTargetingTemplate.canRotate;
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }

        activeTemplateIndex = _index;


        switch (targetingStyle)
        {
            case TargetingStyle.SingleTarget:
                currentState = FriendlyState.Targeting_Single;
                break;
            case TargetingStyle.AOE:
                currentState = FriendlyState.Targeting_AOE;
                break;
            case TargetingStyle.ProjectileAOE:
                currentState = FriendlyState.Targeting_Projectile;
                templateParent.UnlockTemplate();
                break;
        }
    }

    public void TargetingSingleTarget()
    {
        if (input.mouseDownLeft && !uiCombat.buttonPressedThisFrame)
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

            if (anim != null && anim.HasState(0, Animator.StringToHash("Attack")))
            {
                anim.Play("Attack");
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
        if (input.mouseDownLeft && !uiCombat.buttonPressedThisFrame)
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

            if (anim != null && anim.HasState(0, Animator.StringToHash("Attack")))
            {
                anim.Play("Attack");
            }

            EndTargeting();
        }
        else if (input.mouseDownRight)
        {
            EndTargeting();
        }
    }

    public void TargetingProjectile()
    {
        if (input.mouseDownLeft && !uiCombat.buttonPressedThisFrame)
        {
            switch (activeTemplateIndex)
            {
                case 0:
                    if (basicAttackTargetingTemplate.allActiveNodes.Count > 0)
                    {
                        UseProjectileSkill(activeTemplateIndex, basicAttackTargetingTemplate.allActiveNodes);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 1:
                    if (primarySkillTargetingTemplate.allActiveNodes.Count > 0)
                    {
                        UseProjectileSkill(activeTemplateIndex, primarySkillTargetingTemplate.allActiveNodes);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 2:
                    if (secondarySkillTargetingTemplate.allActiveNodes.Count > 0)
                    {
                        UseProjectileSkill(activeTemplateIndex, secondarySkillTargetingTemplate.allActiveNodes);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 3:
                    if (tertiarySkillTargetingTemplate.allActiveNodes.Count > 0)
                    {
                        UseProjectileSkill(activeTemplateIndex, tertiarySkillTargetingTemplate.allActiveNodes);
                        turnData.hasAttacked = true;
                    }
                    break;

                case 4:
                    if (signatureSkillTargetingTemplate.allActiveNodes.Count > 0)
                    {
                        UseProjectileSkill(activeTemplateIndex, signatureSkillTargetingTemplate.allActiveNodes);
                        turnData.hasAttacked = true;
                    }
                    break;

                default:
                    throw new System.Exception("Error: invalid skill index given.");
            }

            if (anim != null && anim.HasState(0, Animator.StringToHash("Attack")))
            {
                anim.Play("Attack");
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

        uiCombat.ActivateCancelPopup(false);

        templateParent.LockTemplate();
    }

    public void UseSkill(int _skillIndex, List<TargetingTemplateNode> _nodesWithTargets)
    {
        bool _useAP = true;
        foreach (TargetingTemplateNode node in _nodesWithTargets)
        {
            UseSkill(_skillIndex, node.unit, _useAP);
            _useAP = false;
        }
    }

    public void UseSkill(int _skillIndex, Unit _target, bool _useAp = true)
    {
        switch (_skillIndex)
        {
            case 0:
                if (!alternativeWeapon)
                {
                    BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.basicAttack.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                    friendlyUnitData.basicAttack.UseSkill(this, _target);
                    if (_useAp)
                    {
                        turnData.ap -= friendlyUnitData.basicAttack.apCost;
                    }
                }
                else
                {
                    BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.alternativeAttack.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                    friendlyUnitData.alternativeAttack.UseSkill(this, _target);
                    if (_useAp)
                    {
                        turnData.ap -= friendlyUnitData.alternativeAttack.apCost;
                    }
                }
                break;
            case 1:
                BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.primarySkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                friendlyUnitData.primarySkill.UseSkill(this, _target);
                if (_useAp)
                {
                    turnData.ap -= friendlyUnitData.primarySkill.apCost;
                    primarySkillCooldown = friendlyUnitData.primarySkill.skillCooldown;
                }
                break;
            case 2:
                BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.secondarySkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                friendlyUnitData.secondarySkill.UseSkill(this, _target);
                if (_useAp)
                {
                    turnData.ap -= friendlyUnitData.secondarySkill.apCost;
                    secondarySkillCooldown = friendlyUnitData.secondarySkill.skillCooldown;
                }
                break;
            case 3:
                BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.tertiarySkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                friendlyUnitData.tertiarySkill.UseSkill(this, _target);
                if (_useAp)
                {
                    turnData.ap -= friendlyUnitData.tertiarySkill.apCost;
                    tertiarySkillCooldown = friendlyUnitData.tertiarySkill.skillCooldown;
                }
                break;
            case 4:
                BattleLog.Log($"{friendlyUnitData.unitName} uses {friendlyUnitData.signatureSkill.skillName} on {_target.unitData.unitName}", BattleLogType.Combat);
                friendlyUnitData.signatureSkill.UseSkill(this, _target);
                if (_useAp)
                {
                    turnData.ap -= friendlyUnitData.signatureSkill.apCost;
                    signatureSkillCooldown = friendlyUnitData.signatureSkill.skillCooldown;
                }
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }
    }

    // Temp
    public void UseProjectileSkill(int _skillIndex, List<TargetingTemplateNode> _allNodes)
    {
        List<Vector3> _positions = new List<Vector3>();

        for (int i = 0; i < _allNodes.Count; i++)
        {
            _positions.Add(_allNodes[i].transform.position);
        }

        switch (_skillIndex)
        {
            case 0:
                if (!alternativeWeapon)
                {
                    friendlyUnitData.basicAttack.UseProjectileSkill(this, _positions);
                    turnData.ap -= friendlyUnitData.basicAttack.apCost;
                }
                else
                {
                    friendlyUnitData.alternativeAttack.UseProjectileSkill(this, _positions);
                    turnData.ap -= friendlyUnitData.alternativeAttack.apCost;
                }
                break;
            case 1:
                friendlyUnitData.primarySkill.UseProjectileSkill(this, _positions);
                turnData.ap -= friendlyUnitData.primarySkill.apCost;
                primarySkillCooldown = friendlyUnitData.primarySkill.skillCooldown;
                break;
            case 2:
                friendlyUnitData.secondarySkill.UseProjectileSkill(this, _positions);
                turnData.ap -= friendlyUnitData.secondarySkill.apCost;
                secondarySkillCooldown = friendlyUnitData.secondarySkill.skillCooldown;
                break;
            case 3:
                friendlyUnitData.tertiarySkill.UseProjectileSkill(this, _positions);
                turnData.ap -= friendlyUnitData.tertiarySkill.apCost;
                tertiarySkillCooldown = friendlyUnitData.tertiarySkill.skillCooldown;
                break;
            case 4:
                friendlyUnitData.signatureSkill.UseProjectileSkill(this, _positions);
                turnData.ap -= friendlyUnitData.signatureSkill.apCost;
                signatureSkillCooldown = friendlyUnitData.signatureSkill.skillCooldown;
                break;
            default:
                throw new System.Exception("Error: invalid skill index given.");
        }
    }

    public void EndTurn()
    {
        EndTargeting();
        currentState = FriendlyState.Inactive;
        activeUnitIcon.SetChildActive(false);
    }
}
