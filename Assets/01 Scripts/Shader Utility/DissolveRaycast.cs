using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveRaycast : MonoBehaviour
{
    Camera cam;

    List<GameObject> mustViewObjects = new List<GameObject>();

    List<GameObject> obstacles = new List<GameObject>();

    public float minimumHeight = .25f;
    public float maximumHeight = 50;
    public float dissolveSpeed = 5f;

    public LayerMask layermask;

    public static DissolveRaycast instance;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        instance = this;
    }

    private void Start()
    {
        Unit[] units = TurnManager.instance.units.ToArray();
        mustViewObjects.Add(GridCursor.instance.gameObject);

        for (int i = 0; i < units.Length; i++)
        {
            mustViewObjects.Add(units[i].gameObject);
        }

        GameObject[] _allObjects = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject _g in _allObjects)
        {
            obstacles.Add(_g);
        }
    }

    private void FixedUpdate()
    {
        List<GameObject> _hitObjects = new List<GameObject>();

        foreach (GameObject viewableObject in mustViewObjects)
        {
            Collider _collider = viewableObject.GetComponent<Collider>();

            if(_collider != null)
            {
                Vector3[] _targets = new Vector3[]
                {
                    viewableObject.transform.position,
                    _collider.bounds.center,
                    new Vector3(_collider.bounds.center.x + _collider.bounds.extents.x, _collider.bounds.center.y + _collider.bounds.extents.y, _collider.bounds.center.z + _collider.bounds.extents.z),
                    new Vector3(_collider.bounds.center.x + _collider.bounds.extents.x, _collider.bounds.center.y + _collider.bounds.extents.y, _collider.bounds.center.z - _collider.bounds.extents.z),
                    new Vector3(_collider.bounds.center.x + _collider.bounds.extents.x, _collider.bounds.center.y - _collider.bounds.extents.y, _collider.bounds.center.z + _collider.bounds.extents.z),
                    new Vector3(_collider.bounds.center.x + _collider.bounds.extents.x, _collider.bounds.center.y - _collider.bounds.extents.y, _collider.bounds.center.z - _collider.bounds.extents.z),
                    new Vector3(_collider.bounds.center.x - _collider.bounds.extents.x, _collider.bounds.center.y + _collider.bounds.extents.y, _collider.bounds.center.z + _collider.bounds.extents.z),
                    new Vector3(_collider.bounds.center.x - _collider.bounds.extents.x, _collider.bounds.center.y + _collider.bounds.extents.y, _collider.bounds.center.z - _collider.bounds.extents.z),
                    new Vector3(_collider.bounds.center.x - _collider.bounds.extents.x, _collider.bounds.center.y - _collider.bounds.extents.y, _collider.bounds.center.z + _collider.bounds.extents.z),
                    new Vector3(_collider.bounds.center.x - _collider.bounds.extents.x, _collider.bounds.center.y - _collider.bounds.extents.y, _collider.bounds.center.z - _collider.bounds.extents.z)
                };

                foreach (Vector3 _target in _targets)
                {
                    Vector3 _dir = (_target - transform.position).normalized;
                    Ray _ray = new Ray(transform.position, _dir);
                    RaycastHit[] _hits = Physics.RaycastAll(_ray, 100, layermask);

                    foreach (RaycastHit _hit in _hits)
                    {
                        GameObject _hitObject = _hit.collider.gameObject;
                        if (!_hitObjects.Contains(_hitObject))
                        {
                            _hitObjects.Add(_hitObject);
                        }
                    }

                    Debug.DrawLine(transform.position, _target);
                }
            }
            else
            {
                Vector3 _dir = (viewableObject.transform.position - transform.position).normalized;
                Ray _ray = new Ray(transform.position, _dir);
                RaycastHit[] _hits = Physics.RaycastAll(_ray, 100, layermask);

                foreach (RaycastHit _hit in _hits)
                {
                    GameObject _hitObject = _hit.collider.gameObject;
                    if (!_hitObjects.Contains(_hitObject))
                    {
                        _hitObjects.Add(_hitObject);
                    }
                }

                Debug.DrawLine(transform.position, viewableObject.transform.position);
            }
        }

        foreach (GameObject _go in obstacles)
        {
            Renderer _ren = _go.GetComponent<Renderer>();

            if (_ren == null) { continue; }

            Material[] _mats = _ren.materials;

            float _targetHeight = (_hitObjects.Contains(_go)) ? minimumHeight : maximumHeight;


            for (int i = 0; i < _mats.Length; i++)
            {
                float _height = Mathf.Lerp(_mats[i].GetFloat("MaxHeight"), _targetHeight, Time.fixedDeltaTime * dissolveSpeed);
                _mats[i].SetFloat("MaxHeight", _height);
            }
        }
    }

    public void RemoveGameobject(GameObject _objectToRemove)
    {
        if (mustViewObjects.Contains(_objectToRemove))
        {
            mustViewObjects.Remove(_objectToRemove);
        }
    }
}
