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

        [ReadOnly] public bool isInteracting;
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
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, Time.deltaTime * moveSpeed);
        }

        public void MoveNorth()
        {
            if (!isInteracting && CanInput && currentPoint.upConnection != null && currentPoint.upConnection.isUnlocked)
            {
                ChangeCurrentPoint(currentPoint.upConnection);
            }
        }

        public void MoveEast()
        {
            if (!isInteracting && CanInput && currentPoint.rightConnection != null && currentPoint.rightConnection.isUnlocked)
            {
                ChangeCurrentPoint(currentPoint.rightConnection);
            }
        }
        public void MoveSouth()
        {
            if (!isInteracting && CanInput && currentPoint.downConnection != null && currentPoint.downConnection.isUnlocked)
            {
                ChangeCurrentPoint(currentPoint.downConnection);
            }
        }

        public void MoveWest()
        {
            if (!isInteracting && CanInput && currentPoint.leftConnection != null && currentPoint.leftConnection.isUnlocked)
            {
                ChangeCurrentPoint(currentPoint.leftConnection);
            }
        }

        public void Interact()
        {
            if (CanInput)
            {
                OverworldData.SetLastPoint(currentPoint.Index);
                currentPoint.Interact();
            }
        }

        private void ChangeCurrentPoint(OverworldPoint _newPoint)
        {
            currentPoint = _newPoint;
            OverworldData.SetLastPoint(currentPoint.Index);
        }
    }
}