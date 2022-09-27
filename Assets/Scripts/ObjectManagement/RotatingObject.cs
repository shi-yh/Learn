using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : PersistableObject
{
    [SerializeField] private Vector3 _angularVelocity;

    private void Update()
    {
        transform.Rotate(_angularVelocity * Time.deltaTime);
    }
}