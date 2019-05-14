Shader "Volund/Standard (Specular, Surface)" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		
		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_SpecColor("Specular", Color) = (0.2,0.2,0.2)
		_SpecGlossMap("Specular", 2D) = "white" {}

		_BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}
		
		_Parallax ("Height Scale", Range (0.005, 0.08)) = 0.02
		_ParallaxMap ("Height Map", 2D) = "black" {}
		
		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}
		
		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}
		
		_DetailMask("Detail Mask", 2D) = "white" {}
		
		_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
		_DetailNormalMapScale("Scale", Float) = 1.0
		_DetailNormalMap("Normal Map", 2D) = "bump" {}

		[Enum(UV0,0,UV1,1)] _UVSec ("UV Set for secondary textures", Float) = 0

		// Blending state
		[HideInInspector] _Mode ("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("__src", Float) = 1.0
		[HideInInspector] _DstBlend ("__dst", Float) = 0.0
		[HideInInspector] _ZWrite ("__zw", Float) = 1.0
		
		// Volund properties
		[HideInInspector] _Orthonormalize ("__orthonormalize", Float) = 0.0
		[HideInInspector] _SmoothnessInAlbedo ("__smoothnessinalbedo", Float) = 0.0
		//_PlaneReflectionBumpScale("Plane Reflection Bump Scale", Range(0.0, 1.0)) = 0.4
		//_PlaneReflectionBumpClamp("Plane Reflection Bump Clamp", Range(0.0, 0.15)) = 0.05
	}
	
	SubShader {
		Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }
		LOD 300
		
		Blend [_SrcBlend] [_DstBlend]
		ZWrite [_ZWrite]

		CGPROGRAM
		// - Physically based Standard lighting model, specular workflow
		// - 'fullforwardshadows' to enable shadows on all light types
		// - 'addshadow' to ensure alpha test works for depth/shadow passes
		// - 'keepalpha' to allow alpha blended output options
		// - 'interpolateview' because that's what the non-surface Standard does
		// - Custom vertex function to setup detail UVs as expected by Standard shader
		// - Custom finalcolor function to output controlled final alpha
		// - 'exclude_path:prepass' since we have no use for this legacy path
		#pragma surface SurfSpecular StandardSpecular fullforwardshadows addshadow keepalpha interpolateview vertex:StandardSurfaceVertex finalcolor:StandardSurfaceSpecularFinal exclude_path:prepass

		// Use shader model 3.0 target, to get nicer looking lighting (PBS toggles internally on shader model)
		#pragma target 3.0

		// This shader probably works fine for console/mobile platforms as well, but 
		// these are the ones we've actually tested.
		#pragma only_renderers d3d11 d3d9 opengl
		
		// Standard shader feature variants
		#pragma shader_feature _NORMALMAP
		#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
		#pragma shader_feature _SPECGLOSSMAP
		#pragma shader_feature _DETAIL_MULX2
		#pragma shader_feature _EMISSION
		
		// Standard, but unused in this project
		//#pragma shader_feature _PARALLAXMAP
		
		// Volund additional variants
		#pragma shader_feature ORTHONORMALIZE_TANGENT_BASE
		#pragma shader_feature SMOOTHNESS_IN_ALBEDO
			
		// Include all the Standard shader surface helpers
		#include "UnityStandardSurface.cginc"

		// Our main surface entry point
		void SurfSpecular(Input IN, inout SurfaceOutputStandardSpecular o)
		{
			StandardSurfaceSpecular(IN, o);

			// Optionally sample smoothness from albedo texture alpha channel instead of sg texture
#ifdef SMOOTHNESS_IN_ALBEDO
			o.Smoothness = tex2D(_MainTex, IN.texcoord.xy).a;
#endif
		}

		ENDCG
	}
	
	CustomEditor "VolundMultiStandardShaderGUI"
	FallBack "Diffuse"
}