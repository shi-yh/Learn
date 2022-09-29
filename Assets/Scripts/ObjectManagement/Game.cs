using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
    [SerializeField] private ShapeFactory _shapeFactory;

    public KeyCode createKey = KeyCode.C;

    public KeyCode newGameKey = KeyCode.N;

    public KeyCode saveKey = KeyCode.S;

    public KeyCode loadKey = KeyCode.L;

    public KeyCode destroyKey = KeyCode.X;

    public int levelCount;

    [SerializeField] private bool _reseedOnLoad;

    private List<Shape> _shapes;

    public PersisentStorage storage;

    private const int SAVE_VERSION = 3;

    private int _loadSceneIdx;

    private Random.State _mainRandomState;


    private void Awake()
    {
        _shapes = new List<Shape>();

        StartCoroutine(LoadLevel(1));
    }

    private void Start()
    {
        BeginNewGame();
        _mainRandomState = Random.state;
    }


    IEnumerator LoadLevel(int levelIdx)
    {
        enabled = false;

        if (_loadSceneIdx > 0)
        {
            yield return SceneManager.UnloadSceneAsync(_loadSceneIdx);
        }

        yield return SceneManager.LoadSceneAsync(levelIdx, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelIdx));

        _loadSceneIdx = levelIdx;

        enabled = true;
    }


    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateShape();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }
        else if (Input.GetKeyDown(saveKey))
        {
            storage.Save(this, SAVE_VERSION);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);
        }
        else if (Input.GetKeyDown(destroyKey))
        {
            DestroyShape();
        }
        else
        {
            for (int i = 0; i <= levelCount; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }
            }
        }
    }

    private void BeginNewGame()
    {
        Random.state = _mainRandomState;
        int seed = Random.Range(0, int.MaxValue) ^ (int) Time.unscaledTime;
        _mainRandomState = Random.state;
        Random.InitState(seed);

        for (int i = 0; i < _shapes.Count; i++)
        {
            _shapeFactory.Reclaim(_shapes[i]);
        }

        _shapes.Clear();
    }

    private void DestroyShape()
    {
        if (_shapes.Count > 0)
        {
            int index = Random.Range(0, _shapes.Count);
            _shapeFactory.Reclaim(_shapes[index]);

            ///List继承自Array，删除一个元素需要将后面的所有元素向前移动，因此直接将需要删除的元素放到末尾
            int lastIndex = _shapes.Count - 1;
            _shapes[index] = _shapes[lastIndex];
            _shapes.RemoveAt(lastIndex);
        }
    }


    public override void Save(GameDataWriter writer)
    {
        writer.Write(_shapes.Count);
        writer.Write(Random.state);
        writer.Write(SceneManager.GetActiveScene().buildIndex);
        GameLevel.current.Save(writer);
        for (int i = 0; i < _shapes.Count; i++)
        {
            writer.Write(_shapes[i].ShapeId);
            writer.Write(_shapes[i].MaterialId);
            _shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;

        if (version > SAVE_VERSION)
        {
            Debug.LogError("Unsupported future save version" + version);
            return;
        }

        StartCoroutine(LoadGame(reader));
    }

    private IEnumerator LoadGame(GameDataReader reader)
    {
        int version = reader.Version;

        int count = version <= 0 ? -version : reader.ReadInt();

        if (version >= 2)
        {
            Random.State state = reader.ReadRandomState();

            if (!_reseedOnLoad)
            {
                Random.state = state;
            }
        }

        yield return LoadLevel(version < 2 ? 1 : reader.ReadInt());

        GameLevel.current.Load(reader);


        for (int i = 0; i < count; i++)
        {
            int shapeId = version > 0 ? reader.ReadInt() : 0;
            int materialId = version > 0 ? reader.ReadInt() : 0;

            Shape o = _shapeFactory.Get(shapeId, materialId);
            o.Load(reader);
            _shapes.Add(o);
        }
    }


    void CreateShape()
    {
        Shape instance = _shapeFactory.GetRandom();

        GameLevel.current.ConfigureSpawn(instance);
        
        _shapes.Add(instance);
        
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _shapes.Count; i++)
        {
            _shapes[i].GameUpdate();
        }
    }
}