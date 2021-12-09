using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class Unit_UI : MonoBehaviour
{
    public Unit_UI_HealthBar healthBar;
    public Unit_UI_EffectsManager effectsManager;
    public GameObject damageTextParent;
    TextMeshProUGUI damageText;

    private void Start()
    {
        damageText = damageTextParent.GetComponentInChildren<TextMeshProUGUI>();
        damageTextParent.SetActive(false);
    }
    public void DisplayDamageText(int _damageAmount)
    {
        StartCoroutine(DisplayDamage(_damageAmount));
    }

    IEnumerator DisplayDamage(int _damageAmount)
    {
        damageText.text = (_damageAmount > 0) ? $"-{_damageAmount}" : (_damageAmount == 0) ? "0" : $"+{_damageAmount}";
        damageTextParent.SetActive(true);

        yield return new WaitForSeconds(2f);

        damageTextParent.SetActive(false);
        damageText.text = "";
    }
}
