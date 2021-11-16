using System.Collections;
using System.Collections.Generic;
using Harpaesis.Combat;
using UnityEngine.UI;
using UnityEngine;

public class UIManager_Combat_SkillDescription : MonoBehaviour
{
    public Text skillNameText;
    public Text skillDescription;
    public Text apCostText;

    TurnManager manager;

    public void SetSkillInfo(int _skillIndex)
    {
        Skill _skill;
        

        if(manager == null)
        {
            manager = TurnManager.instance;
        }

        FriendlyUnitData _data = (FriendlyUnitData)manager.activeTurn.unitData;

        switch (_skillIndex)
        {
            case 0:
                _skill = _data.basicAttack;
                break;
            case 1:
                _skill = _data.primarySkill;
                break;
            case 2:
                _skill = _data.secondarySkill;
                break;
            case 3:
                _skill = _data.tertiarySkill;
                break;
            default:
                _skill = _data.signatureSkill;
                break;
        }

        skillNameText.text = _skill.skillName;
        skillDescription.text = _skill.skillDescription;
        apCostText.text = _skill.apCost.ToString();
    }
}
