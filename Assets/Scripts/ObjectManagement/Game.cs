using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
    public ShapeFactory shapeFactory;

    public KeyCode createKey = KeyCode.C;

    public KeyCode newGameKey = KeyCode.N;

    public KeyCode saveKey = KeyCode.S;

    public KeyCode loadKey = KeyCode.L;

    public KeyCode destroyKey = KeyCode.X;

    public float CreationSpeed { get; set; }

    public float DestructionSpeed { get; set; }

    private float _creationProgress;
    private float _destructionProgress;

    private List<Shape> _shapes;

    public PersisentStorage storage;

    private const int SAVE_VERSION = 1;

    private void Awake()
    {
        _shapes = new List<Shape>();
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

        _creationProgress += Time.deltaTime * CreationSpeed;

        if (_creationProgress >= 1)
        {
            _creationProgress -= 1;
            CreateShape();
        }

        _destructionProgress += Time.deltaTime * DestructionSpeed;
        if (_destructionProgress >= 1)
        {
            _destructionProgress -= 1;
            DestroyShape();
        }
    }

    private void BeginNewGame()
    {
        for (int i = 0; i < _shapes.Count; i++)
        {
            shapeFactory.Reclaim(_shapes[i]);
        }

        _shapes.Clear();
    }

    private void DestroyShape()
    {
        if (_shapes.Count > 0)
        {
            int index = Random.Range(0, _shapes.Count);
            shapeFactory.Reclaim(_shapes[index]);

            ///List继承自Array，删除一个元素需要将后面的所有元素向前移动，因此直接将需要删除的元素放到末尾
            int lastIndex = _shapes.Count - 1;
            _shapes[index] = _shapes[lastIndex];
            _shapes.RemoveAt(lastIndex);
        }
    }


    public override void Save(GameDataWriter writer)
    {
        writer.Write(_shapes.Count);
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

        int count = version <= 0 ? -version : reader.ReadInt();

        for (int i = 0; i < count; i++)
        {
            int shapeId = version > 0 ? reader.ReadInt() : 0;
            int materialId = version > 0 ? reader.ReadInt() : 0;

            Shape o = shapeFactory.Get(shapeId, materialId);
            o.Load(reader);
            _shapes.Add(o);
        }
    }


    void CreateShape()
    {
        Shape instance = shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = Random.insideUnitSphere * 5;
        t.rotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        instance.SetColor(Random.ColorHSV(0, 1, 0.5f, 1f, 0.25f, 1f, 1f, 1f));
        _shapes.Add(instance);
    }
}