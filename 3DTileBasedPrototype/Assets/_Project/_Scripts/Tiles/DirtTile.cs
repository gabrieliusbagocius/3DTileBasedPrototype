using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtTile : Tile
{
    [SerializeField] private Material _baseColor = null, _offsetColor = null;

    public override void Init(int x, int y, int z)
    {
        //var isOffSet = ((x + z) % 2 == 1);
        //_renderer.material = isOffSet ? _offsetColor : _baseColor;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
