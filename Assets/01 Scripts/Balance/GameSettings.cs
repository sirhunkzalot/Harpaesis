using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static UnitPassiveSettings unitPassiveSettings;
    public static StatusEffectSettings statusEffectSettings;

    private void Awake()
    {
        unitPassiveSettings = (UnitPassiveSettings)Resources.Load("Game Settings/Unit Passive Settings");
        statusEffectSettings = (StatusEffectSettings)Resources.Load("Game Settings/Status Effect Settings");
    }
}
