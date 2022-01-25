
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class BalanceSettingsWindow : EditorWindow
{
    static UnitPassiveSettings passiveSettings;
    static StatusEffectSettings statusEffectSettings;
    static ItemSettings itemSettings;


    [MenuItem("Window/Balance Settings")]
    public static void ShowWindow()
    {
        LoadSettings();
        GetWindow<BalanceSettingsWindow>("Balance Settings");
    }

    private void OnGUI()
    {
        GUILayout.Space(1);
        GUILayout.Label("Friendly Passive Settings:", EditorStyles.whiteLargeLabel);

        GUILayout.Space(1);
        GUILayout.Label("Joachim Passive", EditorStyles.whiteLabel);
        passiveSettings.joachimBonusAtkAmount = EditorGUILayout.IntField("Attack Bonus", passiveSettings.joachimBonusAtkAmount);
        passiveSettings.joachimBonusDefAmount = EditorGUILayout.IntField("Defense Bonus", passiveSettings.joachimBonusDefAmount);

        GUILayout.Space(3);
        GUILayout.Label("Enemy Passive Settings:", EditorStyles.whiteLargeLabel);

        GUILayout.Space(1);
        GUILayout.Label("Vampire Passive", EditorStyles.whiteLabel);
        passiveSettings.vampireBaseHealPercent = EditorGUILayout.FloatField("Base Heal Percent", passiveSettings.vampireBaseHealPercent);
        passiveSettings.vampireBoostedHealPercent = EditorGUILayout.FloatField("Boosted Heal Percent", passiveSettings.vampireBoostedHealPercent);

        GUILayout.Space(1);
        GUILayout.Label("Lycan Passive", EditorStyles.whiteLabel);
        passiveSettings.lycanHealPercentOfMissingHealth = EditorGUILayout.FloatField("Heal Percent of Missing Health", passiveSettings.lycanHealPercentOfMissingHealth);

        GUILayout.Space(3);
        GUILayout.Label("Item Settings:", EditorStyles.whiteLargeLabel);

        GUILayout.Space(1);
        GUILayout.Label("Health Potion", EditorStyles.whiteLabel);
        itemSettings.potionHealAmount = EditorGUILayout.IntField("Heal Amount:", itemSettings.potionHealAmount);

        GUILayout.Label("Health Potion", EditorStyles.whiteLabel);
        itemSettings.atkPotionBuffAmount = EditorGUILayout.IntField("Buff Amount:", itemSettings.atkPotionBuffAmount);
        itemSettings.atkPotionBuffDuration = EditorGUILayout.IntField("Buff Duration:", itemSettings.atkPotionBuffDuration);

        GUILayout.Label("Health Potion", EditorStyles.whiteLabel);
        itemSettings.defPotionBuffAmount = EditorGUILayout.IntField("Buff Amount:", itemSettings.defPotionBuffAmount);
        itemSettings.defPotionBuffDuration = EditorGUILayout.IntField("Buff Duration:", itemSettings.defPotionBuffDuration);

    }

    static void LoadSettings()
    {
        if (passiveSettings == null)
        {
            passiveSettings = (UnitPassiveSettings)Resources.Load("Game Settings/Unit Passive Settings");

            if (passiveSettings == null)
            {
                passiveSettings = CreateInstance<UnitPassiveSettings>();
                AssetDatabase.CreateAsset(passiveSettings, "Assets/Resources/Game Settings/Unit Passive Settings.asset");
            }
        }

        if (statusEffectSettings == null)
        {
            statusEffectSettings = (StatusEffectSettings)Resources.Load("Game Settings/Status Effect Settings");

            if (statusEffectSettings == null)
            {
                statusEffectSettings = CreateInstance<StatusEffectSettings>();
                AssetDatabase.CreateAsset(statusEffectSettings, "Assets/Resources/Game Settings/Status Effect Settings.asset");
            }
        }

        if (itemSettings == null)
        {
            itemSettings = (ItemSettings)Resources.Load("Game Settings/Item Settings");

            if (itemSettings == null)
            {
                itemSettings = CreateInstance<ItemSettings>();
                AssetDatabase.CreateAsset(itemSettings, "Assets/Resources/Game Settings/Item Settings.asset");
            }
        }
    }
    static void SaveSettings()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    protected void OnEnable()
    {
        LoadSettings();
    }
    protected void OnFocus()
    {
        LoadSettings();
    }
    protected void OnValidate()
    {
        SaveSettings();
    }
    protected void OnLostFocus()
    {
        SaveSettings();
    }
    protected void OnDisable()
    {
        SaveSettings();
    }
    protected void OnDestroy()
    {
        SaveSettings();
    }
}
#endif