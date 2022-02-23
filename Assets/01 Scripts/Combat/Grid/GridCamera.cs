using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    /**
     * @author Matthew Sommer
     * class GridCamera handles the movement and rotation of the primary camera*/
    public class GridCamera : MonoBehaviour
    {
        [Header("Data")]
        [ReadOnly, SerializeField] Vector3 currentZoom;
        [ReadOnly, SerializeField] Vector3 targetPosition;
        [ReadOnly, SerializeField] Vector3 targetEulers;
        [ReadOnly] public Unit followUnit;

        [Header("Settings")]
        public float zoomSpeed = 3f;
        public float zoomAcceleration = 10f;

        public float movementSpeed = 10;
        public float accelerationSpeed = 10f;
        public float rotationSpeed = 10;

        [Header("Constraints")]
        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;
        public float zMin;
        public float zMax;

        HideWalls hideWalls;
        PlayerInput_Combat input;
        Camera cam;

        CameraView currentView = CameraView.Front;

        float delta;

        #region Singleton
        public static GridCamera instance;

        private void Awake()
        {

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }
        #endregion

        private void Start()
        {
            targetEulers = transform.eulerAngles;
            targetPosition = transform.position;
            cam = GetComponentInChildren<Camera>();
            hideWalls = cam.GetComponent<HideWalls>();
            currentZoom = cam.transform.localPosition;

            input = PlayerInput_Combat.instance;
        }

        void LateUpdate()
        {
            delta = Time.deltaTime;

            HandleCameraZoom();

            if (followUnit == null)
            {
                HandleCameraInput();
            }
            else
            {
                FollowUnit();
            }

            HandleCameraMovement();
            HandleCameraRotation();

            hideWalls.CameraTick(currentView);
        }

        /* HandleCameraZoom manages the size of the orthographic camera based on the scroll wheel */
        private void HandleCameraZoom()
        {
            Vector3 _newZoom = currentZoom + input.cameraScroll * zoomSpeed * cam.transform.InverseTransformDirection(-cam.transform.up - cam.transform.forward - cam.transform.right);

            

            if (_newZoom.y >= yMin && _newZoom.y <= yMax)
            {
                currentZoom = _newZoom;
            }
        }

        private void HandleCameraInput()
        {
            Vector2 _camMove = input.cameraMovement;

            Vector3 _vertical = _camMove.y * -((transform.right + transform.forward) * .5f);
            Vector3 _horizontal = _camMove.x * -((transform.right - transform.forward) * .5f);

            Vector3 _moveDir = (_vertical + _horizontal).normalized;

            Vector3 _newPosition = targetPosition + (_moveDir * movementSpeed);

            _newPosition.x = Mathf.Clamp(_newPosition.x, xMin, xMax);
            _newPosition.z = Mathf.Clamp(_newPosition.z, zMin, zMax);

            targetPosition = _newPosition;
        }

        /* HandleCameraMovement updates the camera's position via input*/
        private void HandleCameraMovement()
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * accelerationSpeed);
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, currentZoom, delta * zoomAcceleration);
        }

        public void RotateLeft()
        {
            targetEulers.y -= 90;

            switch (currentView)
            {
                case CameraView.Front:
                    currentView = CameraView.Left;
                    break;
                case CameraView.Right:
                    currentView = CameraView.Front;
                    break;
                case CameraView.Back:
                    currentView = CameraView.Right;
                    break;
                case CameraView.Left:
                    currentView = CameraView.Back;
                    break;
                default:
                    throw new System.Exception("Invalid Camera View Given");
            }
        }

        public void RotateRight()
        {
            targetEulers.y += 90;

            switch (currentView)
            {
                case CameraView.Front:
                    currentView = CameraView.Right;
                    break;
                case CameraView.Right:
                    currentView = CameraView.Back;
                    break;
                case CameraView.Back:
                    currentView = CameraView.Left;
                    break;
                case CameraView.Left:
                    currentView = CameraView.Front;
                    break;
                default:
                    throw new System.Exception("Invalid Camera View Given");
            }
        }


        /* HandleCameraRotation sets the targetRotation by 90 degrees via inputs and updates the Camera's rotation*/
        private void HandleCameraRotation()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetEulers), Time.deltaTime * rotationSpeed);
        }

        public void JumpToCurrentUnit()
        {
            JumpToPosition(TurnManager.instance.activeTurn.unit.transform.position);
        }

        /* JumpToPosition moves the camera to keep its current vertical offset while centering on the world position*/
        public void JumpToPosition(Vector3 _worldPosition)
        {
            targetPosition = _worldPosition;
        }

        void FollowUnit()
        {
            JumpToPosition(followUnit.transform.position);
        }
    }

    public enum CameraView { Front, Right, Left, Back }
}
