#ifndef THE_DARK_CHAIN_UNLIT
#define THE_DARK_CHAIN_UNLIT

#include "../Library/Common.hlsl"

CBUFFER_START(MaterialProperties)
	float4 _BaseColor;
CBUFFER_END

float4 Vertex(float3 positionOS : POSITION) : SV_POSITION
{
	const float3 worldSpace = WorldSpacePosition(positionOS.xyz);
	const float4 clipSpace = ClipSpacePosition(worldSpace);
	return clipSpace;
}

float4 Fragment() : SV_TARGET
{
	return _BaseColor;
}

#endif
