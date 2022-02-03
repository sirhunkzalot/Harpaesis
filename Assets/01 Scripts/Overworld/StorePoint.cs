using System.Collections.Generic;
using Cinemachine;
using Harpaesis.UI;
using Harpaesis.Inventory;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class StorePoint : OverworldPoint
    {
        public CinemachineVirtualCamera storeCam;

        bool isInteracting;

        public override void Interact()
        {
            if (!isInteracting)
            {
                LoadStore();
            }
            else
            {
                UnloadStore();
            }

            isInteracting = !isInteracting;
        }

        private void LoadStore()
        {
            VirtualCameraManager.instance.SetCameraPriority(storeCam);
            UIManager_Store.instance.EnterStore();
        }

        private void UnloadStore()
        {
            VirtualCameraManager.instance.ReturnToPrimaryCamera();
            UIManager_Store.instance.LeaveStore();
        }
    }
}