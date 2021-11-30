using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ProgressBarAppearing : MonoBehaviour {
  
    Vector3 targetScale;
    public Vector3 targetScaleP;
    public float appearTime = 1f;
    public float appearTimeColor = 1f;
    public Image animatedBackground;
    public Image animatedProgress;
    public bool animateFill;
    public bool animateProg;
    public bool animateColor;
    public bool animateSize;
    public Color fromColor;
    public Color toColor;

    private float controlTime;
    private float controlTimeProg;

    void Start () {
        if (animateSize) {               
           targetScale = animatedBackground.transform.localScale;
           animatedBackground.transform.localScale = targetScale / 4;        
            if (animatedProgress != null && animateProg) {
                targetScaleP = animatedProgress.transform.localScale;
                animatedProgress.transform.localScale = targetScaleP / 4;
            }
        }
        if (animateColor) {
            animatedBackground.color = fromColor;
        }
        if (animateFill) {
            animatedBackground.fillAmount = 0f;
        }
    }

	void Update () {
        CalculateSize();
        CalculateFill();
        CalculateColor();
        if (AnimationsIsOver()) {
            Destroy(this);
        }
    }

    private void CalculateSize() {
        if (animateSize) {
            if (animatedProgress != null && animateProg) {
                if (animatedProgress.transform.localScale.x < targetScaleP.x) {
                    animatedProgress.transform.localScale += targetScaleP / appearTime * Time.deltaTime;
                } else {
                    animateProg = false;
                }
            }
            if (animatedBackground.transform.localScale.x < targetScale.x) {
                animatedBackground.transform.localScale += targetScale / appearTime * Time.deltaTime;
            } else {
                animateSize = animateProg;
            }
        }
    }

    private void CalculateFill() {
        if (animateFill) {
            animatedBackground.fillAmount += 1.0f / appearTime * Time.deltaTime;
            if (animatedBackground.fillAmount == 1f) {
                animateFill = false;
            }
        }
    }

    private void CalculateColor() {
        if (animateColor) {
            if (controlTime < 1) {
                controlTime += Time.deltaTime / appearTimeColor;
            } else {
                animateColor = false;
            }
            animatedBackground.color = Color.Lerp(fromColor, toColor, controlTime);
        }
    }

    private bool AnimationsIsOver() {
        return !(animateSize || animateColor || animateFill || animateColor);
    }
}

