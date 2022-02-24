using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class OverworldPoint : MonoBehaviour
    {
        public OverworldPoint upConnection, rightConnection, downConnection, leftConnection;

        public UnityEvent onCompletePoint;

        public bool isUnlocked;

        readonly float offsetAmount = .05f;

        protected OverworldController controller;

        [HideInInspector] public float Index { get { return transform.position.x + transform.position.y + transform.position.z; } }

        private void Start()
        {
            controller = OverworldController.instance;
            OverworldData.OverworldPointInit(Index, new PointData(this, isUnlocked));
        }

        public virtual void Interact()
        {

        }

        private void OnValidate()
        {
            if (upConnection != null)
            {
                upConnection.downConnection = this;
            }
            if (rightConnection != null)
            {
                rightConnection.leftConnection = this;
            }
            if (downConnection != null)
            {
                downConnection.upConnection = this;
            }
            if (leftConnection != null)
            {
                leftConnection.rightConnection = this;
            }
        }

        private void OnDrawGizmos()
        {
            if (upConnection != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position + (Vector3.right + Vector3.up) * offsetAmount, upConnection.transform.position + (Vector3.right + Vector3.up) * offsetAmount);
            }
            if (rightConnection != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position + (Vector3.back + Vector3.up) * offsetAmount, rightConnection.transform.position + (Vector3.back + Vector3.up) * offsetAmount);
            }
            if (downConnection != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position + (Vector3.left + Vector3.up) * offsetAmount, downConnection.transform.position + (Vector3.left + Vector3.up) * offsetAmount);
            }
            if (leftConnection != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position + (Vector3.forward + Vector3.up) * offsetAmount, leftConnection.transform.position + (Vector3.forward + Vector3.up) * offsetAmount);
            }
        }

        public void UnlockPoint()
        {
            print("Point Unlocked");
            OverworldData.OverridePointData(Index, true);
            isUnlocked = true;
        }

        public void CompletePoint()
        {
            Debug.Log("Point Complete!");
            onCompletePoint.Invoke();

        }
    }
}