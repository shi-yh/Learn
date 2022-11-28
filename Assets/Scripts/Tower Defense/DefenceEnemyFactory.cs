using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DefenceEnemyFactory : GameObjecFactory
{
    [SerializeField] private DefenceEnemy _prefab = default;

    public DefenceEnemy Get()
    {
        DefenceEnemy instance = CreateGameObjectInstance(_prefab);
        instance.OriginFactory = this;
        return instance;
    }

    public void Reclaim(DefenceEnemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}