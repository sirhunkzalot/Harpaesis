using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class OverworldPoint : MonoBehaviour
    {
        public OverworldPoint upConnection, rightConnection, downConnection, leftConnection;

        public bool pointIsComplete;

        float _offsetAmount = .05f;

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
                Gizmos.DrawLine(transform.position + (Vector3.right + Vector3.up) * _offsetAmount, upConnection.transform.position + (Vector3.right + Vector3.up) * _offsetAmount);
            }
            if (rightConnection != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position + (Vector3.back + Vector3.up) * _offsetAmount, rightConnection.transform.position + (Vector3.back + Vector3.up) * _offsetAmount);
            }
            if (downConnection != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position + (Vector3.left + Vector3.up) * _offsetAmount, downConnection.transform.position + (Vector3.left + Vector3.up) * _offsetAmount);
            }
            if (leftConnection != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position + (Vector3.forward + Vector3.up) * _offsetAmount, leftConnection.transform.position + (Vector3.forward + Vector3.up) * _offsetAmount);
            }
        }
    }
}