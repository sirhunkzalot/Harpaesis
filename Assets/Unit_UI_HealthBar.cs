using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Unit_UI_HealthBar : MonoBehaviour
{
    public Image healthBar;
    public Image barShadow;

    

    public float baseSpeed = 5f;
    public float shadowSpeed = 5f;
    public float tickDelay = 1f;

    public void Tick(float _hpPerc)
    {
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, _hpPerc, Time.deltaTime * baseSpeed);
        StartCoroutine(DelayedTick(_hpPerc));
    }

    IEnumerator DelayedTick(float _amount)
    {
        yield return new WaitForSeconds(tickDelay);

        barShadow.fillAmount = Mathf.MoveTowards(barShadow.fillAmount, _amount, Time.deltaTime * shadowSpeed);
    }
}
