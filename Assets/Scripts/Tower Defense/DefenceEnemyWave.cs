using UnityEngine;

namespace Tower_Defense
{
    [CreateAssetMenu]
    public class DefenceEnemyWave : ScriptableObject
    {
        [SerializeField] private DefenceEnemySpawnSequence[] _spawnSequences = { new DefenceEnemySpawnSequence() };

        public State Begin() => new State(this);

        public struct State
        {
            private DefenceEnemyWave _wave;

            private int _index;

            private DefenceEnemySpawnSequence.State _sequence;

            public State(DefenceEnemyWave wave)
            {
                _wave = wave;

                _index = 0;

                _sequence = _wave._spawnSequences[0].Begin();
            }

            public float Progress(float deltaTime)
            {
                deltaTime = _sequence.Progress(deltaTime);
                while (deltaTime >= 0f)
                {
                    if (++_index >= _wave._spawnSequences.Length)
                    {
                        return deltaTime;
                    }

                    _sequence = _wave._spawnSequences[_index].Begin();
                    deltaTime = _sequence.Progress(deltaTime);
                }

                return -1f;
            }
        }
    }
}