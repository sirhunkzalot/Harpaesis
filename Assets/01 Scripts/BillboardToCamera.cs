using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    Transform cam;


    private void LateUpdate()
    {
        if (cam == null)
        {
            cam = Harpaesis.Combat.GridCamera.instance.transform;
        }
        transform.forward = cam.forward;
    }
}
