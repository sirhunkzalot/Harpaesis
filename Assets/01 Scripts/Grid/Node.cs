using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.GridAndPathfinding
{
    public class Node : IHeapItem<Node>
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX, gridY;
        public int movementPenalty;

        public int gCost, hCost;
        public int FCost { get { return gCost + hCost; } }

        public int apCost = 1;

        public Bounds bounds;

        public Node parent;
        int heapIndex;

        public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY, float _nodeSize, int _movementPenalty, int _apCost = 1)
        {
            walkable = _walkable;
            worldPosition = _worldPosition;

            gridX = _gridX;
            gridY = _gridY;

            movementPenalty = _movementPenalty;
            apCost = _apCost;

            bounds = new Bounds(_worldPosition, Vector3.one * _nodeSize);
        }

        public void UpdateNodeData(int _movementPenalty, int _apCost)
        {
            if(movementPenalty >= 0)
            {
                movementPenalty = _movementPenalty;
            }
            if(apCost >= 0)
            {
                apCost = _apCost;
            }
        }

        public int PathAPScore
        {
            get
            {
                int _ap = apCost;

                Node _parent = parent;

                while (_parent != null)
                {
                    _ap += _parent.apCost;
                    _parent = _parent.parent;
                }

                return _ap;
            }
        }

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(Node _nodeToCompare)
        {
            int _compare = FCost.CompareTo(_nodeToCompare.FCost);
            if(_compare == 0)
            {
                _compare = hCost.CompareTo(_nodeToCompare.hCost);
            }

            return -_compare;
        }
    }
}