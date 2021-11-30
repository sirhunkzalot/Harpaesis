using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotInEditor : MonoBehaviour
{
    private void Awake()
    {
        if (!Application.isEditor)
        {
            Destroy(gameObject);
        }
    }
}
