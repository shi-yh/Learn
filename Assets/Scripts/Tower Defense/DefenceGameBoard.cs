using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceGameBoard : MonoBehaviour
{
    [SerializeField] private Transform _ground = default;

    [SerializeField] private DefenceGameTile _tilePrefab = default;


    private Vector2Int _size;

    private DefenceGameTile[] _tiles;

    private Queue<DefenceGameTile> _searchFrontier = new Queue<DefenceGameTile>();


    public void Initialize(Vector2Int size)
    {
        this._size = size;
        _ground.localScale = new Vector3(size.x, size.y, 1f);

        _tiles = new DefenceGameTile[_size.x * _size.y];

        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
        int index = 0;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, index++)
            {
                DefenceGameTile tile = _tiles[index] = Instantiate(_tilePrefab);
                Transform transform1;
                (transform1 = tile.transform).SetParent(transform, false);
                transform1.localPosition = new Vector3(x - offset.x, 0, y - offset.y);

                if (x > 0)
                {
                    DefenceGameTile.MakeEastWestNeighbors(tile, _tiles[index - 1]);
                }

                if (y > 0)
                {
                    DefenceGameTile.MakeNorthSouthNeigbors(tile, _tiles[index - size.x]);
                }
            }
        }

        FindPaths();
    }

    private void FindPaths()
    {
        foreach (DefenceGameTile defenceGameTile in _tiles)
        {
            defenceGameTile.ClearPath();
        }

        _tiles[0].BecomeDestination();
        _searchFrontier.Enqueue(_tiles[0]);

        while (_searchFrontier.Count > 0)
        {
            DefenceGameTile tile = _searchFrontier.Dequeue();
            if (tile != null)
            {
                _searchFrontier.Enqueue(tile.GrowPathNorth());
                _searchFrontier.Enqueue(tile.GrowPathSouth());
                _searchFrontier.Enqueue(tile.GrowPathEast());
                _searchFrontier.Enqueue(tile.GrowPathWest());
            }
        }

        foreach (var VARIABLE in _tiles)
        {
            VARIABLE.ShowPath();
        }
    }
}