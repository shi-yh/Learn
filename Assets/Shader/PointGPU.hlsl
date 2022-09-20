#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
StructuredBuffer<float3> _Positions;
#endif

float2 _Scale;


///看不明白...
void ConfigureProcedural()
{
    #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
    ///通过使用当前正在绘制的实例的标识符为位置缓冲区建立索引来检索点的位置
    float3 position = _Positions[unity_InstanceID];

    unity_ObjectToWorld = 0.0;
    unity_ObjectToWorld._m03_m13_m23_m33  = float4(position,1.0);            
    unity_ObjectToWorld._m00_m11_m22  = _Scale.x;

    unity_WorldToObject = 0.0;
    unity_WorldToObject._m03_m13_m23_m33  = float4(-position,1.0);            
    unity_WorldToObject._m00_m11_m22  = _Scale.y;            
            
    #endif
}

void ShaderGraphFunction_float(float3 In, out float3 Out)
{
    Out = In;
}

void ShaderGraphFunction_Half(half3 In, out half3 Out)
{
    Out = In;
}
