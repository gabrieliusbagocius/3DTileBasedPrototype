using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : BaseUnit
{
    private PathFinder pathFinder;
    [SerializeField]
    private float movementSpeed = 2f;
    [SerializeField]
    private float activationDistance;
    [SerializeField]
    private Transform _movePoint = null;

    private List<Tile> pathToPlayer;
    private Vector3 _movementDirection;
    private Tile _tileAtMovPos;

    private int _listIterator = 0;
    private bool keepSameHero = false;
    private Tile playerTile;

    [SerializeField]
    private float attackRate = 1f;

    private float nextAttack;
    private float timeSinceLastSuccessfulAtk = 0f;


    [SerializeField]
    private GameObject _projectilePrefab = null;

    private enum States
    {
        Start = 0,
        Wait = 1,
        Continue = 2,
    }

    private States _state = States.Start;

    public void Death()
    {
        OccupiedTile.RemoveUnit(this);
        GetComponent<HealthBar>().DisableHealthBars();
        Destroy(base.gameObject);
    }

    protected void Start()
    {
        nextAttack = attackRate;
        _movePoint.parent = GameObject.Find("Enemies").transform;
        _tileAtMovPos = GridManager.Instance.GetTileAtPosition(_movePoint.position);

    }
    /*
    public void Init()
    {
        nextAttack = attackRate;
        _movePoint.parent = null;
        _tileAtMovPos = GridManager.Instance.GetTileAtPosition(_movePoint.position);
    }
    */

    protected void FixedUpdate()
    {
        if (unitClass == UnitClass.Fighter)
        {
            MoveEnemy();
            AttemptAttack();
        }
        else AttemptToFire();
    }
    public void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, movementSpeed * Time.deltaTime);
        if (_state == States.Start)
        {
            pathFinder = new PathFinder();
            _listIterator = 0;
            if (!keepSameHero || playerTile == null) playerTile = UnitManager.Instance.GetRandomPlayer();
            if (playerTile != null)
            {
                pathToPlayer = pathFinder.FindPath(OccupiedTile, playerTile);
                if (pathToPlayer.Count > 2)
                {

                    //pathToPlayer.RemoveAt(pathToPlayer.Count - 1);
                    _state = States.Continue;
                }
            }
        }
        else if (_state == States.Continue)
        {
            if (pathToPlayer.Count > _listIterator && activationDistance > pathToPlayer.Count) MoveAlongPath(pathToPlayer[_listIterator]);
            else _state = States.Start;
        }
    }


    void AttemptToFire()
    {

        if (playerTile == null)
        {
            playerTile = UnitManager.Instance.GetRandomPlayer();
        }

        if (playerTile != null)
        {
            if (timeSinceLastSuccessfulAtk >= nextAttack)
            {
                nextAttack = timeSinceLastSuccessfulAtk + attackRate;
                FireProjectile();
            }
            if (timeSinceLastSuccessfulAtk == 0)
            {

                timeSinceLastSuccessfulAtk = Time.time;
                nextAttack = timeSinceLastSuccessfulAtk + attackRate;
            }
            else timeSinceLastSuccessfulAtk = Time.time;
        }
        else
        {
            timeSinceLastSuccessfulAtk = 0;
        }

    }

    void FireProjectile()
    {
        var closestTile = GetClosestTileToPlayer();
        if (closestTile != null)
        {
            var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            projectile.transform.parent = GameObject.Find("EnemyAttacks").transform;
        }
    }

    void AttemptAttack()
    {
        var foundPlayer = FindNearbyPlayer();
        if (foundPlayer != null)
        {
            _state = States.Wait;

            if (timeSinceLastSuccessfulAtk >= nextAttack)
            {
                nextAttack = timeSinceLastSuccessfulAtk + attackRate;
                Attack(foundPlayer);
                //timeSinceLastSuccessfulAtk += nextAttack;
            }
            if (timeSinceLastSuccessfulAtk == 0)
            {
                timeSinceLastSuccessfulAtk = Time.time;
                nextAttack = timeSinceLastSuccessfulAtk + attackRate;
            }
            else timeSinceLastSuccessfulAtk = Time.time;
        }
        else
        {
            if (_state == States.Wait) _state = States.Start;
            timeSinceLastSuccessfulAtk = 0;
        }

    }

    BaseUnit FindNearbyPlayer()
    {
        var tileAtCurrentPos = GridManager.Instance.GetTileAtPosition(_movePoint.position);
        var neighbours = GetNeighbourTiles(tileAtCurrentPos);
        foreach (Tile x in neighbours)
        {
            if (x.OccupiedUnit != null)
            {
                if (x.OccupiedUnit.Faction == Faction.Hero) return x.OccupiedUnit;
            }
        }
        return null;
    }

    Tile GetClosestTileToPlayer()
    {
        Tile bestTile = null;
        int bestScore = 0;
        var neighbours = GetNeighbourTiles(GridManager.Instance.GetTileAtPosition(_movePoint.position));

        foreach (Tile x in neighbours)
        {
            var tileScores = Mathf.RoundToInt(Mathf.Abs(playerTile.transform.position.x - _movePoint.position.x) + Mathf.Abs(playerTile.transform.position.z - _movePoint.position.z));
            if (bestScore < tileScores)
            {
                if (x.Walkable)
                {
                    tileScores = bestScore;
                    bestTile = x;
                }
            }
        }
        return bestTile;
    }

    void Attack(BaseUnit unit)
    {
        unit.TakeDamage(damage);
    }

    private List<Tile> GetNeighbourTiles(Tile currentTile)
    {
        var map = GridManager.Instance.Tiles;
        List<Tile> neighbours = new List<Tile>();

        //right
        Vector3Int locationToCheck = new Vector3Int(
            currentTile.gridLocation.x + 1,
            currentTile.gridLocation.y,
            currentTile.gridLocation.z
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //left
        locationToCheck = new Vector3Int(
            currentTile.gridLocation.x - 1,
            currentTile.gridLocation.y,
            currentTile.gridLocation.z
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //top
        locationToCheck = new Vector3Int(
            currentTile.gridLocation.x,
            currentTile.gridLocation.y,
            currentTile.gridLocation.z + 1
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //bottom
        locationToCheck = new Vector3Int(
            currentTile.gridLocation.x,
            currentTile.gridLocation.y,
            currentTile.gridLocation.z - 1
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        return neighbours;
    }

    void MoveAlongPath(Tile tile)
    {
        if (transform.position == _movePoint.position)
        {
            _movementDirection = new Vector3(tile.transform.position.x - _movePoint.position.x, tile.transform.position.y - _movePoint.position.y, tile.transform.position.z - _movePoint.position.z);
            if (Mathf.Abs(_movementDirection.x) > 0.8f)
            {
                _tileAtMovPos = GridManager.Instance.GetTileAtPosition(new Vector3(_movePoint.position.x + _movementDirection.x, _movePoint.position.y, _movePoint.position.z));

                if (_tileAtMovPos != null && _tileAtMovPos.Walkable) ChangeTheMovePoint(new Vector3(Mathf.Round(_movementDirection.x), 0f, 0f));
                else _state = States.Start;
                _listIterator++;

            }
            else if (Mathf.Abs(_movementDirection.z) > 0.8f)
            {
                _tileAtMovPos = GridManager.Instance.GetTileAtPosition(new Vector3(_movePoint.position.x, _movePoint.position.y, _movePoint.position.z + _movementDirection.z));
                if (_tileAtMovPos != null && _tileAtMovPos.Walkable) ChangeTheMovePoint(new Vector3(0f, 0f, _movementDirection.z));
                else _state = States.Start;
                _listIterator++;
            }
        }
    }

    void ChangeTheMovePoint(Vector3 changeBy)
    {
        _tileAtMovPos.RemoveUnit(this);
        _movePoint.position += changeBy;
        _tileAtMovPos.AddUnit(this);
    }

}
