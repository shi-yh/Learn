using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawnZone : SpawnZone
{
    [SerializeField] private bool _surfaceOnly;

    public override Vector3 SpawnPoint => transform.TransformPoint(_surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        ///默认Gizmos在世界空间中绘制，使用转置矩阵，将局部坐标转世界坐标
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero,1f);
    }
}