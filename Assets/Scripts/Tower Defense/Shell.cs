using UnityEngine;

namespace Tower_Defense
{
    public class Shell : DefenceWarEntity
    {
        private Vector3 _launchPoint, _targetPoint, _lunchVelocity;

        private float _age, _blastRadius, _damage;

        public void Initialized(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity,
            float blastRadius, float damage)
        {
            _launchPoint = launchPoint;
            _targetPoint = targetPoint;
            _lunchVelocity = launchVelocity;
            _blastRadius = blastRadius;
            _damage = damage;
        }

        public override bool GameUpdate()
        {
            _age += Time.deltaTime;

            Vector3 p = _launchPoint + _lunchVelocity * _age;

            p.y -= 0.5f * 9.81f * _age * _age;


            if (p.y <= 0f)
            {
                DefenceGame.SpawnExplosion().Initialize(_targetPoint, _blastRadius, _damage);
                OriginFactory.Reclaim(this);
                return false;
            }


            transform.localPosition = p;

            Vector3 d = _lunchVelocity;
            d.y -= 9.81f * _age;
            transform.localRotation = Quaternion.LookRotation(d);

            DefenceGame.SpawnExplosion().Initialize(p, 0.2f);
            return true;
        }
    }
}