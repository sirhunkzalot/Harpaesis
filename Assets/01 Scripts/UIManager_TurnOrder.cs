using System.Collections;
using Harpaesis.Combat;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager_TurnOrder : MonoBehaviour
{
    public List<Image> turnImages = new List<Image>();

    Turn[] turns;

    public static UIManager_TurnOrder instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateImages(int _turnIndex, List<Turn> _turns)
    {
        for (int i = 0; i < turnImages.Count; i++)
        {
            int _newIndex = _turnIndex + i;

            while(_newIndex >= _turns.Count)
            {
                _newIndex -= _turns.Count;
            }

            UnitData _unitData = _turns[_newIndex].unitData;

            turnImages[i].sprite = (_unitData.unitIcon != null) ? _unitData.unitIcon : null;
        }
    }
}
