using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovementBehaviour : MonoBehaviour
{

    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    [SerializeField]
    private Transform _movePoint = null;

    //Stored Values
    private Vector3 _movementDirection;
    private Tile _tileAtMovPos;
    [SerializeField]
    private BaseUnit baseUnit = null;

    private void Start()
    {
        _movePoint.parent = GameObject.Find("Heroes").transform;

    }
    void FixedUpdate()
    {
        if (baseUnit.Alive)
        {
            MoveThePlayer();
        }
    }

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
        _movementDirection = newMovementDirection;
    }

    void MoveThePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _movePoint.position) <= .1f)
        {
            if (Mathf.Abs(_movementDirection.x) > 0.8f && Mathf.Abs(_movementDirection.z) < 0.4f)
            {
                CalculateMovementAndAttackLogic(new Vector3(_movePoint.position.x + _movementDirection.x, _movePoint.position.y, _movePoint.position.z), new Vector3(_movementDirection.x, 0f, 0f));
            }
            else if (Mathf.Abs(_movementDirection.z) > 0.8f && _movementDirection.x < 0.4f)
            {
                CalculateMovementAndAttackLogic(new Vector3(_movePoint.position.x, _movePoint.position.y, _movePoint.position.z + _movementDirection.z), new Vector3(0f, 0f, _movementDirection.z));
            }
        }
    }

    void CalculateMovementAndAttackLogic(Vector3 tileLocationAtMovPos, Vector3 newMovePoint)
    {
        _tileAtMovPos = GridManager.Instance.GetTileAtPosition(tileLocationAtMovPos);
        if (_tileAtMovPos != null)
        {
            if (_tileAtMovPos.Walkable) UpdateTheMovePoint(newMovePoint);
            else if (_tileAtMovPos.HasEnemy)
            {
                if (_tileAtMovPos.OccupiedUnit.CheckIfLethal(baseUnit.damage)) baseUnit.GetComponentInChildren<BaseHero>().IncrementKC();
                _tileAtMovPos.OccupiedUnit.TakeDamage(baseUnit.damage);

            }
        }
    }

    void UpdateTheMovePoint(Vector3 changeBy)
    {
        _tileAtMovPos.RemoveUnit(baseUnit);
        _movePoint.position += changeBy;
        _tileAtMovPos.AddUnit(baseUnit);
    }
}
