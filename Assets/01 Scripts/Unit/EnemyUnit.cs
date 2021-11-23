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

    [SerializeField, ReadOnly] PathResult results = null;
    [SerializeField, ReadOnly] Unit currentTarget;

    FriendlyUnit[] friendlyUnits;

    public LayerMask viewObstructionMask;

    protected override void Init()
    {
        enemyUnitData = (EnemyUnitData)unitData;
    }

    public override void StartTurn()
    {
        base.StartTurn();

        uiCombat.IsPlayerTurn(false);
        gridCam.followUnit = this;
        results = null;

        friendlyUnits = FindObjectsOfType<FriendlyUnit>();
        GetUnitDistances();
        Invoke(nameof(PlanTurn), 1f);
    }

    void GetUnitDistances()
    {
        foreach (FriendlyUnit unit in friendlyUnits)
        {
            Vector3 _dir = (transform.position - unit.transform.position).normalized;
            Vector3 _targetPosition = grid.NodePositionFromWorldPoint(unit.transform.position);
            PathRequestManager.RequestPath(new PathRequest(transform.position, _targetPosition, HandlePathResults, unit));
        }
    }

    public void HandlePathResults(PathResult _result)
    {
        if(results == null || _result.pathLength < results.pathLength)
        {
            results = _result;
            currentTarget = results.unit;
        }
    }

    public void PlanTurn()
    {
        List<EnemyMove> _validMoves = new List<EnemyMove>();

        Dictionary<int,Vector3> _validPositionsWithVision = new Dictionary<int, Vector3>();

        for (int i = 0; i < results.pathLength; i++)
        {
            if(!Physics.Linecast(results.path[i].position + Vector3.up, currentTarget.transform.position + Vector3.up, viewObstructionMask))
            {
                _validPositionsWithVision.Add(i, results.path[i].position);
            }
        }

        int _shortRange = (canMove) ? 3 + enemyUnitData.apStat : 3;
        int _mediumRange = (canMove) ? 5 + enemyUnitData.apStat : 5;

        foreach (KeyValuePair<int, Vector3> _point in _validPositionsWithVision)
        {
            int _apAtPoint = enemyUnitData.apStat - _point.Key;
            int _distanceFromTarget = Mathf.RoundToInt(Vector3.Distance(_point.Value, currentTarget.transform.position));

            for (int i = 0; i < enemyUnitData.enemySkills.Length; i++)
            {
                EnemySkill _skill = enemyUnitData.enemySkills[i];
                bool _canUseMoveAtPoint = (_skill.rangeEstimate <= _distanceFromTarget) && (_skill.skill.apCost <= _apAtPoint);

                if (_canUseMoveAtPoint)
                {
                    _validMoves.Add(new EnemyMove(_point.Value, _point.Key, _skill));
                }
            }
        }


        if (_validMoves.Count == 0)
        {
            if(results.pathLength == 0)
            {
                EndTurn();
                return;
            }
            ExecuteTurn(new EnemyMove(results.path[results.path.Length - 1].position, enemyUnitData.apStat - 1, null));
        }
        else if(_validMoves.Count == 1)
        {
            ExecuteTurn(_validMoves[0]);
        }
        else
        {
            int _maxWeight = 0;
            Dictionary<EnemyMove, Vector2> _weightRanges = new Dictionary<EnemyMove, Vector2>();

            for (int i = 0; i < _validMoves.Count; i++)
            {
                int _dis = Mathf.RoundToInt(Vector3.Distance(_validMoves[i].movePosition, currentTarget.transform.position));


               _maxWeight += (_dis <= _shortRange) ? _validMoves[i].skillToUse.shortRangeWeight :
                    (_dis <= _mediumRange) ? _validMoves[i].skillToUse.mediumRangeWeight : _validMoves[i].skillToUse.longRangeWeight;

                if(i == 0)
                {
                    _weightRanges.Add(_validMoves[i], new Vector2(1, _maxWeight));
                }
                else
                {
                    _weightRanges.Add(_validMoves[i], new Vector2(_weightRanges[_validMoves[i - 1]].y + 1, _maxWeight));
                }
            }

            int _chosenValue = Random.Range(1, _maxWeight + 1);

            foreach (KeyValuePair<EnemyMove,Vector2> _moveWeightRanges in _weightRanges)
            {
                if(_chosenValue >= _moveWeightRanges.Value.x && _chosenValue <= _moveWeightRanges.Value.y)
                {
                    StartCoroutine(ExecuteTurn(_moveWeightRanges.Key));
                    return;
                }
            }
        }
    }

    public IEnumerator ExecuteTurn(EnemyMove _move)
    {
        if(_move.movePosition != transform.position)
        {
            Waypoint[] _waypoints = new Waypoint[_move.pathPositionIndex];

            for (int i = 0; i < _waypoints.Length; i++)
            {
                _waypoints[i] = results.path[i];
            }

            motor.Move(_waypoints);

            do
            {
                yield return new WaitForEndOfFrame();
            } while (motor.isMoving);
        }

        if(_move.skillToUse != null)
        {
            _move.skillToUse.skill.UseSkill(this, currentTarget);
        }

        EndTurn();
    }

    void EndTurn()
    {
        results = null;
        currentTarget = null;
        TurnManager.instance.NextTurn();
    }
}

public struct EnemyMove
{
    public Vector3 movePosition;
    public int pathPositionIndex;
    public EnemySkill skillToUse;

    public EnemyMove(Vector3 _movePosition, int _pathPositionIndex, EnemySkill _skillToUse)
    {
        movePosition = _movePosition;
        pathPositionIndex = _pathPositionIndex;
        skillToUse = _skillToUse;
    }
}