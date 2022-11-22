using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceGameBoard : MonoBehaviour
{
    [SerializeField] private Transform _ground = default;

    [SerializeField] private DefenceGameTile _tilePrefab = default;


    private Vector2Int _size;

    private DefenceGameTile[] _tiles;

    public void Initialize(Vector2Int size)
    {
        this._size = size;
        _ground.localScale = new Vector3(size.x, size.y, 1f);

        _tiles = new DefenceGameTile[_size.x * _size.y];

        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
        int index = 0;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++,index++)
            {
                DefenceGameTile tile = _tiles[index] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform,false);
                tile.transform.localPosition = new Vector3(x - offset.x,0, y - offset.y);
            }
        }
        

    }
}