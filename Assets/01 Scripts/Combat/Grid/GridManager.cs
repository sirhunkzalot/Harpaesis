using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.GridAndPathfinding
{
    /**
     * @author Matthew Sommer
     * class GridManager sets up the logic that generates and enables other scripts
     * to utilize grid logic */

    [RequireComponent(typeof(Pathfinder))]
    [RequireComponent(typeof(PathRequestManager))]
    public class GridManager : MonoBehaviour
    {
        public LayerMask unwalkableMask;
        public Vector2 gridWorldSize = new Vector2(10, 10);
        public float nodeSize = 1f;
        public TerrainType[] walkableRegions;
        LayerMask walkableMask;
        Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

        public Vector3 gridOffset;

        Node[,] grid;

        public bool displayGridGizmos;
        public bool diagonalMovement;

        [HideInInspector] public float nodeRadius;
        [HideInInspector] public int gridSizeX, gridSizeY;
        public int MaxSize { get { return gridSizeX * gridSizeY; } }

        public static GridManager instance;

        private void Awake()
        {
            Application.targetFrameRate = 120;

            instance = this;

            nodeRadius = nodeSize / 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeSize);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeSize);

            foreach (TerrainType region in walkableRegions)
            {
                walkableMask.value |= region.terrainMask.value;
                walkableRegionsDictionary.Add(Mathf.RoundToInt(Mathf.Log(region.terrainMask.value, 2)), region.terrainPenalty);
            }

            CreateGrid();
        }

        /* CreateGrid generates a series of nodes based on the settings given */
        void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];

            Vector3 _bottomLeftCorner = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = _bottomLeftCorner + Vector3.right * (x * nodeSize + nodeRadius) + Vector3.forward * (y * nodeSize + nodeRadius);

                    bool _walkable = WorldPointIsWalkable(worldPoint);

                    int _movementPenalty = 0;

                    if (_walkable)
                    {
                        Ray _ray = new Ray(worldPoint + Vector3.up, Vector3.down);
                        RaycastHit _hit;

                        if(Physics.Raycast(_ray, out _hit, 1.5f, walkableMask))
                        {
                            walkableRegionsDictionary.TryGetValue(_hit.collider.gameObject.layer, out _movementPenalty);
                        }
                    }

                    grid[x, y] = new Node(_walkable, worldPoint, x, y, nodeSize, _movementPenalty);
                }
            }
        }
       
        /* GetNeighbors finds all of the nodes surrounding the given node
         * @param _node is the node to search around
         * @return a list of the viable surrounding nodes for pathfinding */
        public List<Node> GetNeighbors(Node _node)
        {
            List<Node> _neighbors = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    bool _isDiagonal = Mathf.Abs(x) + Mathf.Abs(y) > 1;

                    // Skips Current Node and Diagonal Nodes if told to skip them
                    if (x == 0 && y == 0 || !diagonalMovement && _isDiagonal)
                    {
                        continue;
                    }
                    else if (_isDiagonal)
                    {
                        int _x = _node.gridX;
                        int _y = _node.gridY;

                        bool _nodeIsOutsideGrid = _x + x < 0 || _x + x >= gridWorldSize.x || _y + y < 0 || _y + y >= gridWorldSize.y;

                        if(_nodeIsOutsideGrid || !grid[_x + x, _y].walkable && !grid[_x, _y + y].walkable)
                        {
                            continue;
                        }
                    }

                    // Gets Node
                    int _checkX = _node.gridX + x;
                    int _checkY = _node.gridY + y;

                    if(_checkX >= 0 && _checkX < gridSizeX && _checkY >= 0 && _checkY < gridSizeY)
                    {
                        _neighbors.Add(grid[_checkX, _checkY]);
                    }
                }
            }

            return _neighbors;
        }

        /* GetNeighborsRaw returns a list containing the given node and all of its neighbors
         * @param _node is the node to search around
         * @return a list of nodes containing the given node and the 8 nodes surrounding it */
        public List<Node> GetNeighborsRaw(Node _node, bool _unoccupiedNeighborsOnly = false)
        {
            List<Node> _neighbors = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Gets Node
                    int _checkX = _node.gridX + x;
                    int _checkY = _node.gridY + y;

                    if (_checkX >= 0 && _checkX < gridSizeX && _checkY >= 0 && _checkY < gridSizeY)
                    {
                        Node _newNode = grid[_checkX, _checkY];

                        if (!(_unoccupiedNeighborsOnly && _newNode.hasUnit))
                        {
                            _neighbors.Add(_newNode);
                        }
                    }
                }
            }

            return _neighbors;
        }

        /* RetrieveNode retrieves a node from the grid based on its x and y position within the grid
         * @param _x is the x position of the requested node
         * @param _yis the y position of the requested node
         * @return the node at the given grid position or returns null if node doesn't exist*/
        public Node RetrieveNode(int _x, int _y)
        {
            if (_x >= 0 && _x < gridSizeX && _y >= 0 && _y < gridSizeY)
            {
                return grid[_x, _y];
            }

            return null;
        }

        /* GetClosestNode finds the closest node to the given position amongst a list of nodes
         * @param _worldPosition is the position to find the closest node to
         * @param _nodes is the list of nodes to check
         * @return the closest node to the given position from the given list of nodes */
        public Node GetClosestNode(Vector3 _worldPosition, List<Node> _nodes)
        {
            Node _closestNode = null;
            float _distance = float.MaxValue;

            foreach (Node _node in _nodes)
            {
                float _newDis = (_node.worldPosition - _worldPosition).sqrMagnitude;

                if (_newDis < _distance)
                {
                    _distance = _newDis;
                    _closestNode = _node;
                }
            }

            return _closestNode;
        }

        /* NodeFromWorldPoint uses a world position to roughly get the closest node
         * @param Vector3 _worldPosition is the world position to find a node with
         * @return the node that is near given world position */
        public Node NodeFromWorldPoint(Vector3 _worldPosition)
        {
            float _percentX = Mathf.Clamp01((_worldPosition.x + (gridWorldSize.x / 2)) / gridWorldSize.x);
            float _percentY = Mathf.Clamp01((_worldPosition.z + (gridWorldSize.y / 2)) / gridWorldSize.y);

            int _x = Mathf.RoundToInt((gridSizeX - 1) * _percentX);
            int _y = Mathf.RoundToInt((gridSizeY - 1) * _percentY);

            return grid[_x, _y];
        }

        /* NodePositionFromWorldPoint accurately gets the world position of the nearest node to the given point
         * @param Vector3 _worldPosition is the world position to find a node with
         * @return the world position of the node closest to the given world position */
        public Vector3 NodePositionFromWorldPoint(Vector3 _worldPosition, bool _unoccupiedPositionsOnly = true)
        {
            // Gets a rough estimate of the node's position
            Node _node = NodeFromWorldPoint(_worldPosition);

            // Gets the node and it's neighbors
            List<Node> _neighbors = GetNeighborsRaw(_node, _unoccupiedPositionsOnly);

            return GetClosestNode(_worldPosition, _neighbors).worldPosition;
        }

        public bool WorldPointIsWalkable(Vector3 _worldPosition)
        {
            return !Physics.CheckSphere(_worldPosition, nodeRadius * .95f, unwalkableMask);
        }

        public bool NodeIsWalkable(Node _node)
        {
            return !Physics.CheckSphere(_node.worldPosition, nodeRadius * .95f, unwalkableMask);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if(grid != null && displayGridGizmos)
            {
                Vector3 _gizmoSize;

                foreach (Node node in grid)
                {
                    Gizmos.color = (node.walkable) ? Color.white : Color.red;
                    _gizmoSize = Vector3.one * (nodeSize * .9f);
                    _gizmoSize.y *= (node.walkable) ? .2f: 2;

                    Gizmos.DrawCube(node.worldPosition, _gizmoSize);
                }
            }
        }

        public bool LinecastToWorldPoint(Vector3 _startPosition, Vector3 _worldPosition)
        {
            Vector3 _origin = _startPosition + Vector3.up;
            Vector3 _targetPosition = _worldPosition + Vector3.up;

            return Physics.Linecast(_origin, _targetPosition, unwalkableMask);
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public string terrainType;
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}