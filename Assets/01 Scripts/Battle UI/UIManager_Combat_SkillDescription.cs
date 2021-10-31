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

    private void Start()
    {
        manager = TurnManager.instance;
    }

    public void SetSkillInfo(int _skillIndex)
    {
        Skill _skill;

        switch (_skillIndex)
        {
            case 0:
                _skill = manager.activeTurn.unitData.primarySkill;
                break;
            case 1:
                _skill = manager.activeTurn.unitData.secondarySkill;
                break;
            case 2:
                _skill = manager.activeTurn.unitData.tertiarySkill;
                break;
            case 3:
                _skill = manager.activeTurn.unitData.specialSkill;
                break;
            default:
                _skill = manager.activeTurn.unitData.specialSkill;
                break;
        }

        skillNameText.text = _skill.skillName;
        skillDescription.text = _skill.skillDescription;
        apCostText.text = _skill.apCost.ToString();
    }
}
