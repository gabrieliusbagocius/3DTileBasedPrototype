using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Tile _grassTile = null, _dirtTile = null;
    [SerializeField] private GameObject _spawnedTileHierarchyLoc;
    [SerializeField] private bool _useRandSeed = true;
    [SerializeField] private int _width = 10, _height = 1, _length = 10;
    [SerializeField] private int _randSeed = 123321;
    private int _randVar = 0;
    private Tile _spawnedTile = null;
    private Dictionary<Vector3, Tile> _tiles;
    

    public Dictionary<Vector3, Tile> Tiles
    {
        get { return _tiles; }
        set { }
    }

    public Tile FindFactionInTiles(Faction faction)
    {
        var factionTiles = new List<Tile>();
        if (_tiles != null)
        {
            foreach (Tile x in _tiles.Values)
            {
                if (x.OccupiedUnit != null)
                {
                    if (x.OccupiedUnit.Faction == faction)
                    {
                        factionTiles.Add(x);
                    }
                }
            }
            if (factionTiles.Any())
            {
                return factionTiles[Random.Range(0, factionTiles.Count())];
            }
        }
        return null;
    }

    public void GenerateGrid()
    {
        if (_useRandSeed)
        {
            Random.seed = _randSeed;
        }

        if (_tiles == null)
        {
            _tiles = new Dictionary<Vector3, Tile>();
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _length; z++)
                {
                    _randVar = Random.Range(0, 10);
                    if (_randVar == 9)
                    {
                        _spawnedTile = Instantiate(_dirtTile, new Vector3(x, y, z), Quaternion.identity);
                        _spawnedTile.transform.parent = _spawnedTileHierarchyLoc.transform;
                        SetTileToDictionary(new Vector3(x, y, z), _spawnedTile);
                        _spawnedTile.Init(x, y, z);
                        _spawnedTile = Instantiate(_dirtTile, new Vector3(x, y + 1, z), Quaternion.identity);
                        _spawnedTile.transform.parent = _spawnedTileHierarchyLoc.transform;
                        SetTileToDictionary(new Vector3(x, y, z), _spawnedTile);
                        _spawnedTile.Init(x, y, z);
                    }
                    else
                    {
                        _spawnedTile = Instantiate(_grassTile, new Vector3(x, y, z), Quaternion.identity);
                        _spawnedTile.transform.parent = _spawnedTileHierarchyLoc.transform;
                        SetTileToDictionary(new Vector3(x, y, z), _spawnedTile);
                        _spawnedTile.Init(x, y, z);
                    }
                }
            }
        }

        for (int x = -1; x < _width + 1; x++)
        {
            SetupTile(_dirtTile, x, 0, -1);
            SetupTile(_dirtTile, x, 1, -1);
            SetupTile(_dirtTile, x, 0, _length);
            SetupTile(_dirtTile, x, 1, _length);
        }

        for (int z = 0; z < _length; z++)
        {
            SetupTile(_dirtTile, -1, 0, z);
            SetupTile(_dirtTile, -1, 1, z);
            SetupTile(_dirtTile, _width, 0, z);
            SetupTile(_dirtTile, _width, 1, z);
        }
    }

    public Tile GetEnemySpawnTile()
    {
        return _tiles.Where(t => t.Key.z > _length / 4 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetHeroSpawnTile()
    {
        return _tiles.Where(t => t.Key.z < _length / 8 && t.Key.y == 0 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector3 pos)
    {
        if (_tiles.ContainsKey(pos))
        {
            if (_tiles.TryGetValue(pos, out var tile))
            {
                return tile;
            }
        }
        return null;
    }

    public void SetTileToDictionary(Vector3 location, Tile tile)
    {
        if (_tiles == null)
        {
            _tiles = new Dictionary<Vector3, Tile>();
        }
        if (!_tiles.ContainsKey(location))
        {
            tile.name = $"Tile({location.x}, {location.y}, {location.z})";
            _tiles.Add(location, tile);
        }
    }

    private void SetupTile(Tile prefabTile, int x, int y, int z)
    {
        var spawnedTile = Instantiate(prefabTile, new Vector3(x, y, z), Quaternion.identity);
        spawnedTile.transform.parent = _spawnedTileHierarchyLoc.transform;
        SetTileToDictionary(new Vector3(x, y, z), spawnedTile);
        _spawnedTile.Init(x, y, z);
    }
}