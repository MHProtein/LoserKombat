Shader "Unlit/Grass"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _TipColor("Tip Color", Color) = (1, 1, 1, 1)
        _BladeTexture("Blade Texture", 2D) = "White" {}
        
        _BladeWidthMin("Blade Width (Min)", Range(0, 0.1)) = 0.02
        _BladeWidthMax("Blade Width (Max)", Range(0, 0.1)) = 0.05
        _BladeHeightMin("Blade Height (Min)", Range(0, 2)) = 0.05
        _BladeHeightMax("Blade Height (Max)", Range(0, 2)) = 0.05
        
        _BladeSegments("Blade Segments", Range(0, 10)) = 3
        _BladeBendDistance("Blade Forward Amount", Float) = .38
        _BladeBendCurve("Blade Curvature Amount", Range(1, 4)) = 2
        
        _BladeDelta("Blade Variation", Range(0, 1)) = .2
        
        _TessellationGrassDistance("Tessellation Grass Distance", Range(.01, 2)) = .1
        
        _GrassMap("Grass Visibility Map", 2D) = "white" {}
        _GrassThreshold("Grass Visibility Threshold", Range(-0.1, 1)) = .5
        _GrassFalloff("Grass Visibility Fade-In Falloff", Range(0, .5)) = .05
        
        _WindMap("Wind Offset Map", 2D) = "bump" {}
        _WindVelocity("Wind Velocity", Vector) = (1, 0, 0, 0)
        _WindFrequency("Grass Pulse Frequency", Range(0, 1)) = .01
        
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100
        Cull Off
        
        HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _TipColor;
                sampler2D _BladeTexture;

                float _BladeWidthMin;
                float _BladeWidthMax;
                float _BladeHeightMin;
                float _BladeHeightMax;

                float _BladeBendDistance;
                float _BladeBendCurve;

                float _BladeDelta;

                float _TessellationGrassDistance;

                sampler2D _GrassMap;
                float4 _GrassMap_ST;
                float _GrassThreshold;
                float _GrassFalloff;

                sampler2D _WindMap;
                float4 _WindMap_ST;
                float4 _WindVelocity;
                float _WindFrequency;

                float4 _ShadowColor;
            CBUFFER_END

            struct VertexInput
            {
                float4 vertex  : POSITION;
                float3 normal  : NORMAL;
                float4 tangent : TANGENT;
                float2 uv      : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 vertex  : SV_POSITION;
                float3 normal  : NORMAL;
                float4 tangent : TANGENT;
                float2 uv      : TEXCOORD0;
            };

            struct TessellationFactors
            {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
            };
            
            struct GeomData
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float rand(float3 co)
            {
                return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
            }

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.normal = v.normal;
                o.tangent = v.tangent;
                o.uv = TRANSFORM_TEX(v.uv, _GrassMap);
                return o;
            }
            
        ENDHLSL
        
        Pass
        {
            Name "GrassPass"
            Tags{ "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 frag(VertexOutput i) : SV_Target
            {
                return float4(1.0f, 1.0f, 1.0f, 1.0f);
            }
            
            ENDHLSL
        }

    }
}
