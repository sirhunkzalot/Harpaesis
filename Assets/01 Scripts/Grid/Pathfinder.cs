using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridAndPathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        GridManager grid;

        private void Awake()
        {
            grid = GetComponent<GridManager>();
        }

        public void FindPath(PathRequest _request, Action<PathResult> _callback)
        {
            Vector3[] _waypoints = new Vector3[0];
            bool _pathSuccess = false;
            int _usedAP = 0;

            Node _startNode = grid.NodeFromWorldPoint(_request.pathStart);
            Node _targetNode = grid.NodeFromWorldPoint(_request.pathEnd);

            if(_startNode.walkable && _targetNode.walkable)
            {
                Heap<Node> _openSet = new Heap<Node>(grid.MaxSize);
                HashSet<Node> _closedSet = new HashSet<Node>();

                _openSet.Add(_startNode);

                while (_openSet.Count > 0)
                {
                    Node _currentNode = _openSet.RemoveFirst();
                    _closedSet.Add(_currentNode);

                    if (_currentNode == _targetNode)
                    {
                        // Found Path
                        _pathSuccess = true;
                        break;
                    }

                    foreach (Node _neighbor in grid.GetNeighbors(_currentNode))
                    {
                        if (!_neighbor.walkable || _closedSet.Contains(_neighbor))
                        {
                            continue;
                        }

                        int _newMovementCostToNeighbor = _currentNode.gCost + GetDistance(_currentNode, _neighbor) + _neighbor.movementPenalty;

                        bool _v = _newMovementCostToNeighbor < _neighbor.gCost || !_openSet.Contains(_neighbor);
                        bool _pathIsWithinAPCap = _request.availibleAP != -1 && _neighbor.PathAPScore <= _request.availibleAP;

                        if (_v/* && _pathIsWithinAPCap*/)
                        {
                            _neighbor.gCost = _newMovementCostToNeighbor;
                            _neighbor.hCost = GetDistance(_neighbor, _targetNode);
                            _neighbor.parent = _currentNode;

                            if (!_openSet.Contains(_neighbor))
                            {
                                _openSet.Add(_neighbor);
                            }
                            else
                            {
                                _openSet.UpdateItem(_neighbor);
                            }
                        }
                    }
                }
            }

            if (_pathSuccess)
            {
                _waypoints = RetracePath(_startNode, _targetNode, out _usedAP);
            }

            _callback(new PathResult(_waypoints, _pathSuccess, _request.callback, _usedAP));
        }

        // RetracePath aggregates a path by starting at the end node and following the parent nodes back until it reaches the original node.
        Vector3[] RetracePath(Node _startNode, Node _endNode, out int _usedAP)
        {
            _usedAP = 0;

            List<Node> _path = new List<Node>();
            Node _currentNode = _endNode;

            while (_currentNode != _startNode)
            {
                _path.Add(_currentNode);
                _currentNode = _currentNode.parent;
            }
            //Vector3[] _waypoints = SimplifyPath(_path);
            //Array.Reverse(_waypoints);

            List<Vector3> _waypoints = new List<Vector3>();

            for (int i = 0; i < _path.Count; i++)
            {
                _waypoints.Add(_path[i].worldPosition);
                _usedAP += _path[i].apCost;
            }
            _waypoints.Reverse();

            return _waypoints.ToArray();
        }

        // SimplifyPath takes in a list of Nodes and removes the points that align with the angle of the previous 2 points.
        // For a grid it seems unnecessary but I'm leaving it in for now in case we find a use for it later.
        Vector3[] SimplifyPath(List<Node> _path)
        {
            List<Vector3> _waypoints = new List<Vector3>();
            Vector2 _directionOld = Vector2.zero;

            for (int i = 1; i < _path.Count; i++)
            {
                Vector2 directionNew = new Vector2(_path[i - 1].gridX - _path[i].gridX, _path[i - 1].gridY - _path[i].gridY);
                if (directionNew != _directionOld)
                {
                    _waypoints.Add(_path[i - 1].worldPosition);
                }
                _directionOld = directionNew;
            }
            return _waypoints.ToArray();
        }

        int GetDistance(Node _nodeA, Node _nodeB)
        {
            int _dstX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
            int _dstY = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

            // The Distance Formula: 14l + 10(g-l); g = greater number, l = lesser number
            if (_dstX > _dstY)
            {
                return (14 * _dstY) + 10 * (_dstX - _dstY);
            }
            else
            {
                return (14 * _dstX) + 10 * (_dstY - _dstX);
            }
        }
    }
}