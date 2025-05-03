Shader "Custom/ShockwaveShaderB"
{
    Properties
    {
        _RefractionStrength ("Refraction Strength", Range(0, 0.1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
Name"RefractionPass"
            Tags
{"LightMode"="UniversalForward"
}

Blend SrcAlpha
OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
};

struct Varyings
{
    float4 positionHCS : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 viewDir : TEXCOORD1;
};

            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            CBUFFER_START(UnityPerMaterial)
float _RefractionStrength;
CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                float3 viewPos = TransformWorldToView(worldPos);

                OUT.viewDir = normalize(viewPos);

                return OUT;
            }

half4 frag(Varyings IN) : SV_Target
{
    float2 screenUV = saturate(IN.positionHCS.xy / _ScreenParams.xy);
    float2 distortion = IN.viewDir.xy * _RefractionStrength;
    float2 refractedUV = screenUV + distortion;

                // Opaque Texture에서 색상 샘플링
    half4 col = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, refractedUV);

    return col;
}
            ENDHLSL
        }
    }
}
