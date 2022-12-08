using UnityEngine;

namespace Tower_Defense
{
    [System.Serializable]
    public class DefenceEnemySpawnSequence
    {
        [SerializeField] private DefenceEnemyFactory _factory = default;

        [SerializeField] private DefenceEnemyType _type = DefenceEnemyType.Medium;

        [SerializeField, Range(1, 100)] private int _amount = 1;

        [SerializeField, Range(0.1f, 10f)] private float _cooldown = 1f;


        /// <summary>
        /// 每当要开始处理序列时，就需要为其获取一个新的状态实例
        /// </summary>
        /// <returns></returns>
        public State Begin() => new State(this);


        [System.Serializable]
        public struct State
        {
            private DefenceEnemySpawnSequence _sequence;

            private int _count;

            private float _cooldown;
            
            
            public State(DefenceEnemySpawnSequence sequence)
            {
                _sequence = sequence;
                _count = 0;
                _cooldown = _sequence._cooldown;
            }

            public float Progress(float deltaTime)
            {
                _cooldown += deltaTime;
                while (_cooldown >= _sequence._cooldown)
                {
                    _cooldown -= _sequence._cooldown;
                    _count++;
                    
                    DefenceGame.SpawnEnemy(_sequence._factory,_sequence._type);
                    
                    ///为了保持严谨，返回一个序列结束后，多出的时间
                    if (_count>=_sequence._amount)
                    {
                        return _cooldown;
                    }
                    
                }

                return -1f;
            }

        }
    }
}