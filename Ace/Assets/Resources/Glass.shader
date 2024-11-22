Shader "Custom/AdvancedGlass" {
    Properties {
        // Base Properties
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        
        // Surface Properties
        _Smoothness ("Smoothness", Range(0,1)) = 0.98
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        // Normal Mapping
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpStrength ("Normal Strength", Range(0,2)) = 0.5
        _DetailNormalMap ("Detail Normal Map", 2D) = "bump" {}
        _DetailNormalMapScale ("Detail Normal Scale", Range(0,2)) = 1.0
        
        // Transparency and Refraction
        _IndexOfRefraction ("Index of Refraction", Range(1.0, 2.5)) = 1.52
        _ChromaticDispersion ("Chromatic Dispersion", Range(0,0.2)) = 0.02
        _Transparency ("Transparency", Range(0,1)) = 0.9
        
        // Thickness Simulation
        _ThicknessMap ("Thickness Map", 2D) = "white" {}
        _ThicknessScale ("Thickness Scale", Range(0,10)) = 1.0
        _AbsorptionColor ("Absorption Color", Color) = (0.3,0.7,1.0,1)
        _AbsorptionStrength ("Absorption Strength", Range(0,10)) = 2.0
        
        // Surface Detail
        _Scratches ("Scratch Texture", 2D) = "bump" {}
        _ScratchStrength ("Scratch Strength", Range(0,1)) = 0.1
        _Dustiness ("Dustiness", Range(0,1)) = 0.0
        _DustColor ("Dust Color", Color) = (0.5,0.5,0.5,1)
        
        // Fresnel Effects
        _FresnelPower ("Fresnel Power", Range(0,10)) = 5
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        
        // Environment Interaction
        _CubeMap ("Cubemap", CUBE) = "" {}
        _ReflectionStrength ("Reflection Strength", Range(0,1)) = 0.5
        _EnvironmentBlur ("Environment Blur", Range(0,8)) = 1.0
        
        // Edge Effects
        _EdgeFalloff ("Edge Falloff", Range(0,5)) = 1.0
        _EdgeTint ("Edge Tint", Color) = (1,1,1,1)
    }
    
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
        LOD 300
        
        // Shadow casting pass
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"
            
            struct v2f { 
                V2F_SHADOW_CASTER;
            };
            
            v2f vert(appdata_base v) {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }
            
            float4 frag(v2f i) : SV_Target {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        
        // First pass - Back face refraction pre-pass
        GrabPass { "_BackgroundTexture" }
        
        Pass {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma target 4.0
            
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "AutoLight.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 bitangentDir : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                UNITY_FOG_COORDS(7)
                UNITY_SHADOW_COORDS(8)
            };
            
            // Property variables
            sampler2D _MainTex, _BumpMap, _DetailNormalMap, _ThicknessMap, _Scratches;
            sampler2D _BackgroundTexture;
            samplerCUBE _CubeMap;
            
            float4 _MainTex_ST, _BumpMap_ST, _DetailNormalMap_ST, _ThicknessMap_ST, _Scratches_ST;
            float4 _Color, _AbsorptionColor, _DustColor, _FresnelColor, _EdgeTint;
            
            float _Smoothness, _Metallic, _BumpStrength, _DetailNormalMapScale;
            float _IndexOfRefraction, _ChromaticDispersion, _Transparency;
            float _ThicknessScale, _AbsorptionStrength, _ScratchStrength, _Dustiness;
            float _FresnelPower, _ReflectionStrength, _EnvironmentBlur, _EdgeFalloff;
            
            v2f vert (appdata v) {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv2, _DetailNormalMap);
                
                o.screenPos = ComputeScreenPos(o.pos);
                COMPUTE_EYEDEPTH(o.screenPos.z);
                
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                
                o.projPos = ComputeGrabScreenPos(o.pos);
                
                UNITY_TRANSFER_SHADOW(o, v.uv);
                UNITY_TRANSFER_FOG(o, o.pos);
                
                return o;
            }
            
            float3 CalculateNormal(v2f i) {
                float3 normalMap = UnpackNormal(tex2D(_BumpMap, i.uv.xy));
                float3 detailNormal = UnpackNormal(tex2D(_DetailNormalMap, i.uv.zw));
                
                normalMap = lerp(float3(0,0,1), normalMap, _BumpStrength);
                detailNormal = lerp(float3(0,0,1), detailNormal, _DetailNormalMapScale);
                
                float3 surfaceNormal = normalize(normalMap + detailNormal);
                
                // Add surface scratches
                float3 scratchNormal = UnpackNormal(tex2D(_Scratches, i.uv.xy));
                surfaceNormal = lerp(surfaceNormal, scratchNormal, _ScratchStrength);
                
                // Transform to world space
                float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
                return normalize(mul(surfaceNormal, tangentTransform));
            }
            
            float3 CalculateRefraction(float2 screenUV, float3 normal, float dispersionOffset) {
                float2 offset = normal.xy * (_IndexOfRefraction - 1.0 + dispersionOffset);
                return tex2D(_BackgroundTexture, screenUV + offset).rgb;
            }
            
            float4 frag (v2f i) : SV_Target {
                // Calculate view direction and normal
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 normalDir = CalculateNormal(i);
                
                // Calculate screen UV for refraction
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                
                // Calculate chromatic aberration
                float3 refraction;
                refraction.r = CalculateRefraction(screenUV, normalDir, _ChromaticDispersion).r;
                refraction.g = CalculateRefraction(screenUV, normalDir, 0.0).g;
                refraction.b = CalculateRefraction(screenUV, normalDir, -_ChromaticDispersion).b;
                
                // Calculate fresnel effect
                float fresnel = pow(1.0 - saturate(dot(normalDir, viewDir)), _FresnelPower);
                float3 fresnelColor = lerp(_Color.rgb, _FresnelColor.rgb, fresnel);
                
                // Sample thickness map and calculate absorption
                float thickness = tex2D(_ThicknessMap, i.uv.xy).r * _ThicknessScale;
                float3 absorption = exp(-_AbsorptionColor.rgb * thickness * _AbsorptionStrength);
                
                // Calculate reflections
                float3 reflectDir = reflect(-viewDir, normalDir);
                float4 envSample = texCUBElod(_CubeMap, float4(reflectDir, _EnvironmentBlur));
                float3 reflection = envSample.rgb * _ReflectionStrength;
                
                // Calculate dust effect
                float3 dustColor = lerp(1.0, _DustColor.rgb, _Dustiness);
                
                // Calculate edge falloff
                float edgeFactor = 1.0 - pow(abs(dot(normalDir, viewDir)), _EdgeFalloff);
                float3 edgeColor = lerp(1.0, _EdgeTint.rgb, edgeFactor);
                
                // Combine all effects
                float3 finalColor = lerp(refraction, reflection, fresnel);
                finalColor *= absorption * dustColor * edgeColor * fresnelColor;
                
                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                // Calculate final alpha
                float alpha = _Transparency * (1.0 - fresnel) + fresnel;
                
                return float4(finalColor, alpha);
            }
            ENDCG
        }
    }
    
    CustomEditor "AdvancedGlassShaderGUI"
    FallBack "Transparent/VertexLit"
}