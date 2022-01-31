using System.Collections.Generic;
using Cinemachine;
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
            VcamManager.instance.SetCameraPriority(storeCam);
        }

        private void UnloadStore()
        {
            VcamManager.instance.ReturnToPrimaryCamera();
        }
    }
}