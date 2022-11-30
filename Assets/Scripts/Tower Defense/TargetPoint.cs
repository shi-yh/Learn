using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TargetPoint : MonoBehaviour
{

    public DefenceEnemy Enemy { get; private set; }

    public Vector3 Position => transform.position;


    private void Awake()
    {
        Enemy.transform.root.GetComponent<DefenceEnemy>();
        Debug.Assert(Enemy!=null,"Target point without Enemy root",this);
    }
}
