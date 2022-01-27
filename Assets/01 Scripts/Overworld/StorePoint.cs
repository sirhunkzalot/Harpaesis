using System.Collections.Generic;
using Cinemachine;
using Harpaesis.Inventory;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class StorePoint : OverworldPoint
    {
        [Header("Store Settings")]
        [SerializeField] List<StoreItem> storeItems = new List<StoreItem>();

        public CinemachineVirtualCamera storeCam;

        public override void Interact()
        {
            LoadStore();
        }

        private void LoadStore()
        {
            VcamManager.instance.SetCameraPriority(storeCam);
        }

        [System.Serializable]
        struct StoreItem
        {
            public Item item;
            public int itemPrice;
        }
    }
}