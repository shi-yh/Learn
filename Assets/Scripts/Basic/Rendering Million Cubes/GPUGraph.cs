using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    [SerializeField, Range(10, 1000)] private int resolution = 10;

    [SerializeField] private FunctionLibrary.Function3DName _functionName = default;

    [SerializeField] private ComputeShader _computeShader = default;

    [SerializeField] private Material _material = default;

    [SerializeField] private Mesh _mesh = default;

    [SerializeField] private float _transitionDuration = 1;


    private const int maxResolution = 1000;

    private bool _transitioning;

    private float _duration;

    private FunctionLibrary.Function3DName _transitionFunction;


    private static readonly int
        positionId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time"),
        scaleId = Shader.PropertyToID("_Scale"),
        translationProgressId = Shader.PropertyToID("_TransitionProgress");


    private ComputeBuffer _positionBuffer;


    /// <summary>
    /// 每帧的时候改变内核参数，计算位置
    /// </summary>
    void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;


        _computeShader.SetInt(resolutionId, resolution);
        _computeShader.SetFloat(stepId, step);
        _computeShader.SetFloat(timeId, Time.time);

        if (_transitioning)
        {
            _computeShader.SetFloat(translationProgressId, Mathf.SmoothStep(0f, 1f, _duration / _transitionDuration));
        }

        var kernelIndex = (int) _functionName + (int) (_transitioning ? _transitionFunction : _functionName) * FunctionLibrary.GetFunctionCount();

        _computeShader.SetBuffer(kernelIndex, positionId, _positionBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);

        ///我们可以通过使用四个整数参数在compute shader上调用Dispatch来运行内核
        /// 由于我们固定的8×8群组大小，我们在X和Y维度上需要的群组数量等于分辨率除以8（四舍五入）
        _computeShader.Dispatch(kernelIndex, groups, groups, 1);

        _material.SetBuffer(positionId, _positionBuffer);
        _material.SetVector(scaleId, new Vector4(step, 1f / step));
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));

        Graphics.DrawMeshInstancedProcedural(_mesh, 0, _material, bounds, resolution * resolution);
    }

    private void Update()
    {
        Transition();
        UpdateFunctionOnGPU();
    }

    private void Transition()
    {
        if (!_transitioning)
        {
            _duration += Time.deltaTime;

            if (_duration >= _transitionDuration)
            {
                _transitionFunction = _functionName;

                _functionName = FunctionLibrary.GetNextFunctionName(_functionName);

                _duration = 0;

                _transitioning = true;
            }
        }
        else
        {
            _duration += Time.deltaTime;

            if (_duration >= _transitionDuration)
            {
                _transitioning = false;

                _duration = 0;
            }
        }
    }


    private void OnEnable()
    {
        ///存储3D位置矢量，有三个浮点数，每个浮点数的大小是四个字节
        _positionBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4);
    }

    private void OnDisable()
    {
        _positionBuffer.Release();
        _positionBuffer = null;
    }
}