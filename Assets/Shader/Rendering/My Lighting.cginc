#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED
#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"

struct Interpolators
{
    float4 position:SV_POSITION;
    float3 normal:TEXCOORD1;
    float2 uv:TEXCOORD0;
    float3 worldPos:TEXCOORD2;

    #if defined(VERTEXLIGHT_ON)
    float3 vertexLightColor:TEXCOORD3;
    #endif
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

void ComputeVertexLightColor(inout Interpolators i)
{
    #if defined(VERTEXLIGHT_ON)
    i.vertexLightColor = Shade4PointLights(unity_4LightPosX0,unity_4LightPosY0,unity_4LightPosZ0,unity_LightColor[0],
       unity_LightColor[1].rgb,unity_LightColor[2].rgb,unity_LightColor[3].rgb,unity_4LightAtten0,i.worldPos,i.normal);
    
    #endif
}

UnityIndirect CreateIndirectLight(Interpolators i)
{
    UnityIndirect indirectLight;
    indirectLight.diffuse = 0;
    indirectLight.specular = 0;

    ///将点光源产生的颜色假定为环境光
    #if defined(VERTXELIGHT_ON)
    indirectLight.diffuse = i.vertexLightColor;
    #endif

    #if defined(FORWARD_BASE_PASS)
    indirectLight.diffuse+=max(0,ShadeSH9(float4(i.normal,1)));
    #endif
    
    return indirectLight;
}

UnityLight CreateLight(Interpolators i)
{
    UnityLight light;
    #if defined(POINT) || defined(SPOT)|| defined(POINT_COOKIE)

    light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);

    #else
    light.dir = _WorldSpaceLightPos0.xyz;


    #endif


    ///半径为r的球的表面积为4πr²，要确定光子密度，可以除以它，也就是1/r²,因为4π是常数，所以忽略
    ///但是这会导致在距离无限接近0时，光的强度趋向无穷大，所以改为1/(1+r²)
    //float3 lightVec = _WorldSpaceLightPos0.xyz - i.worldPos;
    //float attenuation = 1 / (1 + dot(lightVec, lightVec));


    UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos)

    light.color = _LightColor0.rgb * attenuation;
    light.ndotl = DotClamped(i.normal, light.dir);
    return light;
}

Interpolators MyVertexProgram(VertexData v)
{
    Interpolators i;

    i.uv = TRANSFORM_TEX(v.uv, _MainTex);
    i.position = UnityObjectToClipPos(v.position);
    i.normal = UnityObjectToWorldNormal(v.normal);
    i.normal = normalize(i.normal);
    i.worldPos = mul(unity_ObjectToWorld, v.position);
    ComputeVertexLightColor(i);
    return i;
}

float4 MyFragmentProgram(Interpolators i):SV_Target
{
    i.normal = normalize(i.normal);
    float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

    float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;

    float3 specularTint;
    float oneMinusReflectivity;
    albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);


    return BRDF3_Unity_PBS(albedo, specularTint, oneMinusReflectivity, _Smoothness, i.normal, viewDir, CreateLight(i),
                           CreateIndirectLight(i));
}

#endif
