using UnityEngine;
using Random = UnityEngine.Random;

namespace Tower_Defense
{
    public class DefenceGame : MonoBehaviour
    {
        [SerializeField] private Vector2Int _boradSize = new Vector2Int(11, 11);

        [SerializeField] private DefenceGameBoard _board = default;

        [SerializeField] private DefenceGameTileFactory _tileContectFactory = default;

        [SerializeField] private DefenceEnemyFactory _enemyFactory;

        [SerializeField] private DefenceWarFactory _warFactory;

        [SerializeField, Range(0.1f, 10f)] private float _spawnSpeed = 1;

        private GameBehaviorCollection _enemies = new GameBehaviorCollection();

        protected GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

        private float _spwanProgress;

        private TowerType _selectedTowerType;

        private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

        private static DefenceGame s_instance;

        private void Awake()
        {
            _board.Initialize(_boradSize, _tileContectFactory);
        }


        private void OnValidate()
        {
            if (_boradSize.x < 2)
            {
                _boradSize.x = 2;
            }

            if (_boradSize.y < 2)
            {
                _boradSize.y = 2;
            }
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                HandleAlternativeTouch();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                _board.ShowPath = !_board.ShowPath;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _board.ShowGrid = !_board.ShowGrid;
            }

            if (Input.GetKeyDown((KeyCode.Alpha1)))
            {
                _selectedTowerType = TowerType.Laser;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _selectedTowerType = TowerType.Mortar;
            }


            _spwanProgress += _spawnSpeed * Time.deltaTime;
            while (_spwanProgress >= 1f)
            {
                _spwanProgress -= 1;
                SpawnEnemy();
            }

            _enemies.GameUpdate();
            ///因为物体初始会生成在000的位置，然后才会移动到需要的位置，所以需要手动同步物理引擎状态
            Physics.SyncTransforms();
            _board.GameUpdate();
            _nonEnemies.GameUpdate();
        }

        private void SpawnEnemy()
        {
            DefenceGameTile tile = _board.GetSpawnPoint(Random.Range(0, _board.SpawnPointCount));

            DefenceEnemy enemy = _enemyFactory.Get();
            enemy.SpawnOn(tile);
            _enemies.Add(enemy);
        }

        public static Shell SpawnShell()
        {
            Shell shell = s_instance._warFactory.Shell;
            s_instance._nonEnemies.Add(shell);
            return shell;
        }

        public static Explosion SpawnExplosion()
        {
            Explosion explosion = s_instance._warFactory.Explosion;
            s_instance._nonEnemies.Add(explosion);
            return explosion;
        }

        private void OnEnable()
        {
            s_instance = this;
        }


        private void HandleTouch()
        {
            DefenceGameTile tile = _board.GetTile(TouchRay);

            if (tile != null)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _board.ToggleTower(tile, _selectedTowerType);
                }
                else
                {
                    _board.ToggleWall(tile);
                }
            }
        }

        private void HandleAlternativeTouch()
        {
            DefenceGameTile tile = _board.GetTile(TouchRay);

            if (tile != null)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _board.ToggleDestination(tile);
                }
                else
                {
                    _board.ToggleSpawnPoint(tile);
                }
            }
        }
    }
}