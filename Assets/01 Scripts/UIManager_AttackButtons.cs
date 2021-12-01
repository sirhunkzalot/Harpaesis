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
            basicAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.basicAttack.apCost;
            primaryAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.primarySkill.apCost;
            secondaryAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.secondarySkill.apCost;
            tertiaryAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.tertiarySkill.apCost;
            signatureAttack.interactable = _friendlyUnit.turnData.hasAttacked == false && _friendlyUnit.turnData.ap >= _friendlyUnit.friendlyUnitData.signatureSkill.apCost;
        }
    }
}
