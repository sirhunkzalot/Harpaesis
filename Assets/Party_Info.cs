using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Harpaesis.Combat;


public class Party_Info : MonoBehaviour
{
    [SerializeField] FriendlyUnitData alexData;
    [SerializeField] FriendlyUnitData coriData;
    [SerializeField] FriendlyUnitData joachimData;
    [SerializeField] FriendlyUnitData reginaData;
    [SerializeField] FriendlyUnitData doranData;

    public Image basicAttackImage;
    public Image alternativeAttackImage;
    public Image primarySkillImage;
    public Image secondarySkillImage;
    public Image tertiarySkillImage;
    public Image signatureSkillImage;

    public TMP_Text basicAttackDescription;
    public TMP_Text alternativeAttackDescription;
    public TMP_Text primarySkillDescription;
    public TMP_Text secondarySkillDescription;
    public TMP_Text tertiarySkillDescription;
    public TMP_Text signatureSkillDescription;


    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            UpdatePartyInfo(i);
        }
        
    }
    public void UpdatePartyInfo(int _index)
    {
        Debug.Log("I am in your loops");
        switch (_index)
        {
            case 0:
                Debug.Log("Alex");
                basicAttackImage.sprite = alexData.basicAttack.skillSprite;
                basicAttackDescription.text = alexData.basicAttack.skillDescription;

                alternativeAttackImage.sprite = alexData.alternativeAttack.skillSprite;
                alternativeAttackDescription.text = alexData.alternativeAttack.skillDescription;

                primarySkillImage.sprite = alexData.primarySkill.skillSprite;
                primarySkillDescription.text = alexData.primarySkill.skillDescription;

                secondarySkillImage.sprite = alexData.secondarySkill.skillSprite;
                secondarySkillDescription.text = alexData.secondarySkill.skillDescription;

                tertiarySkillImage.sprite = alexData.tertiarySkill.skillSprite;
                tertiarySkillDescription.text = alexData.tertiarySkill.skillDescription;

                signatureSkillImage.sprite = alexData.signatureSkill.skillSprite;
                signatureSkillDescription.text = alexData.signatureSkill.skillDescription;
                break;

            case 1:
                Debug.Log("Cori");
                basicAttackImage.sprite = coriData.basicAttack.skillSprite;
                basicAttackDescription.text = coriData.basicAttack.skillDescription;

                alternativeAttackImage.sprite = coriData.alternativeAttack.skillSprite;
                alternativeAttackDescription.text = coriData.alternativeAttack.skillDescription;

                primarySkillImage.sprite = coriData.primarySkill.skillSprite;
                primarySkillDescription.text = coriData.primarySkill.skillDescription;

                secondarySkillImage.sprite = coriData.secondarySkill.skillSprite;
                secondarySkillDescription.text = coriData.secondarySkill.skillDescription;

                tertiarySkillImage.sprite = coriData.tertiarySkill.skillSprite;
                tertiarySkillDescription.text = coriData.tertiarySkill.skillDescription;

                signatureSkillImage.sprite = coriData.signatureSkill.skillSprite;
                signatureSkillDescription.text = coriData.signatureSkill.skillDescription;
                break;

            case 2:
                Debug.Log("Joachim");
                basicAttackImage.sprite = joachimData.basicAttack.skillSprite;
                basicAttackDescription.text = joachimData.basicAttack.skillDescription;

                alternativeAttackImage.sprite = joachimData.alternativeAttack.skillSprite;
                alternativeAttackDescription.text = joachimData.alternativeAttack.skillDescription;

                primarySkillImage.sprite = joachimData.primarySkill.skillSprite;
                primarySkillDescription.text = joachimData.primarySkill.skillDescription;

                secondarySkillImage.sprite = joachimData.secondarySkill.skillSprite;
                secondarySkillDescription.text = joachimData.secondarySkill.skillDescription;

                tertiarySkillImage.sprite = joachimData.tertiarySkill.skillSprite;
                tertiarySkillDescription.text = joachimData.tertiarySkill.skillDescription;

                signatureSkillImage.sprite = joachimData.signatureSkill.skillSprite;
                signatureSkillDescription.text = joachimData.signatureSkill.skillDescription;
                break;

            case 3:
                Debug.Log("Regina");
                basicAttackImage.sprite = reginaData.basicAttack.skillSprite;
                basicAttackDescription.text = reginaData.basicAttack.skillDescription;

                alternativeAttackImage.sprite = reginaData.alternativeAttack.skillSprite;
                alternativeAttackDescription.text = reginaData.alternativeAttack.skillDescription;

                primarySkillImage.sprite = reginaData.primarySkill.skillSprite;
                primarySkillDescription.text = reginaData.primarySkill.skillDescription;

                secondarySkillImage.sprite = reginaData.secondarySkill.skillSprite;
                secondarySkillDescription.text = reginaData.secondarySkill.skillDescription;

                tertiarySkillImage.sprite = reginaData.tertiarySkill.skillSprite;
                tertiarySkillDescription.text = reginaData.tertiarySkill.skillDescription;

                signatureSkillImage.sprite = reginaData.signatureSkill.skillSprite;
                signatureSkillDescription.text = reginaData.signatureSkill.skillDescription;
                break;

            case 4:
                Debug.Log("Doran");
                basicAttackImage.sprite = doranData.basicAttack.skillSprite;
                basicAttackDescription.text = doranData.basicAttack.skillDescription;

                alternativeAttackImage.sprite = doranData.alternativeAttack.skillSprite;
                alternativeAttackDescription.text = doranData.alternativeAttack.skillDescription;

                primarySkillImage.sprite = doranData.primarySkill.skillSprite;
                primarySkillDescription.text = doranData.primarySkill.skillDescription;

                secondarySkillImage.sprite = doranData.secondarySkill.skillSprite;
                secondarySkillDescription.text = doranData.secondarySkill.skillDescription;

                tertiarySkillImage.sprite = doranData.tertiarySkill.skillSprite;
                tertiarySkillDescription.text = doranData.tertiarySkill.skillDescription;

                signatureSkillImage.sprite = doranData.signatureSkill.skillSprite;
                signatureSkillDescription.text = doranData.signatureSkill.skillDescription;
                break;
        }
    }

    public void DisplayAlex()
    {
        UpdatePartyInfo(0);
    }

}
