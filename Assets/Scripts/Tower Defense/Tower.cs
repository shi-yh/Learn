using UnityEngine;

namespace Tower_Defense
{
    public enum TowerType
    {
        Laser,
        Mortar
    }

    public abstract class Tower : GameTileContent
    {
        [SerializeField, Range(1.5f, 10.5f)] protected float _targetingRange = 1.5f;

        public abstract TowerType TowerType { get; }

        protected bool TrackTarget(ref TargetPoint target)
        {
            if (target == null)
            {
                return false;
            }

            Vector3 a = transform.localPosition;
            Vector3 b = target.Position;

            float x = a.x - b.x;
            float z = a.z - b.z;

            float r = _targetingRange + 0.125f * target.Enemy.Scale;

            ///勾股定理
            if (x * x + z * z > r * r)
            {
                target = null;
                return false;
            }

            return true;
        }

        protected bool AcquireTarget(out TargetPoint target)
        {
            if (TargetPoint.FillBuffers(transform.localPosition, _targetingRange))
            {
                target = TargetPoint.RandomBuffered();
            }
            else
            {
                target = null;
            }

            return target != null;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 position = transform.localPosition;
            position.y += 0.01f;
            Gizmos.DrawWireSphere(position, _targetingRange);
        }
    }
}