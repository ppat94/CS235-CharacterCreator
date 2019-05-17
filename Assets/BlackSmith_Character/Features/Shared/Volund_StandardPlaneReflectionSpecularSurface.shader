Shader "Volund/Standard PlaneReflection (Specular, Surface)" {
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
		// TODO: pass these from code since we have no inspector for them.
		_PlaneReflectionBumpScale("Plane Reflection Bump Scale", Range(0.0, 1.0)) = 0.4
		_PlaneReflectionBumpClamp("Plane Reflection Bump Clamp", Range(0.0, 0.15)) = 0.05
	}
	
	SubShader {
		Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }
		LOD 300
		
		Blend [_SrcBlend] [_DstBlend]
		ZWrite [_ZWrite]

		CGPROGRAM
		// - Physically based Standard lighting model, specular workflow
		// - Tweak StandardSpecular light model to inject custom reflection data
		// - 'fullforwardshadows' to enable shadows on all light types
		// - 'addshadow' to ensure alpha test works for depth/shadow passes
		// - 'keepalpha' to allow alpha blended output options
		// - 'interpolateview' because that's what the non-surface Standard does
		// - Custom vertex function to setup detail UVs as expected by Standard shader
		// - Custom finalcolor function to output controlled final alpha
		// - 'exclude_path:deferred' since we only support forward path
		// - 'exclude_path:prepass' since we have no use for this legacy path
		#pragma surface PRStandardSurfaceSpecular PRStandardSpecular fullforwardshadows addshadow keepalpha interpolateview vertex:StandardSurfaceVertex finalcolor:StandardSurfaceSpecularFinal exclude_path:deferred exclude_path:prepass

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
		
		// Standard, but unused in this project
		//#pragma shader_feature _EMISSION
		//#pragma shader_feature _PARALLAXMAP
		
		// Volund additional variants
		#pragma multi_compile _ PLANE_REFLECTION
						
		// Volund uniforms
		uniform half		_PlaneReflectionBumpScale;
		uniform half		_PlaneReflectionBumpClamp;
		uniform half		_PlaneReflectionLodSteps;
		uniform sampler2D	_PlaneReflection;
					
		// We need screen pos for plane reflections
#if defined(PLANE_REFLECTION)
		struct SurfInput {
			float4	texcoord;
	#ifdef _PARALLAXMAP
			half3	viewDir;
	#endif
			float4	screenPos;
			
			INTERNAL_DATA
		};
		#define Input SurfInput
#endif

		// Also need to pass screen pos on as UVs
#if defined(PLANE_REFLECTION)
		struct SurfOutput {
			half3	Albedo;		// diffuse color
			half3	Specular;	// specular color
			half3	Normal;		// tangent space normal, if written
			half3	Emission;
			half	Smoothness;	// 0=rough, 1=smooth
			half	Occlusion;	// occlusion (default 1)
			half	Alpha;		// alpha for transparencies
			float4	screenUVsAndNormal;
		};
		#define SurfaceOutputStandardSpecular SurfOutput
#endif			

		// Include all the Standard shader surface helpers
		#include "UnityStandardSurface.cginc"
		
		// Setup reflection UVs
		void PRStandardSurfaceSpecular(Input IN, inout SurfaceOutputStandardSpecular o)
		{
			StandardSurfaceSpecular(IN, o);

#if defined(PLANE_REFLECTION)
			o.screenUVsAndNormal.xy = IN.screenPos.xy / IN.screenPos.w;
			o.screenUVsAndNormal.zw = WorldNormalVector(IN, o.Normal).xz;
#endif
		}
		
		// Inject our own reflections if enabled. In theory, this should have been in the _GI function,
		// but the shader compiler doens't analyze it, thus optimizing out stuff we actually need.
		inline half4 LightingPRStandardSpecular(SurfaceOutputStandardSpecular s, half3 viewDir, UnityGI gi) {
#if defined(PLANE_REFLECTION)
			float mip = pow(1.f - s.Smoothness, 3.f/4.f) * _PlaneReflectionLodSteps; 
			float4 lookup = float4(s.screenUVsAndNormal.x, s.screenUVsAndNormal.y, 0.f, mip);
			lookup.xy += clamp(-s.screenUVsAndNormal.zw * _PlaneReflectionBumpScale, -_PlaneReflectionBumpClamp, _PlaneReflectionBumpClamp);
			gi.indirect.specular = tex2Dlod(_PlaneReflection, lookup).rgb;
#endif 
			return LightingStandardSpecular(s, viewDir, gi);
		}
		
		// Don't bother sampling reflection probes if we're replacing it with plane
		// reflections anyway. (i.e. let's just make the optimizer's life a little easier)
		inline void LightingPRStandardSpecular_GI(SurfaceOutputStandardSpecular s, UnityGIInput data, inout UnityGI gi) {
#if !defined(PLANE_REFLECTION)
			gi = UnityGlobalIllumination(data, s.Occlusion, s.Smoothness, s.Normal, true);
#else
			gi = UnityGlobalIllumination(data, s.Occlusion, s.Smoothness, s.Normal, false);
#endif
		}

		ENDCG
	}
	
	CustomEditor "VolundMultiStandardShaderGUI"
	FallBack "Diffuse"
}