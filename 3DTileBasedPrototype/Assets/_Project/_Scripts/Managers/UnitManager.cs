using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    private List<ScriptableUnit> _units;

    [SerializeField]
    private int _enemyCount = 1;
    [SerializeField]
    private int spawnTimer = 20;


    void Awake()
    {
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }


    private void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad > spawnTimer)
        {
            SpawnEnemies();
            spawnTimer += spawnTimer;
        }
    }

    public Tile GetRandomPlayer()
    {
        return GridManager.Instance.FindFactionInTiles(Faction.Hero);
    }

    public GameObject SpawnHeroes()
    {
        var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
        var spawnedHero = Instantiate(randomPrefab);
        spawnedHero.transform.parent = GameObject.Find("Heroes").transform;
        var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();
        spawnedHero.transform.position += new Vector3(0, 1f, 0);
        randomSpawnTile.SetUnit(spawnedHero);
        return randomPrefab.gameObject;

    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            spawnedEnemy.transform.parent = GameObject.Find("Enemies").transform;
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();
            randomSpawnTile.SetUnit(spawnedEnemy);
        }
    }


    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }
}
