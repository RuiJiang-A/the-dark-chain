#ifndef COMMON_INCLUDE
#define COMMON_INCLUDE

#include "Unity.hlsl"

float3 WorldSpacePosition(float3 positionOS)
{
	return mul(UNITY_MODEL_MATRIX, float4(positionOS, 1.0)).xyz;
}

float4 ClipSpacePosition(float3 positionWS)
{
	return mul(UNITY_VIEW_PROJECTION_MATRIX, float4(positionWS, 1.0));
}

#endif
