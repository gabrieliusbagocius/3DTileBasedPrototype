using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 2f;
    private Tile _playerTile = null;
    private Tile tileAtPos = null;

    public int damage = 1;

    private void Awake()
    {
        _playerTile = UnitManager.Instance.GetRandomPlayer();
    }

    void FixedUpdate()
    {
        if (_playerTile != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerTile.transform.position, _movementSpeed * Time.deltaTime);
            if (transform.position == _playerTile.transform.position) Destroy(this.gameObject);
            tileAtPos = GridManager.Instance.GetTileAtPosition(new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z)));
            if (tileAtPos != null)
            {
                if (tileAtPos.OccupiedUnit != null)
                {
                    if (tileAtPos.OccupiedUnit.Faction == Faction.Hero)
                    {
                        tileAtPos.OccupiedUnit.TakeDamage(damage);
                        Destroy(base.gameObject);
                    }
                }
                else if (!tileAtPos.Walkable) Destroy(base.gameObject);
            }
        }
    }
}