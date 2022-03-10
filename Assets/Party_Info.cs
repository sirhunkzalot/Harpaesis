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
    public TMP_Text unitName;
    public TMP_Text unitHealth;
    public TMP_Text weaknesses;
    public TMP_Text resistances;


    public GameObject SelectedBackgroundOne;
    public GameObject SelectedBackgroundTwo;
    public GameObject SelectedBackgroundThree;
    public GameObject SelectedBackgroundFour;
    public GameObject SelectedBackgroundFive;

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
                //Alex
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

                unitName.text = alexData.nickname;
                unitHealth.text = alexData.healthStat.ToString();

                weaknesses.text = alexData.weaknesses[0].ToString() + "\n" + "\n" + alexData.weaknesses[1].ToString();
                resistances.text = alexData.resistances[0].ToString() + "\n" + "\n" + alexData.resistances[1].ToString();
                break;

            case 1:
                //cori
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

                unitName.text = coriData.nickname;
                unitHealth.text = coriData.healthStat.ToString();

                weaknesses.text = coriData.weaknesses[0].ToString() + "\n" + "\n" + coriData.weaknesses[1].ToString();
                resistances.text = coriData.resistances[0].ToString() + "\n" + "\n" + coriData.resistances[1].ToString();
                break;

            case 2:
                //joachim
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

                unitName.text = joachimData.nickname;
                unitHealth.text = joachimData.healthStat.ToString();

                weaknesses.text = joachimData.weaknesses[0].ToString() + "\n" + "\n" + joachimData.weaknesses[1].ToString();
                resistances.text = joachimData.resistances[0].ToString() + "\n" + "\n" + joachimData.resistances[1].ToString();
                break;

            case 3:
                //Regina
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

                unitName.text = reginaData.nickname;
                unitHealth.text = reginaData.healthStat.ToString();

                weaknesses.text = reginaData.weaknesses[0].ToString() + "\n" + "\n" + reginaData.weaknesses[1].ToString();
                resistances.text = reginaData.resistances[0].ToString() + "\n" + "\n" + reginaData.resistances[1].ToString();
                break;

            case 4:
                //Doran
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

                unitName.text = doranData.nickname;
                unitHealth.text = doranData.healthStat.ToString();

                weaknesses.text = doranData.weaknesses[0].ToString() + "\n" + "\n" + doranData.weaknesses[1].ToString();
                resistances.text = doranData.resistances[0].ToString() + "\n" + "\n" + doranData.resistances[1].ToString();
                break;
        }
    }

    public void DisplayAlex()
    {
        UpdatePartyInfo(0);
        SelectedBackgroundOne.SetActive(true);
        SelectedBackgroundTwo.SetActive(false);
        SelectedBackgroundThree.SetActive(false);
        SelectedBackgroundFour.SetActive(false);
        SelectedBackgroundFive.SetActive(false);
    }

    public void DisplayCori()
    {
        UpdatePartyInfo(1);
        SelectedBackgroundOne.SetActive(false);
        SelectedBackgroundTwo.SetActive(true);
        SelectedBackgroundThree.SetActive(false);
        SelectedBackgroundFour.SetActive(false);
        SelectedBackgroundFive.SetActive(false);
    }
    public void DisplayJoachim()
    {
        UpdatePartyInfo(2);
        SelectedBackgroundOne.SetActive(false);
        SelectedBackgroundTwo.SetActive(false);
        SelectedBackgroundThree.SetActive(true);
        SelectedBackgroundFour.SetActive(false);
        SelectedBackgroundFive.SetActive(false);
    }
    public void DisplayRegina()
    {
        UpdatePartyInfo(3);
        SelectedBackgroundOne.SetActive(false);
        SelectedBackgroundTwo.SetActive(false);
        SelectedBackgroundThree.SetActive(false);
        SelectedBackgroundFour.SetActive(true);
        SelectedBackgroundFive.SetActive(false);
    }
    public void DisplayDoran()
    {
        UpdatePartyInfo(4);
        SelectedBackgroundOne.SetActive(false);
        SelectedBackgroundTwo.SetActive(false);
        SelectedBackgroundThree.SetActive(false);
        SelectedBackgroundFour.SetActive(false);
        SelectedBackgroundFive.SetActive(true);
    }
}
