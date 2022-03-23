using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovSpawner : MonoBehaviour
{
    public GameObject fireTilePrefab;

    public void Init(List<Vector3> _positions)
    {
        Dictionary<FireTile, float> _fireTiles = new Dictionary<FireTile, float>();
        float _furthestDistance = 0f;

        for (int i = 0; i < _positions.Count; i++)
        {
            FireTile _tile = Instantiate(fireTilePrefab, _positions[i], Quaternion.identity).GetComponent<FireTile>();
            float _distance = Vector3.Distance(transform.position, _tile.transform.position);

            _fireTiles.Add(_tile, _distance);

            if(_distance > _furthestDistance)
            {
                _furthestDistance = _distance;
            }
        }

        foreach (KeyValuePair<FireTile, float> _tileData in _fireTiles)
        {
            int _fireState = 1 + Mathf.RoundToInt((2f * Mathf.InverseLerp(0, _furthestDistance, _tileData.Value)));

            //_tileData.Key.Init(_fireState);
        }

        Destroy(gameObject);
    }
}
