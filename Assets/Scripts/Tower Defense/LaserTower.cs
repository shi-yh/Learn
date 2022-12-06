using UnityEngine;

namespace Tower_Defense
{
    public class LaserTower : Tower
    {
        [SerializeField] private Transform _turret = default, _laserBeam = default;

        [SerializeField] private float _damagePerSecond = 10f;

        private Vector3 _laserBeamScale;

        private TargetPoint _target;

        public override TowerType TowerType => TowerType.Laser;


        private void Awake()
        {
            _laserBeamScale = _laserBeam.localScale;
        }


        public override void GameUpdate()
        {
            if (TrackTarget(ref _target) || AcquireTarget(out _target))
            {
                Shoot();
            }
            else
            {
                _laserBeam.localScale = Vector3.zero;
            }
        }

        private void Shoot()
        {
            Vector3 point = _target.Position;
            _turret.LookAt(point);
            _laserBeam.localRotation = _turret.localRotation;

            float d = Vector3.Distance(_turret.position, point);
            _laserBeamScale.z = d;
            _laserBeam.localScale = _laserBeamScale;
            _laserBeam.localPosition = _turret.localPosition + 0.5f * d * _laserBeam.forward;

            _target.Enemy.ApplyDamage(_damagePerSecond * Time.deltaTime);
        }
    }
}