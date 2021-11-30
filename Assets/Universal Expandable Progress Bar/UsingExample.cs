using UnityEngine;

public class UsingExample : MonoBehaviour, OnCompleteListener {

    public ProgressBar circularInside, circularInside2, horizontal, vertical;

	void Update () {
        if (Input.GetKeyDown(KeyCode.W)) {
            setProgress(100, true);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            setProgress(0, true);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            getProgress();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            vertical.SetTextSize(21)
                .SetAfterText(" /1000 Nice text after progress count.  ")
                .SetMax(1000)
                .SetColor(Color.magenta)
                .SetProgress(500, true);
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            circularInside.addListener(this);
            Debug.Log("Listener was added on green PB");
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            circularInside.removeListener(this);
            Debug.Log("Listener was removed from green PB");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            setProgress(80, false);
        }

    }
    public void setProgress(int count, bool smooth) {
        circularInside.SetProgress(count, smooth);
        circularInside2.SetProgress(count, smooth);
        horizontal.SetProgress(count, smooth);
        vertical.SetProgress(count, smooth);
    }
    public void getProgress() {
        Debug.Log(circularInside2.GetProgress().ToString());
        Debug.Log(circularInside.GetProgress().ToString());
        Debug.Log(horizontal.GetProgress().ToString());
        Debug.Log(vertical.GetProgress().ToString());
    }

    public void progressBarComplete() {
        Debug.Log("Listener says: hey, green one is full!");
    }
}
