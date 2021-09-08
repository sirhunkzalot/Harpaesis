using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridAndPathfinding
{
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

        Node[,] grid;

        public bool displayGridGizmos;
        public bool diagonalMovement;

        float nodeRadius;
        int gridSizeX, gridSizeY;
        public int MaxSize { get { return gridSizeX * gridSizeY; } }

        private void Awake()
        {
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

        void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];

            Vector3 _bottomLeftCorner = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = _bottomLeftCorner + Vector3.right * (x * nodeSize + nodeRadius) + Vector3.forward * (y * nodeSize + nodeRadius);

                    bool _walkable = !Physics.CheckSphere(worldPoint, nodeRadius * .95f, unwalkableMask);

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

        /*public List<Node> GetListOfReachableNodes(Vector3 _unitPosition, int _availibleAP)
        {
            List<Node> _nodes = new List<Node>();

            GridPosition _gridPosition = GridPositionFromWorldPoint(_unitPosition);

            int _xMin = Mathf.Clamp(_gridPosition.x - _availibleAP, 0, gridSizeX - 1);
            int _xMax = Mathf.Clamp(_gridPosition.x + _availibleAP, 0, gridSizeX - 1);

            int _yMin = Mathf.Clamp(_gridPosition.y - _availibleAP, 0, gridSizeY - 1);
            int _yMax = Mathf.Clamp(_gridPosition.y + _availibleAP, 0, gridSizeY - 1);

            List<GridPosition> _possiblePositions = new List<GridPosition>();

            for (int x = _xMin; x <= _xMax; x++)
            {
                for (int y = _yMin; y <= _yMax; y++)
                {
                    _possiblePositions.Add(new GridPosition(x, y));
                }
            }

            for (int i = 0; i < _possiblePositions.Count; i++)
            {
                _nodes.Add(grid[_possiblePositions[i].x, _possiblePositions[i].y]);
            }

            for (int i = 0; i < length; i++)
            {

            }


            return _nodes;
        }
        */
        public List<Node> GetNeighbors(Node _node)
        {
            List<Node> _neighbors = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Skips Current Node and Diagonal Nodes if told to skip them
                    if(x == 0 && y == 0 || !diagonalMovement && Mathf.Abs(x) + Mathf.Abs(y) > 1)
                    {
                        continue;
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

        public Node NodeFromWorldPoint(Vector3 _worldPosition)
        {
            float _percentX = Mathf.Clamp01((_worldPosition.x + (gridWorldSize.x / 2)) / gridWorldSize.x);
            float _percentY = Mathf.Clamp01((_worldPosition.z + (gridWorldSize.y / 2)) / gridWorldSize.y);

            int _x = Mathf.RoundToInt((gridSizeX - 1) * _percentX);
            int _y = Mathf.RoundToInt((gridSizeY - 1) * _percentY);

            return grid[_x, _y];
        }

        public Vector3 NodePositionFromWorldPoint(Vector3 _worldPosition)
        {
            float _percentX = Mathf.Clamp01((_worldPosition.x + (gridWorldSize.x / 2)) / gridWorldSize.x);
            float _percentY = Mathf.Clamp01((_worldPosition.z + (gridWorldSize.y / 2)) / gridWorldSize.y);

            int _x = Mathf.RoundToInt((gridSizeX - 1) * _percentX);
            int _y = Mathf.RoundToInt((gridSizeY - 1) * _percentY);

            return grid[_x, _y].worldPosition;
        }

        /*GridPosition GridPositionFromWorldPoint(Vector3 _worldPosition)
        {
            float _percentX = Mathf.Clamp01((_worldPosition.x + (gridWorldSize.x / 2)) / gridWorldSize.x);
            float _percentY = Mathf.Clamp01((_worldPosition.z + (gridWorldSize.y / 2)) / gridWorldSize.y);

            int _x = Mathf.RoundToInt((gridSizeX - 1) * _percentX);
            int _y = Mathf.RoundToInt((gridSizeY - 1) * _percentY);

            return new GridPosition(_x, _y);
        }*/

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
    }

    [System.Serializable]
    public class TerrainType
    {
        public string terrainType;
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

    /*struct GridPosition
    {
        public int x;
        public int y;

        public GridPosition(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }*/
}