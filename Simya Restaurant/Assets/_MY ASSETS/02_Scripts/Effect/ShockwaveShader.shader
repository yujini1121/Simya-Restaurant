Shader"Custom/ShockwaveEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Distortion Strength", Range(0, 0.1)) = 0.05
        _WaveCenter ("Wave Center", Vector) = (0.5,0.5,0,0)
        _WaveRadius ("Wave Radius", Range(0, 1)) = 0.3
        _WaveWidth ("Wave Width", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        GrabPass { "_GrabTexture" }

        // 직접 화면 텍스처를 사용하는 방식
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata_t
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 screenPos : TEXCOORD1;
};

sampler2D _MainTex;
sampler2D _GrabTexture; // 직접 화면 텍스처를 사용하는 변수
float _Distortion;
float4 _WaveCenter;
float _WaveRadius;
float _WaveWidth;

v2f vert(appdata_t v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    o.screenPos = ComputeScreenPos(o.pos);
    return o;
}

float2 RadialDistortion(float2 uv, float2 center, float radius, float width, float strength)
{
    float dist = distance(uv, center);
    float wave = exp(-pow((dist - radius) / width, 2));
    return uv + (uv - center) * wave * strength;
}

fixed4 frag(v2f i) : SV_Target
{
    float2 screenUV = i.screenPos.xy / i.screenPos.w;
    screenUV = RadialDistortion(screenUV, _WaveCenter.xy, _WaveRadius, _WaveWidth, _Distortion);

                // 화면 텍스처 샘플링
    fixed4 col = tex2D(_GrabTexture, screenUV);
    return col;
}
            ENDCG
        }
    }
}
