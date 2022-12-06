using UnityEngine;

namespace Tower_Defense
{
    public class DefenceGameTile : MonoBehaviour
    {
        [SerializeField] private Transform _arrow = default;

        private DefenceGameTile _north, _east, _south, _west;

        private GameTileContent _content;

        public GameTileContent Content
        {
            get => _content;
            set
            {
                if (_content != null)
                {
                    _content.Recycle();
                }

                _content = value;
                _content.transform.localPosition = transform.localPosition;
            }
        }

        ///下一个瓦片   
        private DefenceGameTile _nextOnPath;

        /// <summary>
        /// 离目的地的距离
        /// </summary>
        private int _distance;

        public bool HasPath => _distance != int.MaxValue;

        public DefenceGameTile NextTileOnPath => _nextOnPath;

        public Vector3 ExitPoint { get; private set; }

        public Direction PathDirection { get; private set; }


        public DefenceGameTile GrowPathNorth() => GrowPathTo(_north, Direction.South);
        public DefenceGameTile GrowPathEast() => GrowPathTo(_east, Direction.West);
        public DefenceGameTile GrowPathSouth() => GrowPathTo(_south, Direction.North);
        public DefenceGameTile GrowPathWest() => GrowPathTo(_west, Direction.East);

        DefenceGameTile GrowPathTo(DefenceGameTile neighbor, Direction direction)
        {
            Debug.Assert(HasPath, "No Path");

            if (neighbor == null || neighbor.HasPath)
            {
                return null;
            }

            neighbor._distance = _distance + 1;
            neighbor._nextOnPath = this;

            neighbor.ExitPoint = neighbor.transform.localPosition + direction.GetHalfVector();

            neighbor.PathDirection = direction;

            return neighbor.Content.BlocksPath ? null : neighbor;
        }


        /// <summary>
        /// 在查找路线前，需要初始化路径数据，在找到路径前，还没有下一个瓦片，因此距离可以认为是无限的
        /// </summary>
        public void ClearPath()
        {
            _distance = int.MaxValue;
            _nextOnPath = null;
        }

        /// <summary>
        /// 当被设置为目的地时，此瓦片距离目的地（自身）的距离为0，且不再有下一个路径
        /// </summary>
        public void BecomeDestination()
        {
            _distance = 0;
            _nextOnPath = null;
            ExitPoint = transform.localPosition;
        }

        public void HidePath()
        {
            _arrow.gameObject.SetActive(false);
        }

        public void ShowPath()
        {
            if (_distance == 0)
            {
                _arrow.gameObject.SetActive(false);
                return;
            }

            _arrow.gameObject.SetActive(true);
            _arrow.localRotation = _nextOnPath == _north ? northRotation :
                _nextOnPath == _east ? eastRotation :
                _nextOnPath == _south ? southRotation :
                westRotation;
        }


        public static void MakeEastWestNeighbors(DefenceGameTile east, DefenceGameTile west)
        {
            Debug.Assert(west._east == null && east._west == null, "Redefined neighbors");

            east._west = west;

            west._east = east;
        }

        public static void MakeNorthSouthNeigbors(DefenceGameTile north, DefenceGameTile south)
        {
            Debug.Assert(south._north == null && north._south == null, "Redefined neighbors");

            south._north = north;

            north._south = south;
        }

        public static Quaternion
            northRotation = Quaternion.Euler(90f, 0f, 0f),
            eastRotation = Quaternion.Euler(90, 90, 0),
            southRotation = Quaternion.Euler(90, 180, 0),
            westRotation = Quaternion.Euler(90, 270, 0);
    }
}