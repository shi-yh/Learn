using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    [SerializeField, Range(10, 10000)] private int resolution = 10;

    [SerializeField] private FunctionLibrary.Function3DName _functionName = default;

    [SerializeField] private ComputeShader _computeShader = default;

    [SerializeField] private Material _material = default;
    
    [SerializeField] private Mesh _mesh = default;

    private static readonly int
        positionId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time"),
        scaleId = Shader.PropertyToID("_Scale");


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
        _computeShader.SetBuffer(0, positionId, _positionBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);

        ///我们可以通过使用四个整数参数在compute shader上调用Dispatch来运行内核
        /// 由于我们固定的8×8群组大小，我们在X和Y维度上需要的群组数量等于分辨率除以8（四舍五入）
        _computeShader.Dispatch(0, groups, groups, 1);
        
        _material.SetBuffer(positionId,_positionBuffer);
        _material.SetVector(scaleId,new Vector4(step,1f/step));
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f+2f/resolution));
        
        Graphics.DrawMeshInstancedProcedural(_mesh,0,_material,bounds,_positionBuffer.count);
    }

    private void Update()
    {
        UpdateFunctionOnGPU();
        
       
        
    }

    private void OnEnable()
    {
        ///存储3D位置矢量，有三个浮点数，每个浮点数的大小是四个字节
        _positionBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
    }

    private void OnDisable()
    {
        _positionBuffer.Release();
        _positionBuffer = null;
    }
}