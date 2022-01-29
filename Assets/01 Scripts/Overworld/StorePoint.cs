using System.Collections.Generic;
using Cinemachine;
using Harpaesis.Inventory;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class StorePoint : OverworldPoint
    {
        public CinemachineVirtualCamera storeCam;

        public override void Interact()
        {
            LoadStore();
        }

        private void LoadStore()
        {
            VcamManager.instance.SetCameraPriority(storeCam);
        }
    }
}