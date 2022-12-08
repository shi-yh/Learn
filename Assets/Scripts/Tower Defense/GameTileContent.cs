using UnityEngine;

namespace Tower_Defense
{
    public enum DefenceGameTileContentType
    {
        Empty,
        Destination,
        Wall,
        SpawnPoint,
        Tower
    }

    [SelectionBase]
    public class GameTileContent : MonoBehaviour
    {
        [SerializeField] private DefenceGameTileContentType _type = default;

        public DefenceGameTileContentType Type => _type;

        private DefenceGameTileFactory _originFactory;


        public bool BlocksPath => _type is DefenceGameTileContentType.Wall or DefenceGameTileContentType.Tower;

        public DefenceGameTileFactory OriginFactory
        {
            get => _originFactory;
            set
            {
                Debug.Assert(_originFactory == null, "Redefined origin Factory");
                _originFactory = value;
            }
        }

        public void Recycle()
        {
            _originFactory.Reclaim(this);
        }

        public virtual void GameUpdate()
        {
        }
    }
}