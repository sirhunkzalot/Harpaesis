using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIManager_PartyDeck : MonoBehaviour
{
    public List<PartyDeckSlot> partyDeckSlots = new List<PartyDeckSlot>();
    public FriendlyUnit[] friendlyUnits;

    bool inited;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        do
        {
            yield return new WaitForEndOfFrame();
            friendlyUnits = TurnManager.instance.friendlyUnits.ToArray();
        } while (friendlyUnits.Length == 0);



        for (int i = 0; i < partyDeckSlots.Count; i++)
        {
            if(i < friendlyUnits.Length && friendlyUnits[i] != null)
            {
                partyDeckSlots[i].nameText.text = friendlyUnits[i].friendlyUnitData.nickname;
                partyDeckSlots[i].image.sprite = friendlyUnits[i].friendlyUnitData.unitIcon;
                partyDeckSlots[i].hpText.text = $"{friendlyUnits[i].currentHP}/{friendlyUnits[i].friendlyUnitData.healthStat}";
            }
            else
            {
                partyDeckSlots[i].parentObject.SetActive(false);
            }
        }

        inited = true;
    }

    public void FixedUpdate()
    {
        if (!inited) return;

        for (int i = 0; i < partyDeckSlots.Count; i++)
        {
            if (partyDeckSlots[i].image.gameObject.activeInHierarchy)
            {
                partyDeckSlots[i].hpText.text = $"{friendlyUnits[i].currentHP}/{friendlyUnits[i].friendlyUnitData.healthStat}";

                if (!friendlyUnits[i].isAlive)
                {
                    partyDeckSlots[i].image.gameObject.SetActive(false);
                }
            }
        }
    }
}

[System.Serializable]
public class PartyDeckSlot
{
    public GameObject parentObject;
    public TextMeshProUGUI nameText;
    public Image image;
    public TextMeshProUGUI hpText;
}
