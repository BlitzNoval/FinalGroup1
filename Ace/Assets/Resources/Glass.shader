Shader "Custom/Glass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0,1)) = 0.95
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpStrength ("Normal Strength", Range(0,1)) = 0.1
        _Transparency ("Transparency", Range(0,1)) = 0.8
        _RefractionStrength ("Refraction Strength", Range(0,1)) = 0.1
        _ChromaticAberration ("Chromatic Aberration", Range(0,1)) = 0.1
        _FresnelPower ("Fresnel Power", Range(0,10)) = 5
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        
        // First pass - renders the refraction
        GrabPass { "_GrabTexture" }
        
        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0
        
        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _GrabTexture;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
            float4 screenPos;
        };
        
        half _Smoothness;
        half _Metallic;
        half _BumpStrength;
        half _Transparency;
        half _RefractionStrength;
        half _ChromaticAberration;
        half _FresnelPower;
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the normal map
            float3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            normal.xy *= _BumpStrength;
            
            // Calculate fresnel
            float fresnel = pow(1.0 - saturate(dot(normal, IN.viewDir)), _FresnelPower);
            
            // Sample the base texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            
            // Calculate refraction UV offset
            float2 offset = normal.xy * _RefractionStrength;
            
            // Apply chromatic aberration
            float4 refractionR = tex2D(_GrabTexture, IN.screenPos.xy/IN.screenPos.w + offset * (1.0 + _ChromaticAberration));
            float4 refractionG = tex2D(_GrabTexture, IN.screenPos.xy/IN.screenPos.w + offset);
            float4 refractionB = tex2D(_GrabTexture, IN.screenPos.xy/IN.screenPos.w + offset * (1.0 - _ChromaticAberration));
            
            // Combine the colors
            o.Albedo = float3(refractionR.r, refractionG.g, refractionB.b);
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Alpha = _Transparency + fresnel * (1 - _Transparency);
            o.Normal = normal;
        }
        ENDCG
    }
    FallBack "Transparent/VertexLit"
}