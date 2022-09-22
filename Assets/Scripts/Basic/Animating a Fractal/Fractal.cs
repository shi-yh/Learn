using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    [SerializeField, Range(1, 8)] private int _depth;

    private void Start()
    {
        name = "Fractal" + _depth;

        if (_depth<=1)
        {
            return;;
        }

        Fractal childA = CreateChild(Vector3.up, Quaternion.identity);
        Fractal childB = CreateChild(Vector3.right, Quaternion.Euler(0,0,-90));
        Fractal childC = CreateChild(Vector3.left, Quaternion.Euler(0,0,90));
        Fractal childD = CreateChild(Vector3.forward, Quaternion.Euler(90,0,0));
        Fractal childE = CreateChild(Vector3.back, Quaternion.Euler(-90,0,0));

        childA.transform.SetParent(transform,false);
        childB.transform.SetParent(transform,false);
        childC.transform.SetParent(transform,false);
        childD.transform.SetParent(transform,false);
        childE.transform.SetParent(transform,false);
        
        
    }

    Fractal CreateChild(Vector3 direction, Quaternion rotation)
    {
        Fractal child = Instantiate(this);

        child._depth = _depth - 1;

        var transform1 = child.transform;
        transform1.localPosition = 0.75f * direction;
        transform1.rotation = rotation;
        transform1.localScale = 0.5f * Vector3.one;

        return child;
    }

    private void Update()
    {
        transform.Rotate(0f,22.5f*Time.deltaTime,0);
    }
}