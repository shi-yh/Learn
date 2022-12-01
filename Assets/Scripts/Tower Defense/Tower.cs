using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : GameTileContent
{
    [SerializeField, Range(1.5f, 10.5f)] private float _targetingRange = 1.5f;

    [SerializeField] private Transform _turret = default, _laserBeam = default;

    [SerializeField] private float _damagePerSecond = 10f;

    private Vector3 _laserBeamScale;


    private TargetPoint _target;

    private const int enemyLayerMask = 1 << 9;

    private Collider[] _tempTargets = new Collider[100];

    private void Awake()
    {
        _laserBeamScale = _laserBeam.localScale;
    }

    public override void GameUpdate()
    {
        if (TrackTarget() || AcquireTarget())
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

    private bool TrackTarget()
    {
        if (_target == null)
        {
            return false;
        }

        Vector3 a = transform.localPosition;
        Vector3 b = _target.Position;

        float x = a.x - b.x;
        float z = a.z - b.z;

        float r = _targetingRange + 0.125f * _target.Enemy.Scale;

        ///勾股定理
        if (x * x + z * z > r * r)
        {
            _target = null;
            return false;
        }

        return true;
    }

    private bool AcquireTarget()
    {
        Vector3 a = transform.localPosition;
        Vector3 b = a;

        ///高度为3的圆柱体，
        b.y += 3f;

        var size = Physics.OverlapCapsuleNonAlloc(a, b, _targetingRange, _tempTargets, enemyLayerMask);

        if (size > 0)
        {
            _target = _tempTargets[Random.Range(0, size)].GetComponent<TargetPoint>();
        }
        else
        {
            _target = null;
        }


        return _target != null;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, _targetingRange);

        if (_target != null)
        {
            Gizmos.DrawLine(position, _target.Position);
        }
    }
}