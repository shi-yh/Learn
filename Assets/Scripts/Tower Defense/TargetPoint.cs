using UnityEngine;

namespace Tower_Defense
{
    [RequireComponent(typeof(SphereCollider))]
    public class TargetPoint : MonoBehaviour
    {
        public DefenceEnemy Enemy { get; private set; }

        public Vector3 Position => transform.position;

        private const int enemyLayerMask = 1 << 9;

        private static Collider[] s_buffer = new Collider[100];

        public static int BufferedCount { get; private set; }

        public static TargetPoint RandomBuffered()
        {
            return GetBuffered(Random.Range(0, BufferedCount));
        }

        public static bool FillBuffers(Vector3 position, float range)
        {
            Vector3 top = position;
            top.y += 3;

            BufferedCount = Physics.OverlapCapsuleNonAlloc(position, top, range, s_buffer, enemyLayerMask);

            return BufferedCount > 0;
        }

        public static TargetPoint GetBuffered(int index)
        {
            var target = s_buffer[index].GetComponent<TargetPoint>();

            return target;
        }


        private void Awake()
        {
            Enemy = transform.root.GetComponent<DefenceEnemy>();
            Debug.Assert(Enemy != null, "Target point without Enemy root", this);

            Enemy.TargetPointCollider = GetComponent<Collider>();

        }
    }
}