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

    [ReadOnly] public EnemyUnitPassive passive;

    List<Unit> units = new List<Unit>();

    List<FriendlyUnit> friendlyUnits = new List<FriendlyUnit>();
    List<FriendlyUnit> ignoreFriendlyUnits = new List<FriendlyUnit>();

    List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
    List<EnemyUnit> ignoreEnemyUnits = new List<EnemyUnit>();

    public bool allegianceChanged;


    protected override void Init()
    {
        enemyUnitData = (EnemyUnitData)unitData;
        turnManager = TurnManager.instance;

        passive = (EnemyUnitPassive)unitPassive;
        passive.OnCombatStart();
    }
    public override void StartTurn()
    {
        uiCombat.ShowPlayerUI(false);
        gridCam.followUnit = this;

        units = turnManager.units;

        friendlyUnits = turnManager.friendlyUnits;
        ignoreFriendlyUnits = new List<FriendlyUnit>();

        enemyUnits = turnManager.enemyUnits;
        ignoreEnemyUnits = new List<EnemyUnit>();

        validMoves = new List<EnemyMove>();

        //print("Start Turn");

        canMove = !HasEffect(StatusEffectType.Root);

        PlanTurn();
    }
    void PlanTurn()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (canMove)
            {
                //print("Generate moves Involving Movement");
                Vector3 _myPos = grid.NodePositionFromWorldPoint(transform.position, false);
                Vector3 _targetPos = grid.NodePositionFromWorldPoint(units[i].transform.position, true);

                PathRequestManager.RequestPath(new PathRequest(_myPos, _targetPos, CreateEnemyMovesWithPath, units[i]));
            }

            //print("Generate moves without Movement");
            CreateEnemyMoves(units[i], transform.position, enemyUnitData.apStat);
        }

        Invoke(nameof(ChooseMove), 1f);
    }
    void CreateEnemyMoves(Unit _unit, Vector3 _position, int _apAtPosition, PathResult _results = null, int _pathPositionIndex = 0)
    {
        if (!Physics.Linecast(_position + Vector3.up, _unit.transform.position + Vector3.up, viewObstructionMask))
        {
            foreach (EnemySkill _skill in enemyUnitData.enemySkills)
            {
                if (_unit != this)
                {
                    bool _isFriendly = _unit.GetType() == typeof(FriendlyUnit);

                    bool _skillTargetMatch =
                        (_isFriendly && _skill.skill.validTargets == TargetMask.Enemy && !allegianceChanged) ||
                        (!_isFriendly && _skill.skill.validTargets == TargetMask.Ally && !allegianceChanged) ||
                        (_isFriendly && _skill.skill.validTargets == TargetMask.Ally && allegianceChanged) ||
                        (!_isFriendly && _skill.skill.validTargets == TargetMask.Enemy && allegianceChanged);


                    if (_skillTargetMatch && _skill.skill.apCost <= _apAtPosition && _skill.rangeEstimate >= Mathf.RoundToInt(Vector3.Distance(_unit.transform.position, transform.position)))
                    {
                        validMoves.Add(new EnemyMove(_position, _pathPositionIndex, _skill, _unit, _results));
                    }
                }
                else
                {
                    if(_skill.skill.validTargets == TargetMask.Self)
                    {
                        validMoves.Add(new EnemyMove(_position, _pathPositionIndex, _skill, _unit, _results));
                    }
                }
            }
        }
    }
    void CreateEnemyMovesWithPath(PathResult _result)
    {
        if (_result.success == false)
        {
            //print("Path was not sucessful");
            return;
        }

        for (int i = 0; i < _result.pathLength; i++)
        {
            //print("Created move with path");
            int _apAtPosition = enemyUnitData.apStat - i;
            CreateEnemyMoves(_result.unit, _result.path[i].position, _apAtPosition, _result, i);
        }
    }
    void ChooseMove()
    {
        if (validMoves.Count == 0)
        {
            //print("No Valid Moves were created");
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
                //print($"First move was weighted between 1 and {_maxWeight}");
                _moveWeights.Add(validMoves[i], new Vector2(1, _maxWeight));
            }
            else
            {
                //print($"Additional move was weighted between {_moveWeights[validMoves[i - 1]].y + 1} and {_maxWeight}");
                _moveWeights.Add(validMoves[i], new Vector2(_moveWeights[validMoves[i - 1]].y + 1, _maxWeight));
            }

        }

        int _chosenValue = Random.Range(1, _maxWeight + 1);
        //print($"Chose Value of: {_chosenValue}.");

        foreach (KeyValuePair<EnemyMove, Vector2> _moveWeightRanges in _moveWeights)
        {
            //print($"Checking if value is between {_moveWeightRanges.Value.x} and {_moveWeightRanges.Value.y}.");
            if (_chosenValue >= _moveWeightRanges.Value.x && _chosenValue <= _moveWeightRanges.Value.y)
            {
                StartCoroutine(ExecuteTurn(_moveWeightRanges.Key));
                return;
            }
            //print("It is not.");
        }

        // Fallback if a move is not chosen for whatever reason
        Debug.LogError($"Error: No move was selected for {enemyUnitData.unitName}. Moving Unit instead.");
        CreateMoveOnlyTurn();
    }
    void CreateMoveOnlyTurn()
    {
        //print("Making a move only turn.");

        if (!canMove)
        {
            //print("Can't Move due to Root");
            Invoke(nameof(EndTurn), .5f);
        }

        float _closestDis = float.MaxValue;
        int _closestUnitIndex = -1;

        for (int i = 0; i < friendlyUnits.Count; i++)
        {
            if (ignoreFriendlyUnits.Count != 0 && ignoreFriendlyUnits.Contains(friendlyUnits[i]))
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
            Debug.Log("No Accessible Friendly Unit");
            Invoke(nameof(EndTurn), .5f);
            return;
        }

        Vector3 _myPos = grid.NodePositionFromWorldPoint(transform.position, false);
        Vector3 _targetPos = grid.NodePositionFromWorldPoint(friendlyUnits[_closestUnitIndex].transform.position, true);

        //print("Requesting path to closest Friendly unit");
        PathRequestManager.RequestPath(new PathRequest(_myPos, _targetPos, MoveOnlyPathResult, friendlyUnits[_closestUnitIndex]));
    }
    void MoveOnlyPathResult(PathResult _result)
    {
        if (_result.success)
        {
            //print("Found Path to closest Friendly Unit!");
            StartCoroutine(MoveOnlyTurn(_result.path));
        }
        else
        {
            //print("No path found to closest friendly Unit.");
            ignoreFriendlyUnits.Add((FriendlyUnit)_result.unit);

            if (ignoreFriendlyUnits.Count < friendlyUnits.Count)
            {
                //print("Ignoring that unit and trying again");
                CreateMoveOnlyTurn();
            }
            else
            {
                //print("No path to nearest friendly unit possible. Trying to move to enemy Unit");
                MoveToNearestEnemyUnit();
            }
        }
    }
    public void MoveToNearestEnemyUnit()
    {
        float _closestDis = float.MaxValue;
        int _closestUnitIndex = -1;

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            if (ignoreEnemyUnits.Count != 0 && ignoreEnemyUnits.Contains(enemyUnits[i]))
            {
                continue;
            }

            float _unitDis = Vector3.Distance(transform.position, enemyUnits[i].transform.position);

            if (_unitDis < _closestDis)
            {
                _closestUnitIndex = i;
                _closestDis = _unitDis;
            }
        }

        Vector3 _myPos = grid.NodePositionFromWorldPoint(transform.position, false);
        Vector3 _targetPos = grid.NodePositionFromWorldPoint(enemyUnits[_closestUnitIndex].transform.position, true);

        //print("Requesting path to closest enemy unit");
        PathRequestManager.RequestPath(new PathRequest(_myPos, _targetPos, MoveOnlyPathResultEnemy, enemyUnits[_closestUnitIndex]));
    }
    public void MoveOnlyPathResultEnemy(PathResult _result)
    {
        if (_result.success)
        {
            //print("Found Path to closest Enemy Unit!");
            StartCoroutine(MoveOnlyTurn(_result.path));
        }
        else
        {
            //print("No path found to closest enemy Unit.");
            ignoreEnemyUnits.Add((EnemyUnit)_result.unit);

            if (ignoreEnemyUnits.Count < enemyUnits.Count)
            {
                //print("Ignoring that unit and trying again");
                MoveToNearestEnemyUnit();
            }
            else
            {
                //print("No path to any unit possible. Passing Turn.");
                Invoke(nameof(EndTurn), .5f);
            }
        }
    }
    public IEnumerator ExecuteTurn(EnemyMove _move)
    {
        //print("Executing move!");
        if (_move.result != null && _move.movePosition != transform.position)
        {
            //print("Moving");
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
            //print("Using Skill!");

            if(_move.skillToUse.skill.targetingStyle == TargetingStyle.ProjectileAOE)
            {
                UseProjectileAOESkill(_move.skillToUse, _move.target.transform.position);
            }
            else
            {
                _move.skillToUse.skill.UseSkill(this, _move.target);
            }
        }

        //print("Ending Turn!");
        Invoke(nameof(EndTurn), .5f);
    }
    public IEnumerator MoveOnlyTurn(Waypoint[] _path)
    {
        //print("Moving Only!");
        motor.Move(_path);
        do
        {
            yield return new WaitForEndOfFrame();
        } while (motor.isMoving);

        //print("Ending Turn after only moving!");
        Invoke(nameof(EndTurn), .5f);
    }
    void EndTurn()
    {
        //print("Turn Ending!");
        TurnManager.instance.NextTurn();
    }

    void UseProjectileAOESkill(EnemySkill _skill, Vector3 _position)
    {

        int _radius = _skill.skill.aoeRadius;

        Node _baseNode = grid.NodeFromWorldPoint(_position);

        int _gridX = _baseNode.gridX;
        int _gridY = _baseNode.gridY;

        int _xMin = Mathf.Clamp(_gridX - _radius, 1, grid.gridSizeX - 1);
        int _xMax = Mathf.Clamp(_gridX + _radius, 1, grid.gridSizeX - 1);

        int _yMin = Mathf.Clamp(_gridY - _radius, 1, grid.gridSizeY - 1);
        int _yMax = Mathf.Clamp(_gridY + _radius, 1, grid.gridSizeY - 1);

        List<Vector3> _validNodes = new List<Vector3>();

        for (int x = _xMin; x <= _xMax; x++)
        {
            for (int y = _yMin; y <= _yMax; y++)
            {
                Node _newNode = grid.RetrieveNode(x, y);

                if (_newNode.walkable || !grid.LinecastToWorldPoint(_baseNode.worldPosition, _newNode.worldPosition))
                {
                    Vector2 _v = new Vector2(x - _gridX, y - _gridY);

                    if (_v.magnitude <= _radius)
                    {
                        _validNodes.Add(_newNode.worldPosition);
                    }
                }
            }
        }

        if (_validNodes.Count > 0)
        {
            _skill.skill.UseProjectileSkill(this, _validNodes);
        }
    }
}

public struct EnemyMove
{
    public Vector3 movePosition;
    public int pathPositionIndex;
    public EnemySkill skillToUse;
    public Unit target;
    public PathResult result;

    public EnemyMove(Vector3 _movePosition, int _pathPositionIndex, EnemySkill _skillToUse, Unit _target, PathResult _result)
    {
        movePosition = _movePosition;
        pathPositionIndex = _pathPositionIndex;
        skillToUse = _skillToUse;
        target = _target;
        result = _result;
    }
}