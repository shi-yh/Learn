using UnityEngine;

namespace Tower_Defense
{
    [CreateAssetMenu]
    public class GameScenario : ScriptableObject
    {
        [SerializeField] private DefenceEnemyWave[] _waves = { };

        public State Begin() => new State(this);

        [System.Serializable]
        public struct State
        {
            private GameScenario _scenario;

            private int _index;

            private DefenceEnemyWave.State _wave;

            public State(GameScenario scenario)
            {
                _scenario = scenario;

                _index = 0;

                _wave = _scenario._waves[0].Begin();
            }


            public bool Progress()
            {
                if (_index >= _scenario._waves.Length)
                {
                    return false;
                }

                float deltatime = _wave.Progress(Time.deltaTime);

                while (deltatime >= 0f)
                {
                    if (++_index >= _scenario._waves.Length)
                    {
                        return false;
                    }

                    _wave = _scenario._waves[_index].Begin();
                    deltatime = _wave.Progress(deltatime);
                }

                return true;
            }
        }
    }
}