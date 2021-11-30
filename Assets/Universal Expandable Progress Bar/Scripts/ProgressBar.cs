using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteInEditMode]
[System.Serializable]
public class ProgressBar : MonoBehaviour {
    private List<OnCompleteListener> listeners = new List<OnCompleteListener>();

    [Tooltip("Image for progress bar.")]
    [SerializeField]
    protected Image progress;
    [Tooltip("Text with current progressbar caption.")]
    [SerializeField]
    protected Text text;

    [Tooltip("Text that will appear before progress bar value.")]
    [SerializeField]
    protected string beforeText = "";

    [Tooltip("Text that will appear after progress bar value.")]
    [SerializeField]
    protected string afterText = "%";

    [Tooltip("Max possible progress.")]
    [SerializeField]
    protected int max = 100;

    [Range(1, 10)]
	[SerializeField]
    protected float smoothTime = 2;

    protected bool smoothly;
    protected int facticalProgress;
    [SerializeField]
    protected int currentProgress;

    [SerializeField]
    private bool isFull;

    void Start() {
        progress.fillAmount = 0;
    }

    void Update() {
        CalculateProgress();
        NotifyListeners();
    }

    public int GetProgress() {
        return facticalProgress;
    }

    public void SetProgress(int progress) {
        smoothly = false;
        currentProgress = progress;
        CalculateProgress();
    }

    public void SetProgress(int progress, bool smooth) {
        smoothly = smooth;
        currentProgress = progress;
    }

    public virtual ProgressBar SetColor(Color c) {
        progress.color = c;
        return this;
    }

    public ProgressBar SetTextSize(int size) {
        text.fontSize = size;
        return this;
    }

    public virtual ProgressBar SetAfterText(string text) {
        afterText = text;
        return this;
    }

    public ProgressBar SetMax(int max) {
        if (max < 0) {
            return this;
        }
        this.max = max;
        if (facticalProgress > max) {
            facticalProgress = max;
        }
        if (currentProgress > max) {
            currentProgress = max;
        }
        return this;
    }

    public virtual ProgressBar SetBeforeText(string text) {
        beforeText = text;
        return this;
    }

    public string GetBeforeText() {
        return beforeText;
    }

    public ProgressBar addListener(OnCompleteListener listener) {
        if (!listeners.Contains(listener)) {
            listeners.Add(listener);
        }
        return this;
    }

    public ProgressBar removeListener(OnCompleteListener listener) {
        if (!listeners.Contains(listener)) {
            listeners.Remove(listener);
        }
        return this;
    }

    protected virtual void CalculateProgress() {
        if (IsMinMax()) return;
        if (smoothly) {
            float fillNeed = currentProgress / (float)max;
            float currentFill = Mathf.Lerp(progress.fillAmount, fillNeed, smoothTime * Time.unscaledDeltaTime);
            progress.fillAmount = currentFill;
            facticalProgress = Mathf.RoundToInt(currentFill * max);
        } else {
            facticalProgress = currentProgress;
            progress.fillAmount = facticalProgress / (float)max;
        }
        if (text != null) {
            text.text = beforeText + facticalProgress + afterText;
        }
    }

    protected bool IsMinMax() {
        if (currentProgress > max) {
            currentProgress = max;
            return true;
        }
        if (currentProgress < 0) {
            currentProgress = 0;
            return true;
        }
        return false;
    }

    private void NotifyListeners() {
        if (facticalProgress == max && !isFull) {
            isFull = true;
            foreach (OnCompleteListener l in listeners) {
                if (l != null) {
                    l.progressBarComplete();
                }
            }
        } else if (facticalProgress < max) {
            isFull = false;
        }
    }
}