using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class DefenceGameTileFactory : GameObjecFactory
{
    [SerializeField] private GameTileContent _destinationPrefab = default;

    [SerializeField] private GameTileContent _emptyPrefab = default;

    [SerializeField] private GameTileContent _wallPrefab;

    [SerializeField] private GameTileContent _SpwanPointPrefab;

    public void Reclaim(GameTileContent gameTileContent)
    {
        Debug.Assert(gameTileContent.OriginFactory == this, "Wrong factory reclaimed");
        Destroy(gameTileContent.gameObject);
    }


    public GameTileContent Get(DefenceGameTileContentType type)
    {
        GameTileContent result = null;
        switch (type)
        {
            case DefenceGameTileContentType.Empty:
            {
                result = Get(_emptyPrefab);
                break;
            }
            case DefenceGameTileContentType.Destination:
            {
                result = Get(_destinationPrefab);
                break;
            }

            case DefenceGameTileContentType.Wall:
            {
                result = Get(_wallPrefab);
                break;
            }
            case DefenceGameTileContentType.SpawnPoint:
            {
                result = Get(_SpwanPointPrefab);
                break;
            }
        }

        return result;
    }

    GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}