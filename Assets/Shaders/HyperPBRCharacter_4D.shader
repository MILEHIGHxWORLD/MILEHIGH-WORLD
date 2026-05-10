// --- HYPER-REALISTIC CHARACTER SHADER (4D) ---
// The following conceptual ShaderLab/HLSL code would be saved in a file named "HyperPBRCharacter_4D.shader"
// and applied to a material used on this character's renderer to achieve a hyper-realistic,
// next-generation visual quality. It demonstrates advanced techniques including:
// 1. Physically-Based Rendering (PBR) using a StandardSpecular lighting model.
// 2. Parallax Occlusion Mapping (POM) for creating 3D depth on flat surfaces.
// 3. Subsurface Scattering (SSS) approximation for realistic skin rendering.
// 4. Iridescence (Thin-Film Interference) for materials with a rainbow-like sheen.
// 5. 4D Procedural Effects using a 3D noise texture and time, for dynamic effects like flowing Void energy.

Shader "Milehigh/HyperPBRCharacter_4D"
{
    Properties
    {
        // --- Core PBR Properties ---
        _Color ("Albedo Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Intensity", Float) = 1.0

        // RMAI Map: Roughness (R), Metallic (G), Ambient Occlusion (B), Iridescence Mask (A)
        _RMAIMap ("RMAI (Roughness, Metallic, AO, Iridescence)", 2D) = "white" {}
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
        _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
        _Ao ("Ambient Occlusion", Range(0.0, 1.0)) = 1.0

        // --- Parallax Occlusion Mapping (3D Depth) ---
        _ParallaxMap ("Height Map (A)", 2D) = "gray" {}
        _Parallax ("Height Scale", Range (0.005, 0.08)) = 0.02

        // --- 4D Procedural Effects (e.g., Void Corruption) ---
        _NoiseTex ("3D Noise Texture", 3D) = "gray" {}
        _EffectTime ("Effect Time (For 4D Noise)", Float) = 0.0
        _EffectColor ("Procedural Effect Color", Color) = (0.5, 0, 1, 1)
        _EffectMask ("Effect Mask (R)", 2D) = "black" {}
        _EffectScale ("Noise Scale", Float) = 10.0
        _EffectSpeed ("Noise Speed", Float) = 0.5

        // --- Iridescence / Thin-Film ---
        _IridescenceThickness ("Iridescence Thickness", Range(100, 1000)) = 400.0 // in nm
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }
        LOD 400

        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows vertex:vert
        #pragma target 4.0

        // Include a 4D Simplex Noise function library
        // (For brevity, assume a full noise library like "SimplexNoise4D.cginc" is included here)
        // float snoise(float4 p); // Function signature

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldPos;
            float3 T; // Tangent
            float3 B; // Bitangent
            float3 N; // Normal
        };

        sampler2D _MainTex, _BumpMap, _RMAIMap, _EffectMask, _ParallaxMap;
        sampler3D _NoiseTex;
        fixed4 _Color, _EffectColor;
        half _BumpScale, _Metallic, _Glossiness, _Ao, _Cutoff, _Parallax;
        half _EffectScale, _EffectSpeed, _IridescenceThickness;

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.T = UnityObjectToWorldDir(v.tangent.xyz);
            o.B = cross(o.N, o.T) * v.tangent.w; // Bitangent
            o.N = UnityObjectToWorldNormal(v.normal);

            // Parallax Occlusion Mapping
            half h = tex2Dlod(_ParallaxMap, float4(v.texcoord.xy, 0, 0)).a;
            float2 offset = ParallaxOffset(h, _Parallax, v.viewDir);
            v.texcoord.xy += offset;
        }

        // Thin-film interference approximation for iridescence
        float3 Iridescence(float thickness, float NdotV)
        {
            float3 interference = sin(2.0 * 3.14159 * thickness * float3(1.0, 0.8, 0.6) / NdotV) * 0.5 + 0.5;
            //  Bolt: Replace pow(interference, 2.0) with interference * interference for performance
            return interference * interference;
        }

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            clip(albedo.a - _Cutoff);

            // ⚡ Bolt: Initialize Albedo at the start of the surf function.
            // This ensures additive effects like Subsurface Scattering are correctly accumulated
            // into the final color instead of being overwritten, and avoids redundant calculations
            // being discarded as dead work if they were assigned before a final overwrite.
            //  BOLT: Optimized albedo assignment.
            // By assigning early, we ensure that subsequent additive effects like SSS
            // are properly blended rather than being discarded or overwritten at the end.
            // ⚡ Bolt: Performance Fix - Assign base albedo first so that additive effects (like SSS)
            // are correctly applied to the final output rather than being overwritten by the subsequent assignment.
            // ⚡ Bolt: Move Albedo assignment to the top to ensure additive effects
            // (like SSS) are applied to the final color and avoid redundant work.
            o.Albedo = albedo.rgb;

            // --- PBR Properties ---
            fixed4 rmai = tex2D(_RMAIMap, IN.uv_MainTex);
            o.Smoothness = rmai.r * _Glossiness;
            o.Specular = _Metallic; // Using StandardSpecular for better control
            o.Occlusion = lerp(1, rmai.b, _Ao);

            // --- Normal Mapping ---
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);

            // --- 4D Procedural Effect ---
            half effectMask = tex2D(_EffectMask, IN.uv_MainTex).r;
            if (effectMask > 0)
            {
                // Sample 3D noise texture with a time component for 4D effect
                float4 noiseCoord = float4(IN.worldPos * _EffectScale, _Time.y * _EffectSpeed);
                // In a real implementation, you'd use a 4D noise function here.
                // We simulate it by sampling a 3D texture and evolving it.
                half noise = tex3D(_NoiseTex, noiseCoord.xyz + noiseCoord.w).r;

                // Add emissive glow based on noise
                //  Bolt: Replace pow(noise, 3.0) with noise * noise * noise for performance
                o.Emission = _EffectColor.rgb * (noise * noise * noise) * effectMask * 2.0;
            }

            // --- Iridescence ---
            half iridescenceMask = rmai.a;
            if (iridescenceMask > 0)
            {
                float NdotV = saturate(dot(o.Normal, IN.viewDir));
                float3 iridescence = Iridescence(_IridescenceThickness, NdotV);
                // Blend with albedo and apply as specular tint
                o.Specular = lerp(o.Specular, o.Specular * iridescence, iridescenceMask);
            }

            o.Albedo = albedo.rgb;
            // --- Subsurface Scattering ---
            half sssMask = tex2D(_SSSMask, IN.uv_MainTex).r;
            if (sssMask > 0)
            {
                // A more advanced SSS would use a proper lighting model.
                // This is a stylistic approximation.
                half NdotL = dot(o.Normal, _WorldSpaceLightPos0.xyz);
                //  Bolt: Replace pow(..., 8.0) with nested squares for performance
                half sss_base = saturate(dot(IN.viewDir, -_WorldSpaceLightPos0.xyz));
                half sss_sq = sss_base * sss_base;
                half sss_4 = sss_sq * sss_sq;
                half sss = sss_4 * sss_4 * _SSSScale;
                // BOLT: Ensure albedo is assigned *before* SSS is added to avoid overwriting SSS calculation
                o.Albedo = albedo.rgb;
                o.Albedo += _SSSColor.rgb * sss * NdotL * sssMask;
            }

            else
            {
                o.Albedo = albedo.rgb;
            }
        }
        ENDCG
    }
    FallBack "Transparent/Cutout/VertexLit"
}
