using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class templateMouseOver : MonoBehaviour
{
    public Material red;
    public Material green;

    public void OnMouseOver()
    {
        GetComponent<Renderer>().material = green;
    }

    public void OnMouseExit()
    {
        GetComponent<Renderer>().material = red;
    }

}
