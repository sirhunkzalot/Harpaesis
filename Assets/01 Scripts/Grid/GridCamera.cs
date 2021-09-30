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

    Camera cam;

    // Raycast Data
    Vector3 lastHitPoint;
    Vector3 lastOffset;
    public LayerMask mask;

    float delta, fixedDelta;

    public static GridCamera instance;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void Start()
    {
        targetEulers = transform.eulerAngles;
        targetPosition = transform.position;
        cam = Camera.main;
        currentZoom = cam.orthographicSize;

        HandleForwardRaycast();
    }

    void Update()
    {
        delta = Time.deltaTime;

        HandleCameraZoom();
        HandleCameraMovement();
        HandleCameraRotation();
        HandleForwardRaycast();
    }

    private void HandleCameraZoom()
    {
        currentZoom -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;

        currentZoom = Mathf.Clamp(currentZoom, zoomMin, zoomMax);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, currentZoom, delta * zoomAcceleration);
    }

    /* HandleCameraMovement updates the camera's position via input*/
    private void HandleCameraMovement()
    {
        Vector3 _vertical = Input.GetAxisRaw("Vertical") * transform.up;
        Vector3 _horizontal = Input.GetAxisRaw("Horizontal") * transform.right;

        Vector3 _moveDir = (_vertical + _horizontal).normalized;

        targetPosition += _moveDir * movementSpeed;

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

        //print(Quaternion.Angle(transform.rotation, Quaternion.Euler(targetEulers)));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetEulers), Time.deltaTime * rotationSpeed);
    }

    private void HandleForwardRaycast()
    {
        Ray _ray = new Ray(transform.position, transform.forward);
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, 100, mask))
        {
            lastHitPoint = _hit.point;
            lastOffset = transform.position - _hit.point;
        }
    }

    private void HandleSpeculativeForwardRaycast(Vector3 _origin)
    {
        Ray _ray = new Ray(_origin, transform.forward);
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, 100, mask))
        {
            lastHitPoint = _hit.point;
        }
    }

    public void JumpToPosition(Vector3 _worldPosition)
    {
        HandleForwardRaycast();

        targetPosition = _worldPosition + lastOffset;
    }
}
