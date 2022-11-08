using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class SpawnZone : PersistableObject
{
    [SerializeField] private SpawnConfiguration _configuration;

    public abstract Vector3 SpawnPoint { get; }

    public virtual Shape SpawnShape()
    {
        int factoryIndex = Random.Range(0, _configuration.factoryps.Length);

        Shape instance = _configuration.factoryps[factoryIndex].GetRandom();
        
        Transform t = instance.transform;
        t.localPosition = SpawnPoint;
        t.rotation = Random.rotation;
        t.localScale = Vector3.one * _configuration.scale.RandomValueRange;

        if (_configuration.uniformColor)
        {
            Color color = _configuration.color.RandomInRange;

            for (int i = 0; i < instance.ColorCount; i++)
            {
                instance.SetColor(color, i);
            }
        }
        else
        {
            for (int i = 0; i < instance.ColorCount; i++)
            {
                Color color = _configuration.color.RandomInRange;

                instance.SetColor(color, i);
            }
        }

        instance.AngularVelocity = Random.onUnitSphere * _configuration.angularSpeed.RandomValueRange;

        Vector3 direction;

        switch (_configuration.spawnMovementDirection)
        {
            case SpawnConfiguration.SpawnMovementDirection.Forward:
            {
                direction = transform.forward;
                break;
            }
            case SpawnConfiguration.SpawnMovementDirection.Upward:
            {
                direction = transform.up;
                break;
            }
            case SpawnConfiguration.SpawnMovementDirection.Outward:
            {
                direction = (t.localPosition - transform.localPosition).normalized;

                break;
            }
            case SpawnConfiguration.SpawnMovementDirection.Random:
            {
                direction = Random.onUnitSphere;
                break;
            }

            default:
            {
                direction = transform.forward;
                break;
            }
        }


        instance.Velocity = direction * _configuration.spawnSpeed.RandomValueRange;

        return instance;
    }
}