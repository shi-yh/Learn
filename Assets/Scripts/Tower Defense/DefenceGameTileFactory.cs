using UnityEngine;

namespace Tower_Defense
{
    [CreateAssetMenu]
    public class DefenceGameTileFactory : GameObjecFactory
    {
        [SerializeField] private GameTileContent _destinationPrefab = default;

        [SerializeField] private GameTileContent _emptyPrefab = default;

        [SerializeField] private GameTileContent _wallPrefab = default;

        [SerializeField] private GameTileContent _spawnPointPrefab = default;

        [SerializeField] private Tower[] _towerPrefabs = default;

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
                    result = Get(_spawnPointPrefab);
                    break;
                }
            }

            return result;
        }


        public Tower Get(TowerType type)
        {
            Tower prefab = _towerPrefabs[(int)type];
            return Get(prefab);
        }

        T Get<T>(T prefab) where T : GameTileContent
        {
            T instance = CreateGameObjectInstance(prefab);
            instance.OriginFactory = this;
            return instance;
        }
    }
}