using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        currentZoom = cam.transform.position;
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
    }

    /* HandleCameraZoom manages the size of the orthographic camera based on the scroll wheel */
    private void HandleCameraZoom()
    {
        Vector3 _newZoom = currentZoom + Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed * cam.transform.forward;

        if(_newZoom.y >= yMin && _newZoom.y <= yMax)
        {
            currentZoom = _newZoom;
        }
    }

    private void HandleCameraInput()
    {
        Vector3 _vertical = Input.GetAxisRaw("Vertical") * -((transform.right + transform.forward) * .5f);
        Vector3 _horizontal = Input.GetAxisRaw("Horizontal") * -((transform.right - transform.forward) * .5f);

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
