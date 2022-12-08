using ObjectManagement;
using UnityEngine;

namespace Tower_Defense
{
    [CreateAssetMenu]
    public class DefenceEnemyFactory : GameObjecFactory
    {
        [System.Serializable]
        private class DefenceEnemyConfig
        {
            public DefenceEnemy prefab = default;
            [FloatRangeSlier(0.5f, 2f)] public FloatRange scale = new FloatRange(1f);
            [FloatRangeSlier(0.2f, 5f)] public FloatRange speed = new FloatRange(1f);
            [FloatRangeSlier(-0.4f, 0.4f)] public FloatRange pathOffset = new FloatRange(0f);
            [FloatRangeSlier(10f, 1000f)] public FloatRange health = new FloatRange(100f);
        }

        [SerializeField] private DefenceEnemyConfig _small = default, _medium = default, _large = default;

        public DefenceEnemy Get(DefenceEnemyType type = DefenceEnemyType.Medium)
        {
            DefenceEnemyConfig config = GetConfig(type);

            DefenceEnemy instance = CreateGameObjectInstance(config.prefab);
            instance.OriginFactory = this;
            instance.Initialize(config.scale.RandomValueRange, config.pathOffset.RandomValueRange,
                config.speed.RandomValueRange, config.health.RandomValueRange);
            return instance;
        }

        private DefenceEnemyConfig GetConfig(DefenceEnemyType type)
        {
            switch (type)
            {
                case DefenceEnemyType.Small:
                {
                    return _small;
                }
                case DefenceEnemyType.Medium:
                {
                    return _medium;
                }
                case DefenceEnemyType.Large:
                {
                    return _large;
                }
            }

            return null;
        }

        public void Reclaim(DefenceEnemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}