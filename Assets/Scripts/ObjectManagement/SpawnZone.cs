using UnityEngine;
using Random = UnityEngine.Random;

namespace ObjectManagement
{
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

            float angularSpeed = _configuration.angularSpeed.RandomValueRange;
            if (angularSpeed != 0f)
            {
                var rotation = instance.AddBehavior<RotationShapeBehavior>();
                rotation.angularVelocity = Random.onUnitSphere * angularSpeed;
            }

        
            float speed = _configuration.spawnSpeed.RandomValueRange;

            if (speed != 0)
            {
                var movement = instance.AddBehavior<MovementShapeBehavior>();
                movement.velocity = GetDirectionVector(_configuration.spawnMovementDirection,t) * speed;
            }

            SetupOscillation(instance);
            return instance;
        }

        private Vector3 GetDirectionVector(SpawnConfiguration.SpawnMovementDirection spawnMovementDirection,Transform t)
        {
            Vector3 direction;

            switch (spawnMovementDirection)
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

            return direction;
        }

        void SetupOscillation(Shape shape)
        {
            float amplitude = _configuration.oscillationAmplitude.RandomValueRange;
            float frequency = _configuration.oscillationFrequency.RandomValueRange;

            if (amplitude==0f|| frequency==0f)
            {
                return;
            }

            var oscillation = shape.AddBehavior<OscillationShapeBehavior>();
            oscillation.Offset = GetDirectionVector(_configuration.oscillationDirection, shape.transform);
            oscillation.Frequency = frequency;
        }
    
    }
}