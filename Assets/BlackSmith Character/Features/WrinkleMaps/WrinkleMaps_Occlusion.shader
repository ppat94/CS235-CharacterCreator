// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Volund/Standard Wrinkle Maps Occlusion" {
SubShader {
	Tags { "Special"="Wrinkles" }
 	Pass {

CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#pragma only_renderers d3d11 d3d9 opengl

#include "UnityCG.cginc"
#include "UnityPBSLighting.cginc"
#include "UnityStandardUtils.cginc"

struct a2v {
	half4 vertex	: POSITION;
	half2 uv0		: TEXCOORD0;
	half2 uv1		: TEXCOORD1;
};

struct v2f {
	half4 pos		: SV_POSITION;
	half2 tex		: TEXCOORD0;
};

half4	_MainTex_ST;
half4	_DetailAlbedoMap_ST;
half	_UVSec;

v2f vert(a2v v)
{
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.tex.xy = TRANSFORM_TEX(v.uv0, _MainTex);

	return o;
}

sampler2D	_WrinkleMask;

half4		_WrinkleInfluences0;
half4		_WrinkleInfluences1;
half4		_WrinkleInfluences2;
half4		_WrinkleInfluences3;

sampler2D	_BumpMap;
half		_BumpScale;
sampler2D	_WrinkleNormalMap0;
sampler2D	_WrinkleNormalMap1;
sampler2D	_WrinkleNormalMap2;
sampler2D	_WrinkleNormalMap3;
half4		_WrinkleMapBumpScales;

sampler2D	_OcclusionMap;
half		_OcclusionStrength;
sampler2D	_WrinkleOcclusionMap0;
sampler2D	_WrinkleOcclusionMap1;
sampler2D	_WrinkleOcclusionMap2;
sampler2D	_WrinkleOcclusionMap3;
half4		_WrinkleOcclusionStrengths;

half4 LerpOneTo4(half4 b, half4 t)
{
	half4 oneMinusT = half4(1.f, 1.f, 1.f, 1.f) - t;
	return oneMinusT + b * t;
}

half Occlusion(const v2f i, const half4 influences) {
	half4 detailOcclusion;
	detailOcclusion.x = tex2D(_WrinkleOcclusionMap0, i.tex.xy).g;
	detailOcclusion.y = tex2D(_WrinkleOcclusionMap1, i.tex.xy).g;
	detailOcclusion.z = tex2D(_WrinkleOcclusionMap2, i.tex.xy).g;
	detailOcclusion.w = tex2D(_WrinkleOcclusionMap3, i.tex.xy).g;
	detailOcclusion = LerpOneTo4(detailOcclusion, _WrinkleOcclusionStrengths * influences);
	
	half combinedDetailOcclusion = min(detailOcclusion.x, min(detailOcclusion.y, min(detailOcclusion.z, detailOcclusion.w)));
	
	half baseOcclusion = LerpOneTo(tex2D(_OcclusionMap, i.tex.xy).g, _OcclusionStrength);
	half blendedOcclusion = min(baseOcclusion, combinedDetailOcclusion);
	
	return blendedOcclusion;
}

half3 Normal(const v2f i, const half4 influences) {
	half3 wrinkleNormalTan0 = UnpackScaleNormal(tex2D(_WrinkleNormalMap0, i.tex.xy), _WrinkleMapBumpScales.x);
	half3 wrinkleNormalTan1 = UnpackScaleNormal(tex2D(_WrinkleNormalMap1, i.tex.xy), _WrinkleMapBumpScales.y);
	half3 wrinkleNormalTan2 = UnpackScaleNormal(tex2D(_WrinkleNormalMap2, i.tex.xy), _WrinkleMapBumpScales.z);
	half3 wrinkleNormalTan3 = UnpackScaleNormal(tex2D(_WrinkleNormalMap3, i.tex.xy), _WrinkleMapBumpScales.w);
	
	half3 detailNormalTangent = normalize(
		wrinkleNormalTan0 * influences.x +
		wrinkleNormalTan1 * influences.y +
		wrinkleNormalTan2 * influences.z +
		wrinkleNormalTan3 * influences.w +
		half3(0.f, 0.f, 1e-5f)
	);

	half blendFactor = min(1.f, dot(influences, 1.f));

	half3 normalTangent = UnpackScaleNormal(tex2D (_BumpMap, i.tex.xy), _BumpScale);
	normalTangent = lerp(normalTangent, detailNormalTangent, blendFactor);
	//normalTangent = lerp(normalTangent, BlendNormals(normalTangent, detailNormalTangent), blendFactor);
	
	return normalize(normalTangent);
}

half4 frag(v2f i) : COLOR {
	half4 mask = tex2D(_WrinkleMask, i.tex.xy);

	half4 influences;
	influences.x = dot(mask, _WrinkleInfluences0);
	influences.y = dot(mask, _WrinkleInfluences1);
	influences.z = dot(mask, _WrinkleInfluences2);
	influences.w = dot(mask, _WrinkleInfluences3);

	half occlusion = Occlusion(i, influences);
	half3 normal = Normal(i, influences);

	return half4(occlusion, normal.x * 0.5f + 0.5f, normal.y * 0.5f + 0.5f, 0); // RGB10A2 target
}
ENDCG

}}//Pass/SubShader

	FallBack Off
}//Shader

