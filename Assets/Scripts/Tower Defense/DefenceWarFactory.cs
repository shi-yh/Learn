using UnityEngine;

namespace Tower_Defense
{
    [CreateAssetMenu]
    public class DefenceWarFactory : GameObjecFactory
    {
        [SerializeField] private Shell _shellPrefab = default;

        [SerializeField] private Explosion _explosionPrefab = default;

        public Shell Shell => Get(_shellPrefab);

        public Explosion Explosion => Get(_explosionPrefab);

        private T Get<T>(T shellPrefab) where T : DefenceWarEntity
        {
            T instance = CreateGameObjectInstance(shellPrefab);
            instance.OriginFactory = this;
            return instance;
        }

        public void Reclaim(DefenceWarEntity entity)
        {
            Destroy(entity.gameObject);
        }
    }
}