using System.Collections;
using System.Collections.Generic;
using Harpaesis.UI.Tooltips;
using UnityEngine;

public class Tooltip_PreviewMove : MonoBehaviour
{
    public void Preview(int _apCost)
    {
        TooltipSystem.Show("", $"AP: {_apCost}");
    }

    public void EndPreview()
    {
        TooltipSystem.Hide();
    }
}
