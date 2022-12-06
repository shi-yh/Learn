using UnityEngine;

namespace ObjectManagement
{
    public class CompositeSpawnZone : SpawnZone
    {
        [SerializeField] private SpawnZone[] _spawnZones;

        [SerializeField] private bool _sequential;

        [SerializeField] private bool _overrideConfig;

        private int _nextSequentialIndex;

        public override Vector3 SpawnPoint
        {
            get { return Vector3.zero; }
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(_nextSequentialIndex);
        }

        public override void Load(GameDataReader reader)
        {
            _nextSequentialIndex = reader.ReadInt();
        }

        public override Shape SpawnShape()
        {
            if (!_overrideConfig)
            {
                return base.SpawnShape();
            }
            else
            {
                int index;

                if (_sequential)
                {
                    index = _nextSequentialIndex++;
                    if (_nextSequentialIndex >= _spawnZones.Length)
                    {
                        _nextSequentialIndex = 0;
                    }
                }
                else
                {
                    index = Random.Range(0, _spawnZones.Length);
                }

                return _spawnZones[index].SpawnShape();
            }
        }
    }
}