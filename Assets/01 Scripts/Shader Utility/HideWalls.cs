using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    public class HideWalls : MonoBehaviour
    {
        Camera cam;

        List<GameObject> allHideableObjects = new List<GameObject>();

        public List<GameObject> frontViewHiddenObjects = new List<GameObject>();
        public List<GameObject> rightViewHiddenObjects = new List<GameObject>();
        public List<GameObject> backViewHiddenObjects = new List<GameObject>();
        public List<GameObject> leftViewHiddenObjects = new List<GameObject>();

        public float minimumHeight = .25f;
        public float maximumHeight = 50;
        public float dissolveSpeed = 5f;

        public static HideWalls instance;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            instance = this;
        }

        private void Start()
        {
            GameObject[] _allObjects = GameObject.FindGameObjectsWithTag("Wall");

            foreach (GameObject _g in _allObjects)
            {
                allHideableObjects.Add(_g);
            }
        }

        public void CameraTick(CameraView _view)
        {
            foreach (GameObject _go in allHideableObjects)
            {
                Renderer[] renderers = _go.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < renderers.Length; i++)
                {
                    Renderer _ren = renderers[i];

                    if (_ren == null) { continue; }

                    Material[] _mats = _ren.materials;

                    List<GameObject> _objectsToHide = new List<GameObject>();

                    switch (_view)
                    {
                        case CameraView.Front:
                            _objectsToHide = frontViewHiddenObjects;
                            break;
                        case CameraView.Right:
                            _objectsToHide = rightViewHiddenObjects;
                            break;
                        case CameraView.Back:
                            _objectsToHide = backViewHiddenObjects;
                            break;
                        case CameraView.Left:
                            _objectsToHide = leftViewHiddenObjects;
                            break;
                        default:
                            throw new System.Exception("Invalid Camera View Given");
                    }

                    float _targetHeight = (_objectsToHide.Count > 0 && _objectsToHide.Contains(_go)) ? minimumHeight : maximumHeight;

                    for (int i = 0; i < _mats.Length; i++)
                    {
                        if (_mats[i].HasFloat("MaxHeight"))
                        {
                            float _height = Mathf.Lerp(_mats[i].GetFloat("MaxHeight"), _targetHeight, Time.fixedDeltaTime * dissolveSpeed);
                            _mats[i].SetFloat("MaxHeight", _height);
                        }
                    }
                }
            }
            }
        }
    }
}