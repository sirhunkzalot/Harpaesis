using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_EndCombatScreen : MonoBehaviour
{
    public GameObject victoryScreen;
    public GameObject defeatScreen;

    public static UIManager_EndCombatScreen instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);
    }

    public void OpenVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    public void OpenLoseScreen()
    {
        defeatScreen.SetActive(true);
    }
}
