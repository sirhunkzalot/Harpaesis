using System.Collections;
using System.Collections.Generic;
using Harpaesis.GridAndPathfinding;
using Harpaesis.Combat;
using UnityEngine;

/**
 * @author Matthew Sommer
 * class EnemyUnit handles the logic that pertains to every enemy unit */
public class EnemyUnit : Unit
{
    [HideInInspector] public EnemyUnitData enemyUnitData;

    [SerializeField, ReadOnly] FriendlyUnit currentTarget;

    List<EnemyMove> validMoves = new List<EnemyMove>();

    TurnManager turnManager;

    public LayerMask viewObstructionMask;

    List<FriendlyUnit> friendlyUnits = new List<FriendlyUnit>();
    List<FriendlyUnit> ignoreUnits = new List<FriendlyUnit>();


    protected override void Init()
    {
        enemyUnitData = (EnemyUnitData)unitData;
        turnManager = TurnManager.instance;
    }

    public override void StartTurn()
    {
        base.StartTurn();
        uiCombat.ShowPlayerUI(false);
        gridCam.followUnit = this;
        friendlyUnits = turnManager.friendlyUnits;
        ignoreUnits = new List<FriendlyUnit>();
        validMoves = new List<EnemyMove>();

        PlanTurn();
    }

    void PlanTurn()
    {
        for (int i = 0; i < friendlyUnits.Count; i++)
        {
            Vector3 _myPos = grid.NodePositionFromWorldPoint(transform.position, false);
            Vector3 _targetPos = grid.NodePositionFromWorldPoint(friendlyUnits[i].transform.position, true);

            PathRequestManager.RequestPath(new PathRequest(_myPos, _targetPos, CreateEnemyMoves, friendlyUnits[i]));

            CreateEnemyMoves(friendlyUnits[i], transform.position, enemyUnitData.apStat);
        }

        Invoke(nameof(ChooseMove), 1f);
    }

    void CreateEnemyMoves(FriendlyUnit _unit, Vector3 _position, int _apAtPosition, PathResult _results = null, int _pathPositionIndex = 0)
    {
        if (!Physics.Linecast(_position + Vector3.up, _unit.transform.position + Vector3.up, viewObstructionMask))
        {
            foreach (EnemySkill _skill in enemyUnitData.enemySkills)
            {
                if (_skill.skill.apCost <= _apAtPosition && _skill.rangeEstimate >= Mathf.RoundToInt(Vector3.Distance(_unit.transform.position, transform.position)))
                {
                    validMoves.Add(new EnemyMove(_position, _pathPositionIndex, _skill, _unit, _results));
                }
            }
        }
    }

    void CreateEnemyMoves(PathResult _result)
    {
        if (_result.success == false)
        {
            //print("Failed.");
            return;
        }

        FriendlyUnit _unit = (FriendlyUnit)_result.unit;

        for (int i = 0; i < _result.pathLength; i++)
        {
            int _apAtPosition = enemyUnitData.apStat - i;
            CreateEnemyMoves(_unit, _result.path[i].position, _apAtPosition, _result, i);
        }
    }

    void ChooseMove()
    {
        if (validMoves.Count == 0)
        {
            CreateMoveOnlyTurn();
            return;
        }

        Dictionary<EnemyMove, Vector2> _moveWeights = new Dictionary<EnemyMove, Vector2>();

        int _maxWeight = 0;

        int _shortRange = 3;
        int _mediumRange = 5;

        for (int i = 0; i < validMoves.Count; i++)
        {
            int _weight = 0;

            int _dis = Mathf.RoundToInt(Vector3.Distance(validMoves[i].movePosition, validMoves[i].target.transform.position));

            if (_dis <= _shortRange)
            {
                _weight += validMoves[i].skillToUse.shortRangeWeight;
            }
            else if (_dis <= _mediumRange)
            {
                _weight += validMoves[i].skillToUse.mediumRangeWeight;
            }
            else // Defaults to Long Range
            {
                _weight += validMoves[i].skillToUse.longRangeWeight;
            }

            _weight += Mathf.RoundToInt((validMoves[i].skillToUse.highHPTargetWeight * .01f) * validMoves[i].target.currentHP);
            _weight += Mathf.RoundToInt((validMoves[i].skillToUse.lowHPTargetWeight / 2) / validMoves[i].target.currentHP);

            _maxWeight += _weight;

            if (i == 0)
            {
                _moveWeights.Add(validMoves[i], new Vector2(1, _maxWeight));
            }
            else
            {
                _moveWeights.Add(validMoves[i], new Vector2(_moveWeights[validMoves[i - 1]].y + 1, _maxWeight));
            }

        }

        int _chosenValue = Random.Range(1, _maxWeight + 1);

        foreach (KeyValuePair<EnemyMove, Vector2> _moveWeightRanges in _moveWeights)
        {
            if (_chosenValue >= _moveWeightRanges.Value.x && _chosenValue <= _moveWeightRanges.Value.y)
            {
                StartCoroutine(ExecuteTurn(_moveWeightRanges.Key));
                return;
            }
        }

        // Fallback if a move is not chosen for whatever reason
        Debug.LogError($"Error: No move was selected for {enemyUnitData.unitName}. Moving Unit instead.");
        CreateMoveOnlyTurn();
    }

    void CreateMoveOnlyTurn()
    {
        float _closestDis = float.MaxValue;
        int _closestUnitIndex = -1;

        for (int i = 0; i < friendlyUnits.Count; i++)
        {
            if (ignoreUnits.Count == 0 && ignoreUnits.Contains(friendlyUnits[i]))
            {
                continue;
            }

            float _unitDis = Vector3.Distance(transform.position, friendlyUnits[i].transform.position);

            if (_unitDis < _closestDis)
            {
                _closestUnitIndex = i;
                _closestDis = _unitDis;
            }
        }

        if(_closestUnitIndex == -1)
        {
            Debug.LogError("Fuuuuuuuuuck");
            EndTurn();
            return;
        }

        Vector3 _myPos = grid.NodePositionFromWorldPoint(transform.position, false);
        Vector3 _targetPos = grid.NodePositionFromWorldPoint(friendlyUnits[_closestUnitIndex].transform.position, true);

        PathRequestManager.RequestPath(new PathRequest(_myPos, _targetPos, MoveOnlyPathResult, friendlyUnits[_closestUnitIndex]));
    }

    void MoveOnlyPathResult(PathResult _result)
    {
        if (_result.success)
        {
            StartCoroutine(MoveOnlyTurn(_result.path));
        }
        else
        {
            ignoreUnits.Add((FriendlyUnit)_result.unit);
            CreateMoveOnlyTurn();
        }
    }

    public IEnumerator ExecuteTurn(EnemyMove _move)
    {
        if(_move.result != null && _move.movePosition != transform.position)
        {
            Waypoint[] _waypoints = new Waypoint[_move.pathPositionIndex + 1];

            for (int i = 0; i < _waypoints.Length; i++)
            {
                _waypoints[i] = _move.result.path[i];
            }

            motor.Move(_waypoints);

            do
            {
                yield return new WaitForEndOfFrame();
            } while (motor.isMoving);
        }

        if(_move.skillToUse != null)
        {
            _move.skillToUse.skill.UseSkill(this, _move.target);
        }

        Invoke(nameof(EndTurn), .5f);
    }

    public IEnumerator MoveOnlyTurn(Waypoint[] _path)
    {
        motor.Move(_path);
        do
        {
            yield return new WaitForEndOfFrame();
        } while (motor.isMoving);

        Invoke(nameof(EndTurn), .5f);
    }

    void EndTurn()
    {
        TurnManager.instance.NextTurn();
    }
}

public struct EnemyMove
{
    public Vector3 movePosition;
    public int pathPositionIndex;
    public EnemySkill skillToUse;
    public FriendlyUnit target;
    public PathResult result;

    public EnemyMove(Vector3 _movePosition, int _pathPositionIndex, EnemySkill _skillToUse, FriendlyUnit _target, PathResult _result)
    {
        movePosition = _movePosition;
        pathPositionIndex = _pathPositionIndex;
        skillToUse = _skillToUse;
        target = _target;
        result = _result;
    }
}