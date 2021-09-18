using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/New Unit Data")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public int healthStat;
    public int inititiveStat;
    public int attackStat;
    public int defenceStat;
    public int apStat;

    private void OnValidate()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
