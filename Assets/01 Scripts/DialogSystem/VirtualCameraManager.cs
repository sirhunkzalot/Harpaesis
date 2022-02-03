using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraManager : MonoBehaviour
{
    CinemachineVirtualCamera[] vcams;

    public CinemachineVirtualCamera primaryCamera;

    public static VirtualCameraManager instance;

    private void Awake()
    {
        vcams = FindObjectsOfType<CinemachineVirtualCamera>();
        ReturnToPrimaryCamera();

        instance = this;
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

    public void ReturnToPrimaryCamera()
    {
        SetCameraPriority(primaryCamera);
    }
}
