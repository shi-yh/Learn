using UnityEngine;

namespace Tower_Defense
{
    public class MortarTower : Tower
    {
        [SerializeField, Range(0.5f, 2f)] private float _shootPerSecond = 1f;

        [SerializeField] private Transform _mortar = default;

        [SerializeField, Range(0.5f, 3f)] private float _shellBlastRadius = 1f;

        [SerializeField, Range(1f, 100f)] private float _shellDamage = 10f;

        public override TowerType TowerType => TowerType.Mortar;

        private float _launchProgress;

        float _launchSpeed;


        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            float x = _targetingRange + 0.25f;
            float y = -_mortar.position.y;
            _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
        }

        public override void GameUpdate()
        {
            _launchProgress += _shootPerSecond * Time.deltaTime;

            while (_launchProgress >= 1)
            {
                if (AcquireTarget(out TargetPoint target))
                {
                    Launch(target);
                    _launchProgress -= 1f;
                }
                else
                {
                    _launchProgress = 0.999f;
                }
            }
        }

        public void Launch(TargetPoint target)
        {
            Vector3 launchPoint = _mortar.position;
            Vector3 targetPoint = target.Position;
            targetPoint.y = 0;

            ///这一大段是计算抛物线的
            Vector2 dir;
            dir.x = targetPoint.x - launchPoint.x;
            dir.y = targetPoint.z - launchPoint.z;

            float x = dir.magnitude;
            float y = -launchPoint.y;
            //得到单位向量
            dir /= x;

            float g = 9.81f;
            float s = _launchSpeed;
            float s2 = s * s;

            float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
            float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
            float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
            float sinTheta = cosTheta * tanTheta;


            _mortar.localRotation =
                Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));

            DefenceGame.SpawnShell().Initialized(launchPoint, targetPoint,
                new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y), _shellBlastRadius, _shellDamage);
        }
    }
}