using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridAndPathfinding
{
    /**
     * @author Matthew Sommer
     * class Pathfinder generates paths from one node to another based on received requests */
    public class Pathfinder : MonoBehaviour
    {
        GridManager grid;

        private void Awake()
        {
            grid = GetComponent<GridManager>();
        }

        public void FindPath(PathRequest _request, Action<PathResult> _callback)
        {
            Waypoint[] _waypoints = new Waypoint[0];
            bool _pathSuccess = false;

            Node _startNode = grid.NodeFromWorldPoint(_request.pathStart);
            Node _targetNode = grid.NodeFromWorldPoint(_request.pathEnd);

            if (_startNode.walkable && _targetNode.walkable)
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

                        bool _isValidAndHasSmallerGCost = _newMovementCostToNeighbor < _neighbor.gCost || !_openSet.Contains(_neighbor);

                        if (_isValidAndHasSmallerGCost)
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
                _waypoints = RetracePath(_startNode, _targetNode);
            }

            _callback(new PathResult(_waypoints, _pathSuccess, _request.callback, _request.unit));
        }

        // RetracePath aggregates a path by starting at the end node and following the parent nodes back until it reaches the original node.
        Waypoint[] RetracePath(Node _startNode, Node _endNode)
        {
            List<Waypoint> _waypoints = new List<Waypoint>();
            Node _currentNode = _endNode;

            while (_currentNode != _startNode)
            {
                _waypoints.Add(new Waypoint(_currentNode.worldPosition));

                Vector2 _currentGridPosition = new Vector2(_currentNode.gridX, _currentNode.gridY);
                Vector2 _parentGridPosition = new Vector2(_currentNode.parent.gridX, _currentNode.parent.gridY);


                bool _isDiagonalToParent = Mathf.Abs(_currentGridPosition.x - _parentGridPosition.x) + Mathf.Abs(_currentGridPosition.y - _parentGridPosition.y) > 1;

                if (_isDiagonalToParent)
                {
                    Vector2 _direction = _parentGridPosition - _currentGridPosition;

                    Node _node1 = grid.RetrieveNode((int)_currentGridPosition.x + (int)_direction.x, (int)_currentGridPosition.y);
                    Node _node2 = grid.RetrieveNode((int)_currentGridPosition.x, (int)_currentGridPosition.y + (int)_direction.y);

                    if (_node1.walkable && !_node2.walkable)
                    {
                        _waypoints.Add(new Waypoint(_node1.worldPosition, 0));
                    }
                    else if (_node2.walkable && !_node1.walkable)
                    {
                        _waypoints.Add(new Waypoint(_node2.worldPosition, 0));
                    }
                    else if (!_node1.walkable && !_node2.walkable)
                    {
                        throw new Exception("Node Error: Neither diagonal node is walkable. Either it was" +
                            " incorrectly determined to be a diagonal movement, or another error was created.");
                    }
                }

                _currentNode = _currentNode.parent;
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