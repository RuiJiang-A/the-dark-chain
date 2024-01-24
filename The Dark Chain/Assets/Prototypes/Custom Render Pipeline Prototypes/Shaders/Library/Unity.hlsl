#ifndef UNITY_INCLUDE
#define UNITY_INCLUDE

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

// Default Unity View Projection Matrix
float4x4 unity_MatrixVP;

CBUFFER_START(ObjectRenderingData)
	float4x4 unity_ObjectToWorld;
	float4x4 unity_WorldToObject;
	float4 unity_LODFade;
	real4 unity_WorldTransformParams;
CBUFFER_END

// float4x4 unity_WorldToObject;
float4x4 unity_MatrixV;
float4x4 unity_MatrixInvV;
float4x4 unity_prev_MatrixM;
float4x4 unity_prev_MatrixIM;
float4x4 glstate_matrix_projection;
// float4 unity_WorldTransformParams;

#define UNITY_MODEL_MATRIX unity_ObjectToWorld
#define UNITY_VIEW_PROJECTION_MATRIX unity_MatrixVP
#define UNITY_MODEL_MATRIX_INVERSE unity_WorldToObject
#define UNITY_VIEW_MATRIX unity_MatrixV
#define UNITY_VIEW_MATRIX_INVERSE unity_MatrixInvV
#define UNITY_PREVIOUS_MODEL_MATRIX unity_prev_MatrixM
#define UNITY_PREVIOUS_MODEL_MATRIX_INVERSE unity_prev_MatrixIM
#define UNITY_PROJECTION_MATRIX glstate_matrix_projection

#endif