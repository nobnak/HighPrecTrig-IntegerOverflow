Shader "Hidden/Sine" {
    Properties {
        _PixelFreq ("Pixel Frequency", Float) = 0.1
        _TimeScale ("Time Scale", Float) = 1.0
        _SimTimeOffset ("Sim Time Offset", Float) = 0.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off
        Pass {
            Name "ColorBlitPass"

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // The Blit.hlsl file provides the vertex shader (Vert),
            // input structure (Attributes) and output strucutre (Varyings)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            //#define TWO_PI 6.28318530718

            CBUFFER_START(UnityPerMaterial)
            float _PixelFreq;
            float _TimeScale;
            float _SimTimeOffset;
            CBUFFER_END

            #include "TickConversion.hlsl"

            float4 frag (Varyings input) : SV_Target {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 pixelCoord = input.texcoord * _ScreenParams.xy;
                uint i = pixelCoord.x + floor(pixelCoord.y/20) * _ScreenParams.x;

                uint tick = (uint)(_GTick * _TimeScale) 
                    + i * _PixelFreq 
                    + SecondsToTick(_SimTimeOffset);
                float time = _Time.y * _TimeScale 
                    + TickToSeconds(i * _PixelFreq) 
                    + _SimTimeOffset;

                if (input.texcoord.x < 0.5) {
                    time = TickToSeconds(tick);
                }
                float sine = sin(time * TWO_PI);
                float4 col = float4(sine, -sine, abs(sine), 1.0);
                
                return col;
            }
            ENDHLSL
        }
    }
}
