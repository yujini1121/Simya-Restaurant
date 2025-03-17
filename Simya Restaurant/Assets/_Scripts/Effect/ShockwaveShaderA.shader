Shader"Custom/ShockwaveShaderA"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Distortion Strength", Range(0, 0.1)) = 0.02
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 3.0
        _WaveFrequency ("Wave Frequency", Range(0, 50)) = 10.0
        _ShockwaveRadius ("Shockwave Radius", Range(0, 5)) = 1.0
        _ShockwaveWidth ("Shockwave Width", Range(0, 1)) = 0.2
        _TimeMultiplier ("Time Multiplier", Range(0, 5)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            Blend SrcAlpha
            OneMinusSrcAlpha
                        ZWrite
            Off
                        Cull
            Back

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
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _CameraOpaqueTexture;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Distortion;
            float _WaveSpeed;
            float _WaveFrequency;
            float _ShockwaveRadius;
            float _ShockwaveWidth;
            float _TimeMultiplier;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                return OUT;
            }

            float remap(float value, float inMin, float inMax, float outMin, float outMax)
            {
                return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 screenUV = IN.positionCS.xy / _ScreenParams.xy;
                float3 worldPos = IN.worldPos;
                
                            // 중심에서의 거리 계산
                float2 center = float2(0.5, 0.5); // 중심점 설정
                float dist = distance(screenUV, center);
                
                            // 시간 기반으로 Shockwave 애니메이션
                float time = _Time.y * _TimeMultiplier;
                float wave = sin((dist - time * _WaveSpeed) * _WaveFrequency) * 0.5 + 0.5;
                
                            // 충격파 거리 기반으로 왜곡 적용
                float shockwave = smoothstep(_ShockwaveRadius, _ShockwaveRadius + _ShockwaveWidth, dist);
                float distortion = wave * shockwave * _Distortion;
                
                            // UV 변형 (왜곡 적용)
                float2 distortedUV = screenUV + (screenUV - center) * distortion;
                
                            // 원본 화면 샘플링
                half4 color = tex2D(_CameraOpaqueTexture, distortedUV);

                return color;
            }
            ENDHLSL
        }
    }
}
