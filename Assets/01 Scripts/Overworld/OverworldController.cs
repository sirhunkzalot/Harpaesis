using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class OverworldController : MonoBehaviour
    {
        public OverworldPoint startingPoint;
        [ReadOnly] public OverworldPoint currentPoint;

        public float moveSpeed = 5f;

        bool CanInput { get { return Vector3.Distance(transform.position, currentPoint.transform.position) <= 0.5f; } }

        private void Start()
        {
            currentPoint = startingPoint;
            currentPoint.pointIsComplete = true;
            transform.position = currentPoint.transform.position;
        }

        private void Update()
        {
            if (CanInput)
            {
                if (Input.GetKeyDown(KeyCode.W) && currentPoint.upConnection != null && (currentPoint.pointIsComplete || currentPoint.upConnection.pointIsComplete))
                {
                    currentPoint = currentPoint.upConnection;
                }
                else if (Input.GetKeyDown(KeyCode.D) && currentPoint.rightConnection != null && (currentPoint.pointIsComplete || currentPoint.rightConnection.pointIsComplete))
                {
                    currentPoint = currentPoint.rightConnection;
                }
                else if (Input.GetKeyDown(KeyCode.S) && currentPoint.downConnection != null && (currentPoint.pointIsComplete || currentPoint.downConnection.pointIsComplete))
                {
                    currentPoint = currentPoint.downConnection;
                }
                else if (Input.GetKeyDown(KeyCode.A) && currentPoint.leftConnection != null && (currentPoint.pointIsComplete || currentPoint.leftConnection.pointIsComplete))
                {
                    currentPoint = currentPoint.leftConnection;
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    currentPoint.Interact();
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, Time.deltaTime * moveSpeed);
        }
    }
}