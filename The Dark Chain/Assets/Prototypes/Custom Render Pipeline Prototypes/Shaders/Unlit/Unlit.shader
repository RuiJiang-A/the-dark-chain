Shader "The Dark Chain/Unlit/Unlit Color" {
	
	Properties {
	    // ---- Base Color ----
		_BaseColor("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		[Toggle(_CLIPPING)] _Clipping ("Alpha Clipping", Float) = 0
		_BaseMap("Texture", 2D) = "white" {}
	
		// ---- Blending Modes ----
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Source Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Destiniation Blend", Float) = 0

		// ZWrite Mode
		[Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
	}
	
	SubShader {
		Pass {
			Blend [_SrcBlend] [_DstBlend]

			HLSLPROGRAM
			#pragma shader_feature _CLIPPING
			#pragma multi_compile_instancing
			#pragma vertex Vertex
			#pragma fragment Fragment
			#include "UnlitPass.hlsl"
			ENDHLSL
		}
	}
}