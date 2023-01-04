// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Textured With Detail"
{
    Properties
    {
        _Tint ("Tint", Color) = (1,1,1,1)
        _MainTex("MainTex",2D) = "white"{}
        _DetailTex("Detail Texture",2D) = "gray"{}
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
                float2 uvDetail:TEXCOORD1;
            };

            struct VertexData
            {
                float4 position:POSITION;
                float2 uv:TEXCOORD0;
            };

            float4 _Tint;
            sampler2D _MainTex,_DetailTex;
            float4 _MainTex_ST,_DetailTex_ST;

            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;

                i.uv = TRANSFORM_TEX(v.uv,_MainTex);
                i.position = UnityObjectToClipPos(v.position);
                i.uvDetail = TRANSFORM_TEX(v.uv,_DetailTex);
                return i;
            }

            ///合并两个纹理的方式是将两个纹理相乘
            ///当两个纹理合并后，除非有一个纹理是纯白图片，否则他们会变暗，因为每个纹理通道的值是介于0-1的
            float4 MyFragmentProgram(Interpolators i):SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv);
                color *= tex2D(_DetailTex,i.uvDetail)*2;
                return color;
            }
            ENDCG
        }
    }
}