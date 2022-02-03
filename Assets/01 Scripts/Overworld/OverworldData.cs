using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public static class OverworldData
    {
        public static float lastPointIndex = float.MinValue;
        public static PointData lastPointData;

        public static Dictionary<float, PointData> overworldPoints = new Dictionary<float, PointData>();

        public static void OverridePointData(float _index, bool _isUnlocked)
        {
            overworldPoints[_index].isUnlocked = _isUnlocked;
        }

        public static void OverworldPointInit(float _index, PointData _pointData)
        {
            if (overworldPoints.ContainsKey(_index))
            {
                _pointData.point.isUnlocked = (_pointData.point.isUnlocked) ? _pointData.point.isUnlocked : overworldPoints[_index].isUnlocked;
                overworldPoints[_index].UpdatePoint(_pointData.point);

                if(_index == lastPointIndex)
                {
                    if (lastPointData.isCompleted)
                    {
                        _pointData.point.CompletePoint();
                    }

                    OverworldController.instance.Init(_pointData.point);
                }
            }
            else
            {
                overworldPoints[_index] = _pointData;
            }
        }

        public static void CompleteCurrentPoint()
        {
            lastPointData.Complete();
        }

        public static void SetLastPoint(float _index)
        {
            if (overworldPoints.ContainsKey(_index))
            {
                lastPointIndex = _index;
                lastPointData = overworldPoints[_index];
            }
        }
    }

    public class PointData
    {
        public OverworldPoint point;
        public bool isUnlocked;
        public bool isCompleted;

        public PointData(OverworldPoint _point, bool _isUnlocked)
        {
            point = _point;
            isUnlocked = _isUnlocked;
            isCompleted = false;
        }

        public void UpdatePoint(OverworldPoint _point)
        {
            point = _point;
        }

        public void UpdateUnlockedStatus(bool _isUnlocked)
        {
            isUnlocked = _isUnlocked;
        }

        public void Complete()
        {
            isCompleted = true;
        }
    }
}