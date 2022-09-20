Shader "Custom/PointSurface GPU"
{
    Properties
    {
        _Smoothness("Smoothness",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows addshadow
        ///表面着色器需要为每个顶点调用一个配置函数
        #pragma instancing_options procedural:ConfigureProcedural
        #pragma editor_sync_compilation

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 4.5

        #include "PointGPU.hlsl"

        
        float _Smoothness;
        
     

        struct Input
        {
            float3 worldPos;
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


      
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Smoothness = _Smoothness;
            o.Albedo = IN.worldPos * 0.5 + 0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}