using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveRaycast : MonoBehaviour
{
    Camera cam;
    public Transform[] units;
    public LayerMask layermask;

    GameObject[] objects;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        objects = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    private void FixedUpdate()
    {
        List<Vector4> _unitCentersOnScreen = new List<Vector4>();
        foreach (Transform unit in units)
        {
            Vector3 _dir = unit.position - transform.position;
            RaycastHit _hit;

            if(Physics.Raycast(transform.position, _dir, out _hit, 100, layermask))
            {
                Debug.DrawLine(transform.position, _hit.point);

                Vector3 _viewportPoint = cam.WorldToViewportPoint(unit.position);
                _unitCentersOnScreen.Add(new Vector4(_viewportPoint.x, _viewportPoint.y));
            }
        }

        foreach (GameObject obj in objects)
        {
            Renderer _r = obj.GetComponent<Renderer>();

            if(_unitCentersOnScreen.Count > 0)
            {
                for (int i = 0; i < _unitCentersOnScreen.Count; i++)
                {
                    _r.material.SetVector("Center", _unitCentersOnScreen[i]);
                }
            }
            else
            {
                _r.material.SetVector("Center", Vector4.one * -1);
            }
        }
    }
}
