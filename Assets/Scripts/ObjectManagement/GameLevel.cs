using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : PersistableObject
{
    [SerializeField] private SpawnZone _spawnZone;

    [SerializeField] private PersistableObject[] _persistableObjects;
    public static GameLevel current { get; private set; }

    public Vector3 SpawnPoint
    {
        get => _spawnZone.SpawnPoint;
    }


    private void OnEnable()
    {
        current = this;

        if (_persistableObjects == null)
        {
            _persistableObjects = new PersistableObject[0];
        }
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(_persistableObjects.Length);
        for (int i = 0; i < _persistableObjects.Length; i++)
        {
            _persistableObjects[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int savedCount = reader.ReadInt();

        for (int i = 0; i < savedCount; i++)
        {
            _persistableObjects[i].Load(reader);
        }
    }
}