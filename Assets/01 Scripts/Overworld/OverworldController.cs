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

        public static OverworldController instance;

        bool CanInput { get { return Vector3.Distance(transform.position, currentPoint.transform.position) <= 0.5f; } }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (OverworldData.lastPointIndex == float.MinValue)
            {
                currentPoint = startingPoint;
                currentPoint.isUnlocked = true;
                OverworldData.SetLastPoint(currentPoint.Index);
                transform.position = currentPoint.transform.position;
            }
        }

        public void Init(OverworldPoint _newPoint)
        {
            currentPoint = _newPoint;
            transform.position = currentPoint.transform.position;
        }

        private void Update()
        {
            if (CanInput)
            {
                if (Input.GetKeyDown(KeyCode.W) && currentPoint.upConnection != null && currentPoint.upConnection.isUnlocked)
                {
                    ChangeCurrentPoint(currentPoint.upConnection);
                }
                else if (Input.GetKeyDown(KeyCode.D) && currentPoint.rightConnection != null && currentPoint.rightConnection.isUnlocked)
                {
                    ChangeCurrentPoint(currentPoint.rightConnection);
                }
                else if (Input.GetKeyDown(KeyCode.S) && currentPoint.downConnection != null && currentPoint.downConnection.isUnlocked)
                {
                    ChangeCurrentPoint(currentPoint.downConnection);
                }
                else if (Input.GetKeyDown(KeyCode.A) && currentPoint.leftConnection != null && currentPoint.leftConnection.isUnlocked)
                {
                    ChangeCurrentPoint(currentPoint.leftConnection);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    OverworldData.SetLastPoint(currentPoint.Index);
                    currentPoint.Interact();
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, Time.deltaTime * moveSpeed);
        }

        private void ChangeCurrentPoint(OverworldPoint _newPoint)
        {
            currentPoint = _newPoint;
            OverworldData.SetLastPoint(currentPoint.Index);
        }
    }
}