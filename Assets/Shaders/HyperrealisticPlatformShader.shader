Shader "Custom/HyperrealisticPlatformShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0.5, 0.5, 0.5, 1)
        _Metallic("Metallic", Range(0, 1)) = 0.2
        _Roughness("Roughness", Range(0, 1)) = 0.8
        _DetailAlbedoMap("Detail Albedo Map", 2D) = "white" {}
        _DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
        _DetailSmoothnessMask("Detail Smoothness Mask", 2D) = "white" {}
        _DetailScale("Detail Scale", Float) = 5
        _NormalMap("Normal Map", 2D) = "bump" {}
        _ParallaxMap("Parallax Map", 2D) = "black" {}
        _ParallaxHeight("Parallax Height", Float) = 0.05
        _EmissiveColor("Emissive Color", Color) = (0, 0.1, 0.2, 1)
        _EmissiveIntensity("Emissive Intensity", Float) = 0.1
        _VoidPulseRate("Void Pulse Rate", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "ForwardLit"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.6
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/BSDF.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float2 detailUV : TEXCOORD3;
                float3 tangentWS : TEXCOORD4;
                float3 bitangentWS : TEXCOORD5;
            };

            float4 _BaseColor;
            half _Metallic;
            half _Roughness;
            sampler2D _DetailAlbedoMap;
            sampler2D _DetailNormalMap;
            sampler2D _DetailSmoothnessMask;
            float _DetailScale;
            sampler2D _NormalMap;
            sampler2D _ParallaxMap;
            float _ParallaxHeight;
            float4 _EmissiveColor;
            float _EmissiveIntensity;
            float _VoidPulseRate;

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = input.uv;
                output.detailUV = input.uv * _DetailScale;
                output.tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz);
                output.bitangentWS = cross(output.normalWS, output.tangentWS) * input.tangentOS.w;
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float pulse = sin(_Time.y * _VoidPulseRate) * 0.5 + 0.5;
                return lerp(_BaseColor, _EmissiveColor * _EmissiveIntensity, pulse);
            }
            ENDHLSL
        }
    }
}
