using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public string UnitName;
    public Tile OccupiedTile;
    public Faction Faction;

    public UnitClass unitClass;

    public int damage = 1;

    public event System.Action<float> OnHealthPctChanged = delegate { };

    private bool _alive = true;
    public bool Alive
    {
        get
        {
            if (_health <= 0) _alive = false;
            return _alive;
        }
        set { _alive = value; }
    }

    public int _health = 4;

    [Tooltip("Health is the current health that player has, this is the starting health")]
    public int startingHealth = 4;

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                if (this.Faction == Faction.Hero) gameObject.GetComponentInChildren<BaseHero>().Death();
                else if (this.Faction == Faction.Enemy) gameObject.GetComponentInChildren<BaseEnemy>().Death();
            }
            else OnHealthPctChanged((float)Health / (float)startingHealth);
        }
    }

    public bool CheckIfLethal(int damage)
    {
        return Health - damage <= 0;
    } 
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

}


