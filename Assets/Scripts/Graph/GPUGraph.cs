using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    private const int maxResolution = 1000;

  
    /// <summary>
    /// 设置一些computeshader的属性
    /// </summary>
    private static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time"),
        transitionProgressId = Shader.PropertyToID("_TransitionProgress");


    /// <summary>
    /// 分辨率
    /// </summary>
    [SerializeField,Range(10,maxResolution)]
    private int _resolution;
    /// <summary>
    /// 当前使用的函数
    /// </summary>
    [SerializeField]
    private FunctionName _functionName;
    /// <summary>
    /// 函数变换间隔
    /// </summary>
    [SerializeField, Min(0f)]
    private float _functionDuration = 1f;
    /// <summary>
    /// 函数变换的缓冲时间
    /// </summary>
    [SerializeField, Min(0f)]
    private float _transitionDuration = 1;

    [SerializeField]
    /// <summary>
    /// 当前使用的材质
    /// </summary>
    private Material _materia;

    /// <summary>
    /// 当前使用的网格
    /// </summary>
    [SerializeField]
    private Mesh _mesh;


    [SerializeField]
    private ComputeShader _computerShader;

    private ComputeBuffer _positionBuffer;



    /// <summary>
    /// 当前是否处在缓冲状态
    /// </summary>
    private bool _translationing;

    /// <summary>
    /// 上一次的函数类型
    /// </summary>
    private FunctionName _transitionFunction;

    /// <summary>
    /// 当前变换计时
    /// </summary>
    private float _duration;





    void UpdateFunctionOnGPU()
    {
        float step = 2f / _resolution;
        _computerShader.SetInt(resolutionId, _resolution);
        _computerShader.SetFloat(stepId, step);
        _computerShader.SetFloat(timeId, Time.time);
        if (_translationing)
        {
            _computerShader.SetFloat(
                transitionProgressId,
                Mathf.SmoothStep(0f, 1f, _duration / _transitionDuration)
            );
        }


        int kernelIndex = (int)_functionName+(int)(_translationing?_transitionFunction:_functionName)*5;

        Debug.Log(kernelIndex);

        _computerShader.SetBuffer(kernelIndex, positionsId, _positionBuffer);

        int groups = Mathf.CeilToInt(_resolution / 8f);


        _computerShader.Dispatch(kernelIndex, groups, groups, 1);

        _materia.SetBuffer(positionsId, _positionBuffer);
        _materia.SetFloat(stepId, step);

        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f+2f/_resolution));

        Graphics.DrawMeshInstancedProcedural(_mesh, 0, _materia,bounds,_positionBuffer.count);

    }

    private void OnEnable()
    {
        ///为了将位置存储在GPU上，需要分配空间
        ///第一个参数为我们需要的点的数量
        ///第二个参数：指定元素的确切大小【字节为单位】，3D位置矢量由三个浮点数构成，一个浮点数是四个字节
        _positionBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4);
    }

    private void OnDisable()
    {
        _positionBuffer.Release();
        _positionBuffer = null;
    }


    void Update()
    {
        _duration += Time.deltaTime;

        if (_translationing)
        {
            if (_duration >= _transitionDuration)
            {
                _duration -= _transitionDuration;
                _translationing = false;
            }
        }
        else if (_duration >= _functionDuration)
        {
            _duration -= _functionDuration;
            _translationing = true;

            _transitionFunction = _functionName;
            _functionName = FuncitionLibrary.GetRandomFUnctionNameOtherThan(_functionName);
        }


        UpdateFunctionOnGPU();

    }





}
