Shader"Custom/ShockwaveEffect"
{
    Properties
    {
        _MainTex ("Screen Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Distortion ("Distortion Amount", Range(0, 1)) = 0.1
        _WaveSize ("Wave Size", Range(0, 5)) = 1
        _TimeFactor ("Time Factor", Range(0, 10)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

#include "UnityCG.cginc"

sampler2D _MainTex;
sampler2D _NormalMap;
float4 _MainTex_ST;
float4 _NormalMap_ST;
float _Distortion;
float _WaveSize;
float _TimeFactor;

struct appdata_t
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float2 screenUV : TEXCOORD1;
};

v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.screenUV = ComputeScreenPos(o.vertex).xy;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
                // 시간에 따라 충격파 반경 계산
    float timeFactor = _Time * _TimeFactor;
    float distance = length(i.screenUV - float2(0.5, 0.5));
    float wave = sin((distance - timeFactor) * _WaveSize) * 0.5 + 0.5;
    wave = smoothstep(0.4, 0.5, wave); // 범위 조정

                // 노말 맵을 사용한 UV 왜곡
    float2 normal = tex2D(_NormalMap, i.uv).xy * 2 - 1;
    float2 offset = normal * _Distortion * wave;
    float2 distortedUV = i.screenUV + offset;

                // 화면 텍스처 샘플링
    fixed4 color = tex2D(_MainTex, distortedUV);
    return color;
}
            ENDHLSL
        }
    }
}
