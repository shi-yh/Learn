using UnityEngine;
using Random = UnityEngine.Random;

namespace Tower_Defense
{
    public class DefenceGame : MonoBehaviour
    {
        [SerializeField] private Vector2Int _boradSize = new Vector2Int(11, 11);

        [SerializeField] private DefenceGameBoard _board = default;

        [SerializeField] private DefenceGameTileFactory _tileContectFactory = default;

        [SerializeField] private DefenceWarFactory _warFactory;

        [SerializeField] private int _startingPlayerHealth = 10;

        [SerializeField] private float _playSpeed = 1f;

        private GameBehaviorCollection _enemies = new GameBehaviorCollection();

        protected GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

        private TowerType _selectedTowerType;

        private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

        [SerializeField] private GameScenario _scenario = default;

        private GameScenario.State _activeScenario;

        private static DefenceGame s_instance;

        private int _playerHealth;

        private const float _pausedTimeScale = 0.01f;

        private void Awake()
        {
            _board.Initialize(_boradSize, _tileContectFactory);
            _board.ShowGrid = true;
            _activeScenario = _scenario.Begin();
            _playerHealth = _startingPlayerHealth;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = Time.timeScale > _pausedTimeScale ? _pausedTimeScale : _playSpeed;
            }
            else if (Time.timeScale>_pausedTimeScale)
            {
                Time.timeScale = _playSpeed;
            }

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

            if (Input.GetKeyDown(KeyCode.B))
            {
                BeginNewGame();
            }

            if (_playerHealth <= 0)
            {
                Debug.Log("Defeat");
                BeginNewGame();
            }


            if (!_activeScenario.Progress() && _enemies.IsEmpty)
            {
                Debug.Log("Victory");
                BeginNewGame();
                _activeScenario.Progress();
            }

            _enemies.GameUpdate();
            ///因为物体初始会生成在000的位置，然后才会移动到需要的位置，所以需要手动同步物理引擎状态
            Physics.SyncTransforms();
            _board.GameUpdate();
            _nonEnemies.GameUpdate();
        }

        public static void SpawnEnemy(DefenceEnemyFactory factory, DefenceEnemyType type)
        {
            DefenceGameTile tile = s_instance._board.GetSpawnPoint(Random.Range(0, s_instance._board.SpawnPointCount));

            DefenceEnemy enemy = factory.Get(type);
            enemy.SpawnOn(tile);
            s_instance._enemies.Add(enemy);
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

        public static void EnemyReachedDestination()
        {
            s_instance._playerHealth -= 1;
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

        void BeginNewGame()
        {
            _enemies.Clear();
            _nonEnemies.Clear();
            _board.Clear();
            _activeScenario = _scenario.Begin();
            _playerHealth = _startingPlayerHealth;
        }
    }
}