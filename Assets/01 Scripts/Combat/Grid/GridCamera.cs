using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class GridCamera handles the movement and rotation of the primary camera*/
public class GridCamera : MonoBehaviour
{
    float currentZoom;
    public float zoomSpeed = 3f;
    public float zoomAcceleration = 10f;
    public float zoomMin, zoomMax;

    Vector3 targetPosition;
    Vector3 targetEulers;

    public float movementSpeed = 10;
    public float accelerationSpeed = 10f;
    public float rotationSpeed = 10;

    public Unit followUnit;

    Camera cam;

    float delta, fixedDelta;

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
        cam = Camera.main;
        currentZoom = cam.orthographicSize;
    }

    void LateUpdate()
    {
        delta = Time.deltaTime;

        HandleCameraZoom();

        if (followUnit == null)
        {
            HandleCameraInput();
            HandleCameraMovement();
            HandleCameraRotation();
        }
        else
        {
            FollowUnit();
            HandleCameraMovement();
        }

    }

    /* HandleCameraZoom manages the size of the orthographic camera based on the scroll wheel */
    private void HandleCameraZoom()
    {
        currentZoom -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;

        currentZoom = Mathf.Clamp(currentZoom, zoomMin, zoomMax);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, currentZoom, delta * zoomAcceleration);
    }

    private void HandleCameraInput()
    {
        Vector3 _vertical = Input.GetAxisRaw("Vertical") * -((transform.right + transform.forward) * .5f);
        Vector3 _horizontal = Input.GetAxisRaw("Horizontal") * -((transform.right - transform.forward) * .5f);

        Vector3 _moveDir = (_vertical + _horizontal).normalized;

        targetPosition += _moveDir * movementSpeed;
    }

    /* HandleCameraMovement updates the camera's position via input*/
    private void HandleCameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * accelerationSpeed);
    }

    /* HandleCameraRotation sets the targetRotation by 90 degrees via inputs and updates the Camera's rotation*/
    private void HandleCameraRotation()
    {
        // Currently hard-coded to these keys for testing. Will make accessible later.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetEulers.y -= 90;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            targetEulers.y += 90;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetEulers), Time.deltaTime * rotationSpeed);
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
