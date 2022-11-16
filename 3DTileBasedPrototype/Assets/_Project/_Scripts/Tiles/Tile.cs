using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected MeshRenderer _renderer;
    [SerializeField] private bool _isWalkable = true;

    //Pathfinding A*
    public int G;
    public int H;
    public int F { get { return G + H; } }
    public Tile previousTile;
    public Vector3Int gridLocation;
    public BaseUnit OccupiedUnit;


    public bool Walkable => _isWalkable && OccupiedUnit == null;
    public bool WalkableCouldBeOccupied => _isWalkable;

    public bool HasEnemy => OccupiedUnit != null && OccupiedUnit.Faction == Faction.Enemy;

    public bool WalkableHasPlayerOrEmpty()
    {
        if (_isWalkable)
        {
            if (OccupiedUnit == null || OccupiedUnit.Faction == Faction.Hero) return true;
            else return false;
        }
        else return false;
    }
    public virtual void Init(int x, int y, int z)
    {

    }

    private void Awake()
    {
        gridLocation = Vector3Int.RoundToInt(transform.position);
        GridManager.Instance.SetTileToDictionary(transform.position, this);
        this.OccupiedUnit = null;
    }

    public void AddUnit(BaseUnit unit)
    {
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    public void RemoveUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null)
        {
            OccupiedUnit = null;
            unit.OccupiedTile.OccupiedUnit = null;
        }
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

}