using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperlinks : MonoBehaviour
{
    public void OpenFeedback()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdhSL6nNw4haAFzVgCdxRGJUPnW28vPshvHJBnSxOtGtlGSlA/viewform?usp=sf_link");
    }
}
