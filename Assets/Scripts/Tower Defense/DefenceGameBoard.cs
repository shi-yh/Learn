using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceGameBoard : MonoBehaviour
{
    [SerializeField] private Transform _ground = default;

    [SerializeField] private DefenceGameTile _tilePrefab = default;

    [SerializeField] private Texture2D _gridTexture = default;


    private Vector2Int _size;

    private DefenceGameTile[] _tiles;

    private Queue<DefenceGameTile> _searchFrontier = new Queue<DefenceGameTile>();

    private DefenceGameTileFactory _contentFactory;

    private bool _showPath, _showGrid;

    private List<DefenceGameTile> _spawnPoints = new List<DefenceGameTile>();

    public int SpawnPointCount => _spawnPoints.Count;

    public bool ShowPath
    {
        get => _showPath;
        set
        {
            _showPath = value;
            if (_showPath)
            {
                foreach (DefenceGameTile tile in _tiles)
                {
                    tile.ShowPath();
                }
            }
            else
            {
                foreach (DefenceGameTile tile in _tiles)
                {
                    tile.HidePath();
                }
            }
        }
    }

    public bool ShowGrid
    {
        get => _showGrid;
        set
        {
            _showGrid = value;

            Material m = _ground.GetComponent<MeshRenderer>().material;

            if (_showGrid)
            {
                m.mainTexture = _gridTexture;
                m.SetTextureScale("_MainTex", _size);
            }
            else
            {
                m.mainTexture = null;
            }
        }
    }


    public void Initialize(Vector2Int size, DefenceGameTileFactory contentFactory)
    {
        this._size = size;

        this._contentFactory = contentFactory;

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

                tile.Content = contentFactory.Get(DefenceGameTileContentType.Empty);
            }
        }

        ToggleDestination(_tiles[0]);
        ToggleSpawnPoint(_tiles[^1]);
    }


    public DefenceGameTile GetSpawnPoint(int index)
    {
        return _spawnPoints[index];
    }

    private bool FindPaths()
    {
        foreach (DefenceGameTile defenceGameTile in _tiles)
        {
            if (defenceGameTile.Content.Type == DefenceGameTileContentType.Destination)
            {
                defenceGameTile.BecomeDestination();
                _searchFrontier.Enqueue(defenceGameTile);
            }
            else
            {
                defenceGameTile.ClearPath();
            }
        }

        if (_searchFrontier.Count == 0)
        {
            return false;
        }

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

        ///防止因为添加墙壁导致的闭合死路
        foreach (DefenceGameTile tile in _tiles)
        {
            if (!tile.HasPath)
            {
                return false;
            }
        }

        if (_showPath)
        {
            foreach (var VARIABLE in _tiles)
            {
                VARIABLE.ShowPath();
            }
        }

        return true;
    }

    public void ToggleDestination(DefenceGameTile tile)
    {
        if (tile.Content.Type == DefenceGameTileContentType.Destination)
        {
            tile.Content = _contentFactory.Get(DefenceGameTileContentType.Empty);

            if (!FindPaths())
            {
                tile.Content = _contentFactory.Get(DefenceGameTileContentType.Destination);
                FindPaths();
            }
        }
        else
        {
            tile.Content = _contentFactory.Get(DefenceGameTileContentType.Destination);
            FindPaths();
        }
    }

    public void ToggleWall(DefenceGameTile tile)
    {
        if (tile.Content.Type == DefenceGameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(DefenceGameTileContentType.Empty);

            FindPaths();
        }
        else if (tile.Content.Type == DefenceGameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(DefenceGameTileContentType.Wall);

            if (!FindPaths())
            {
                tile.Content = _contentFactory.Get(DefenceGameTileContentType.Empty);
                FindPaths();
            }
        }
    }
    
    public void ToggleTower(DefenceGameTile tile)
    {
        if (tile.Content.Type == DefenceGameTileContentType.Tower)
        {
            tile.Content = _contentFactory.Get(DefenceGameTileContentType.Empty);

            FindPaths();
        }
        else if (tile.Content.Type == DefenceGameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(DefenceGameTileContentType.Tower);

            if (!FindPaths())
            {
                tile.Content = _contentFactory.Get(DefenceGameTileContentType.Empty);
                FindPaths();
            }
        }
        else if (tile.Content.Type== DefenceGameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(DefenceGameTileContentType.Tower);
        }
    }
    

    public void ToggleSpawnPoint(DefenceGameTile tile)
    {
        if (tile.Content.Type == DefenceGameTileContentType.SpawnPoint)
        {
            if (_spawnPoints.Count > 1)
            {
                _spawnPoints.Remove(tile);
                tile.Content = _contentFactory.Get(DefenceGameTileContentType.Empty);
            }
        }
        else if (tile.Content.Type == DefenceGameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(DefenceGameTileContentType.SpawnPoint);
            _spawnPoints.Add(tile);
        }
    }


    public DefenceGameTile GetTile(Ray ray)
    {
        ///仅开启第0层
        if (Physics.Raycast(ray, out RaycastHit hit,float.MaxValue,1<<0))
        {
            int x = (int) (hit.point.x + _size.x * 0.5f);
            int y = (int) (hit.point.z + _size.y * 0.5f);

            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
            {
                return _tiles[x + y * _size.x];
            }
        }

        return null;
    }
}