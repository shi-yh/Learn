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

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityPBSLighting.cginc"

            struct Interpolators
            {
                float4 position:SV_POSITION;
                float3 normal:TEXCOORD1;
                float2 uv:TEXCOORD0;
                float3 worldPos:TEXCOORD2;
            };

            struct VertexData
            {
                float4 position:POSITION;
                float2 uv:TEXCOORD0;
                float3 normal:NORMAL;
            };

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Smoothness;
            float _Metallic;

            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;

                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                i.position = UnityObjectToClipPos(v.position);
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.normal = normalize(i.normal);
                i.worldPos = mul(unity_ObjectToWorld, v.position);
                return i;
            }

            float4 MyFragmentProgram(Interpolators i):SV_Target
            {

             
                
                
                i.normal = normalize(i.normal);
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 halfVector = normalize(lightDir + viewDir);

                float3 lightColor = _LightColor0.xyz;

                float oneMinusReflectivity;


                float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;


                float3 specularTint;
                albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);

                float3 specular = specularTint.rgb * lightColor * pow(DotClamped(halfVector, i.normal),
                                                                      _Smoothness * 100);

                albedo = EnergyConservationBetweenDiffuseAndSpecular(albedo, specularTint.rgb, oneMinusReflectivity);

                float3 diffuse = DotClamped(lightDir, i.normal) * lightColor * albedo * lightColor;
                
                return float4(diffuse+specular,1);
            }
            ENDCG
        }
    }
}