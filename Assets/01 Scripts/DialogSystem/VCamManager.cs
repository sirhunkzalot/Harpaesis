using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VCamManager : MonoBehaviour
{
    CinemachineVirtualCamera[] vcams;

    private void Awake()
    {
        vcams = FindObjectsOfType<CinemachineVirtualCamera>();
    }

    public void SetCameraPriority(CinemachineVirtualCamera _camToSet)
    {
        _camToSet.Priority = 50;

        foreach (CinemachineVirtualCamera vcam in vcams)
        {
            if(vcam != _camToSet)
            {
                vcam.Priority = -10;
            }
        }
    }
}
