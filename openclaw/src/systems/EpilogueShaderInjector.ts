/**
 * @fileoverview HDRP Shader Ingestion Service
 * Architecture: Milehigh.world: Into the Void Framework
 * Purpose: Encapsulates HLSL code for the "Memory Flood" reality transition.
 */

export interface ShaderPayload {
    shaderName: string;
    hlslCode: string;
    targetMaterialProperty: string;
}

export class EpilogueShaderInjector {
    private readonly memoryFloodShader: ShaderPayload;

    constructor() {
        this.memoryFloodShader = this.constructShaderPayload();
    }

    /**
     * Encapsulates the raw Unity HDRP HLSL logic for the reality transition.
     * The shader interpolates between the Void Calamity aesthetic and the restored Millennia.
     */
    private constructShaderPayload(): ShaderPayload {
        const hlslString = `
Shader "HDRP/MilehighWorld/MemoryFloodRestoration"
{
    Properties
    {
        _MainTex ("Base Map", 2D) = "white" {}
        _VoidCorruption ("Void Corruption Level", Range(0, 1)) = 1.0
        _RestorationGlow ("Restoration Glow Color", Color) = (1, 0.9, 0.7, 1) // Warm acoustic glow
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="HDRenderPipeline" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _VoidCorruption;
            float4 _RestorationGlow;

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS);
                output.uv = input.uv;
                return output;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                // Sample base environment texture
                float4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);

                // Define Void aesthetic (desaturated, aggressive digital artifacts)
                float luminance = dot(baseColor.rgb, float3(0.299, 0.587, 0.114));
                float4 voidColor = float4(luminance * 0.4, luminance * 0.2, luminance * 0.6, 1.0);

                // Define the restored "Now" aesthetic (warm, vibrant, Memory Flood illumination)
                float4 restoredColor = baseColor * _RestorationGlow;

                // Interpolate based on the system's current corruption state
                return lerp(restoredColor, voidColor, _VoidCorruption);
            }
            ENDHLSL
        }
    }
}
        `;

        return {
            shaderName: "MemoryFloodRestoration",
            hlslCode: hlslString.trim(),
            targetMaterialProperty: "_VoidCorruption"
        };
    }

    /**
     * Executes the pipeline ingestion. This method simulates passing the payload
     * to the rendering engine API bridge to compile and apply the shader.
     */
    public async injectToRenderingEngine(bridgeApi: any): Promise<void> {
        try {
            console.log(`[ShaderInjector] Ingesting HDRP Shader: ${this.memoryFloodShader.shaderName}`);

            // API call to the Unity WebGL/Runtime bridge
            await bridgeApi.compileAndAssignShader(this.memoryFloodShader.hlslCode);

            console.log("[ShaderInjector] Shader successfully ingested and compiled by the machine.");
        } catch (error) {
            console.error("[ShaderInjector] Critical fault during shader ingestion.", error);
            throw error;
        }
    }

    /**
     * Animates the shader property to trigger the visual Memory Flood.
     */
    public async triggerMemoryFloodVisuals(bridgeApi: any, durationInSeconds: number): Promise<void> {
        console.log("[ShaderInjector] Initiating Void purge visualization...");
        // Instruct the bridge to lerp the _VoidCorruption property from 1.0 to 0.0
        await bridgeApi.animateShaderProperty(
            this.memoryFloodShader.targetMaterialProperty,
            1.0,
            0.0,
            durationInSeconds
        );
    }
}
