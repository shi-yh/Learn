using UnityEngine;

namespace Tower_Defense
{
    [CreateAssetMenu]
    public class GameScenario : ScriptableObject
    {
        [SerializeField] private DefenceEnemyWave[] _waves = { };

        [SerializeField, Range(0, 10)] private int _cycles = 1;

        [SerializeField, Range(0, 1f)] private float _cycleSpeedUp = 0.5f;

        public State Begin() => new State(this);

        [System.Serializable]
        public struct State
        {
            private GameScenario _scenario;

            private int _index, _cycle;

            private float _timeScale;

            private DefenceEnemyWave.State _wave;

            public State(GameScenario scenario)
            {
                _scenario = scenario;

                _index = 0;
                _cycle = 0;
                _timeScale = 1f;
                _wave = _scenario._waves[0].Begin();
            }


            public bool Progress()
            {
                if (_index >= _scenario._waves.Length)
                {
                    return false;
                }

                float deltatime = _wave.Progress(_timeScale * Time.deltaTime);

                while (deltatime >= 0f)
                {
                    if (++_index >= _scenario._waves.Length)
                    {
                        if (++_cycle >= _scenario._cycles && _scenario._cycles > 0)
                        {
                            return false;
                        }

                        _index = 0;
                        _timeScale += _scenario._cycleSpeedUp;
                    }

                    _wave = _scenario._waves[_index].Begin();
                    deltatime = _wave.Progress(deltatime);
                }

                return true;
            }
        }
    }
}