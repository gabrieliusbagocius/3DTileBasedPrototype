using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    public List<Tile> FindPath(Tile start, Tile end)
    {
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();
        openList.Add(start);
        while (openList.Count > 0)
        {
            Tile q = openList.OrderBy(x => x.F).First();
            openList.Remove(q);
            closedList.Add(q);
            if (q == end)
            {
                return GetFinishedList(start, end);
            }
            foreach (var tile in GetNeighbourTiles(q))
            {
                if (!tile.WalkableHasPlayerOrEmpty() || closedList.Contains(tile))
                {
                    continue;
                }

                tile.G = GetManhattenDistance(start, tile);
                tile.H = GetManhattenDistance(end, tile);
                tile.previousTile = q;


                if (!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }
        
        return new List<Tile>();
    }

    private int GetManhattenDistance(Tile start, Tile tile)
    {
        return (Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.z - tile.gridLocation.z));
    }

    private List<Tile> GetFinishedList(Tile start, Tile end)
    {
        List<Tile> finishedList = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previousTile;
        }

        finishedList.Reverse();
        return finishedList;
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
  
}
