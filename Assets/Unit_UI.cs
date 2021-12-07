using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class Unit_UI : MonoBehaviour
{
    public TextMeshProUGUI damageText;

    private void Start()
    {
        damageText.gameObject.SetActive(false);
    }
    public void DisplayDamageText(int _damageAmount)
    {
        StartCoroutine(DisplayDamage(_damageAmount));
    }

    IEnumerator DisplayDamage(int _damageAmount)
    {
        damageText.text = (_damageAmount > 0) ? $"-{_damageAmount}" : (_damageAmount == 0) ? "0" : $"+{_damageAmount}";
        damageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        damageText.gameObject.SetActive(false);
        damageText.text = "";
    }
}
