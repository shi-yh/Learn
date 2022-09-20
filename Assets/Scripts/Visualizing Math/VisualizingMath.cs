using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizingMath : MonoBehaviour
{
    [SerializeField] private Transform _point;

    [SerializeField, Range(10, 100)] private int resolution = 10;

    [SerializeField]
    private FunctionLibrary.Function2DName _functionName = default;
    
    private Transform[] _points;
    
    private void Awake()
    {
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        var position = Vector3.zero;

        _points = new Transform[resolution];
        
        for (int i = 0; i < resolution; i++)
        {
            Transform point = Instantiate(_point, transform, true);
            position.x = (i + 0.5f) * step - 1f;
            
            point.localPosition = position;

            point.localScale = scale;

            _points[i] = point;
        }
    }


    private void Update()
    {
        float time = Time.time;
        for (int i = 0; i < resolution; i++)
        {
            Vector3 pointPos = _points[i].position;

            pointPos.y = FunctionLibrary.GetFunction2D(_functionName)(pointPos.x,time);

            _points[i].position = pointPos;
        }
    }
}