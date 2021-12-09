using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    public abstract class UnitData : ScriptableObject
    {
        public string unitName;
        public Sprite unitIcon;

        [Header("Stats")]
        public int healthStat;
        public int inititiveStat;
        public int attackStat;
        public int defenseStat;
        public int apStat;

        private void OnValidate()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
