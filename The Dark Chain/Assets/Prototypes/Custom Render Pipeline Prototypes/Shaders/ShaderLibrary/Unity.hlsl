#ifndef UNITY_INCLUDE
#define UNITY_INCLUDE

// Default Unity View Projection Matrix
float4x4 unity_MatrixVP;
// UnityPerDraw is bultin name for this cbuffer
CBUFFER_START(UnityPerDraw)
	float4x4 unity_ObjectToWorld;
	float4x4 unity_WorldToObject;
	float4 unity_LODFade;
	real4 unity_WorldTransformParams;
CBUFFER_END

float4x4 unity_MatrixV;
float4x4 unity_MatrixInvV;
float4x4 unity_prev_MatrixM;
float4x4 unity_prev_MatrixIM;
float4x4 glstate_matrix_projection;

#define UNITY_MODEL_MATRIX unity_ObjectToWorld
#define UNITY_VIEW_PROJECTION_MATRIX unity_MatrixVP
#define UNITY_MODEL_MATRIX_INVERSE unity_WorldToObject
#define UNITY_VIEW_MATRIX unity_MatrixV
#define UNITY_VIEW_MATRIX_INVERSE unity_MatrixInvV
#define UNITY_PREVIOUS_MODEL_MATRIX unity_prev_MatrixM
#define UNITY_PREVIOUS_MODEL_MATRIX_INVERSE unity_prev_MatrixIM
#define UNITY_PROJECTION_MATRIX glstate_matrix_projection

#define UNITY_MATRIX_M unity_ObjectToWorld
#define UNITY_MATRIX_I_M unity_WorldToObject
#define UNITY_MATRIX_V unity_MatrixV
#define UNITY_MATRIX_I_V unity_MatrixInvV
#define UNITY_MATRIX_VP unity_MatrixVP
#define UNITY_PREV_MATRIX_M unity_prev_MatrixM
#define UNITY_PREV_MATRIX_I_M unity_prev_MatrixIM
#define UNITY_MATRIX_P glstate_matrix_projection

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

#endif