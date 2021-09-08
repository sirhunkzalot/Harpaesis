using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridAndPathfinding
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathResult> results = new Queue<PathResult>();

        Pathfinder pathfinder;

        static PathRequestManager instance;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }

            pathfinder = GetComponent<Pathfinder>();
        }

        private void Update()
        {
            if(results.Count > 0)
            {
                int _itemsInQueue = results.Count;
                lock (results)
                {
                    for (int i = 0; i < _itemsInQueue; i++)
                    {
                        PathResult _result = results.Dequeue();
                        _result.callback(_result);
                    }
                }
            }
        }

        public static void RequestPath(PathRequest _request)
        {
            ThreadStart _threadStart = delegate
            {
                instance.pathfinder.FindPath(_request, instance.FinishedProcessingPath);
            };
            _threadStart.Invoke();
        }

        public void FinishedProcessingPath(PathResult _result)
        {
            lock (results)
            {
                results.Enqueue(_result);
            }
        }
    }

    public struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<PathResult> callback;
        public int availibleAP;

        public PathRequest(Vector3 _start, Vector3 _end, Action<PathResult> _callback, int _availibleAP = -1)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
            availibleAP = _availibleAP;
        }
    }

    public struct PathResult
    {
        public Vector3[] path;
        public bool success;
        public Action<PathResult> callback;
        public int usedAP;


        public PathResult(Vector3[] _path, bool _success, Action<PathResult> _callback, int _usedAP)
        {
            path = _path;
            success = _success;
            callback = _callback;
            usedAP = _usedAP;
        }
    }
}
