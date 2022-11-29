using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DefenceEnemyFactory : GameObjecFactory
{
    [SerializeField] private DefenceEnemy _prefab = default;

    [SerializeField, FloatRangeSlier(0.5f, 2f)]
    private FloatRange _scale = new FloatRange(1);

    [SerializeField, FloatRangeSlier(-0.4f, 0.4f)]
    private FloatRange _pathOffset = new FloatRange(0);

    [SerializeField,Range(0.2f,5f)] private FloatRange _speed = new FloatRange(1f);

    public DefenceEnemy Get()
    {
        DefenceEnemy instance = CreateGameObjectInstance(_prefab);
        instance.OriginFactory = this;
        instance.Initialize(_scale.RandomValueRange, _pathOffset.RandomValueRange,_speed.RandomValueRange);
        return instance;
    }

    public void Reclaim(DefenceEnemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}