// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/My First Lighting Shader"
{
    Properties
    {
        _Tint ("Tint", Color) = (1,1,1,1)
        _MainTex("Albedo",2D) = "white"{}
        _Smoothness("Smoothness",Range(0,1)) = 0.5
        _Metallic("Metallic",Range(0,1))=0.1
    }
    SubShader
    {

        Pass
        {
            Tags
            {
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            #pragma target 3.0

            ///这里一共有两个选择，但是另一个选择没有关键字，所以使用_
            #pragma multi_compile _ VERTEXLIGHT_ON

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            #pragma FORWARD_BASE_PASS

            #include "My Lighting.cginc"
            ENDCG
        }

         Pass
        {
            Tags
            {
                "LightMode" = "ForwardAdd"
            }
            
            Blend One One
            ///上一个pass已经写入了深度数据，这里可以不用再写入
            ZWrite Off

            CGPROGRAM
            #pragma target 3.0
            #pragma multi_compile_fwdadd
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            
            #include "My Lighting.cginc"
            ENDCG
        }

    }
}