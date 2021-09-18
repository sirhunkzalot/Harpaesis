using System.Collections;
using System.Collections.Generic;
using GridAndPathfinding;
using UnityEngine;

[RequireComponent(typeof(UnitMotor))]
public class Unit : MonoBehaviour
{
    public UnitData unitData;
    protected UnitMotor motor;

    protected Vector3[] previewPath;
    public bool hasPath;

    public int currentHP;
    public bool isAlive = true;

    public Turn turnData;

    protected GridManager grid;


    private void Start()
    {
        grid = GridManager.instance;
        motor = GetComponent<UnitMotor>();
        motor.Init(this);
        Init();
    }

    private void Update()
    {
        Tick();
    }

    protected virtual void Init() { }
    protected virtual void Tick() { }

    public void PreviewPath(PathResult _result)
    {
        if (_result.success)
        {
            previewPath = _result.path;
            PathRenderer.instance.Activate(_result);
        }
    }

    public void TakeDamage(int _damageAmount)
    {
        currentHP = Mathf.Clamp(currentHP -_damageAmount, 0, unitData.healthStat);

        isAlive = (currentHP == 0) ? false : isAlive;
    }
}
