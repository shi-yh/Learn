using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
    public PersistableObject prefab;

    public KeyCode createKey = KeyCode.C;

    public KeyCode newGameKey = KeyCode.N;

    public KeyCode saveKey = KeyCode.S;

    public KeyCode loadKey = KeyCode.L;

    private List<PersistableObject> _objects;

    public PersisentStorage storage;

    private void Awake()
    {
        _objects = new List<PersistableObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            storage.Save(this);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);
        }
    }

    private void BeginNewGame()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            Destroy(_objects[i].gameObject);
        }

        _objects.Clear();
    }


    public override void Save(GameDataWriter writer)
    {
        writer.Write(_objects.Count);
        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int count = reader.ReadInt();

        for (int i = 0; i < count; i++)
        {
            PersistableObject o = Instantiate(prefab);
            o.Load(reader);
            _objects.Add(o);
        }
    }


    void CreateObject()
    {
        PersistableObject o = Instantiate(prefab);
        Transform t = o.transform;
        t.localPosition = Random.insideUnitSphere * 5;
        t.rotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        _objects.Add(o);
    }
}