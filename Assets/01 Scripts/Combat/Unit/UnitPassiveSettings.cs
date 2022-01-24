using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassiveSettings : ScriptableObject
{
    #region Friendly Passive Settings
    // Alexander

    // Cori

    // Doran

    // Joachim
    public int joachimBonusAtkAmount = 4;
    public int joachimBonusDefAmount = 4;

    // Regina

    #endregion

    #region Enemy Passive Settings
    // Vampires
    public float vampireBaseHealPercent = .2f;
    public float vampireBoostedHealPercent = .5f;

    // Lycans
    public float lycanHealPercentOfMissingHealth = .33f;

    #endregion
}
