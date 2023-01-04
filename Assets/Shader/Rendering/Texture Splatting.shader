// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Texture Splatting"
{
    Properties
    {
        _MainTex("Splat Map",2D) = "white"{}
        [NoScaleOffset] _Texture1("Texture 1",2D) = "white"{}
        [NoScaleOffset] _Texture2("exture 2",2D) = "white"{}
        [NoScaleOffset] _Texture3("exture 2",2D) = "white"{}
        [NoScaleOffset] _Texture4("exture 2",2D) = "white"{}
    }
    SubShader
    {

        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"

            struct Interpolators
            {
                float4 position:SV_POSITION;
                float2 uv:TEXCOORD0;
                float uvSplat:TEXCOORD1;
            };

            struct VertexData
            {
                float4 position:POSITION;
                float2 uv:TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _Texture1,_Texture2,_Texture3,_Texture4;
            

            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;

                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                i.position = UnityObjectToClipPos(v.position);
                i.uvSplat = v.uv;
                return i;
            }

            float4 MyFragmentProgram(Interpolators i):SV_Target
            {
                float4 splat = tex2D(_MainTex,i.uv);
                return
                tex2D(_Texture1, i.uv)*splat.r
                +tex2D(_Texture2,i.uv)*splat.g
                +tex2D(_Texture3,i.uv)*splat.b
                +tex2D(_Texture4,i.uv)*(1-splat.b-splat.r-splat.g);
            }
            ENDCG
        }
    }
}