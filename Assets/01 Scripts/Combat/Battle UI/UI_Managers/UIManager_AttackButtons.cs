using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine.UI;
using UnityEngine;

public class UIManager_AttackButtons : MonoBehaviour
{
    public Button basicAttack, primaryAttack, secondaryAttack, tertiaryAttack, signatureAttack;

    TurnManager turnManager;

    private void Start()
    {
        turnManager = TurnManager.instance;
    }

    void Update()
    {
        if (gameObject.activeInHierarchy && turnManager.activeTurn.unit.GetType() == typeof(FriendlyUnit))
        {
            FriendlyUnit _friendlyUnit = (FriendlyUnit)turnManager.activeTurn.unit;
            basicAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.basicAttack.apCost && _friendlyUnit.friendlyUnitData.basicAttack.implemented;
            primaryAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.primarySkill.apCost && _friendlyUnit.friendlyUnitData.primarySkill.implemented;
            secondaryAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.secondarySkill.apCost && _friendlyUnit.friendlyUnitData.secondarySkill.implemented;
            tertiaryAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.tertiarySkill.apCost && _friendlyUnit.friendlyUnitData.tertiarySkill.implemented;
            signatureAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.signatureSkill.apCost && _friendlyUnit.friendlyUnitData.signatureSkill.implemented;
        }
    }
}
