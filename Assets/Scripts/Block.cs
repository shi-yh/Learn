using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private GameObject _temp;

    [SerializeField]
    private int _resolution;

    [SerializeField]
    private FunctionName _functionName;

    [SerializeField,Min(0f)]
    private float _functionDuration = 1f;

    [SerializeField,Min(0f)]
    private float _transitionDuration = 1;

    private bool _translationing;

    private FunctionName _transitionFunction;

    private float _duration;

    Transform[] points;

    // Start is called before the first frame update
    void Start()
    {
        points = new Transform[_resolution * _resolution];
        float step = 2.0f / _resolution;
        var scale = Vector3.one * step;

        for (int i = 0; i <points.Length; i++)
        {
           
            Transform trs = Instantiate(_temp, transform).transform;

            trs.localScale = scale;
            points[i] = trs;
        }
    }

    void Update()
    {
        _duration += Time.deltaTime;


        if (_translationing)
        {
            if (_duration>=_transitionDuration)
            {
                _duration -= _transitionDuration;
                _translationing = false;
            }
        }
        else if (_duration>=_functionDuration)
        {
            _duration -= _functionDuration;
            _translationing = true;

            _transitionFunction = _functionName;
            _functionName = FuncitionLibrary.GetRandomFUnctionNameOtherThan(_functionName);
        }

        if (_translationing)
        {
            UpdateFunctionTransition();
        }
        else
        {
            UpdateFunction();
        }

    }


    private void UpdateFunctionTransition()
    {
        FuncitionLibrary.Function
            from = FuncitionLibrary.GetFunction(_transitionFunction),
            to = FuncitionLibrary.GetFunction(_functionName);
        float progress = _duration / _transitionDuration;

        float time = Time.time;
        float step = 2f / _resolution;

        float v = 0.5f * step - 1f;

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == _resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1;
            }

            float u = (x + 0.5f) * step - 1f;


            points[i].localPosition = FuncitionLibrary.Morph(u, v, time,from,to,progress);

        }
    }



    private void UpdateFunction()
    {
        

        FuncitionLibrary.Function f = FuncitionLibrary.GetFunction(_functionName);
        float time = Time.time;
        float step = 2f / _resolution;

        float v = 0.5f * step - 1f;

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == _resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1;
            }

            float u = (x + 0.5f) * step - 1f;


            points[i].localPosition = f(u, v, time);

        }
    }
}
