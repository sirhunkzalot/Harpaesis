﻿using UnityEngine;

public class ColoredProgressBar : ProgressBar {

    [SerializeField] private Color finalColor;
    [SerializeField] private Color startColor;

    private bool maxGained;

    public override ProgressBar SetColor(Color c) {
        finalColor = c;
        return this;
    }

    public ProgressBar SetFinalColor(Color c) {
        finalColor = c;
        return this;
    }

    public ProgressBar SetStartColor(Color c) {
        startColor = c;
        return this;
    }

    protected override void CalculateProgress() {
        if (IsMinMax()) return;
        if (smoothly) {
            float fillNeed = currentProgress / (float)max;
            float currentFill = Mathf.Lerp(progress.fillAmount, fillNeed, smoothTime*Time.deltaTime);
            progress.fillAmount = currentFill;
            facticalProgress = Mathf.RoundToInt(currentFill * max);
        } else {
            facticalProgress = currentProgress;
            progress.fillAmount = facticalProgress / (float)max;
        }

        float progressForColor = facticalProgress / (float) max;
        progress.color = startColor * (1 - progressForColor) + finalColor * progressForColor;


        if (!maxGained) {
            if (GetProgress() == (float)max)
            {
                maxGained = true;
            }
        } else {
            if (GetProgress() < (float)max)
            {
                maxGained = false;
            }
        }
    }

}
