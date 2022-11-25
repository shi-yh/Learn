using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class DefenceGameTileFactory : ScriptableObject
{
    private Scene _contentScene;

    [SerializeField] private GameTileContent _destinationPrefab = default;

    [SerializeField] private GameTileContent _emptyPrefab = default;

    [SerializeField] private GameTileContent _wallPrefab;

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
        }

        return result;
    }

    GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        MoveToFactoryScene(instance.gameObject);
        return instance;
    }

    private void MoveToFactoryScene(GameObject o)
    {
        if (!_contentScene.isLoaded)
        {
            if (Application.isEditor)
            {
                _contentScene = SceneManager.GetSceneByName(name);
                if (!_contentScene.isLoaded)
                {
                    _contentScene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                _contentScene = SceneManager.CreateScene(name);
            }
        }

        SceneManager.MoveGameObjectToScene(o, _contentScene);
    }
}