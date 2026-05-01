// @Cyanilux | https://github.com/Cyanilux/URP_ShaderGraphCustomLighting

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN

/*
- Samples the Shadowmap for the Main Light, based on the World Position passed in. (Position node)
*/
void MainLightShadows_float(float3 WorldPos, half4 Shadowmask, out float ShadowAtten){
	#ifdef SHADERGRAPH_PREVIEW
		ShadowAtten = 1;
	#else
		#if defined(_MAIN_LIGHT_SHADOWS_SCREEN) && !defined(_SURFACE_TYPE_TRANSPARENT)
		float4 shadowCoord = ComputeScreenPos(TransformWorldToHClip(WorldPos));
		#else
		float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
		#endif
		ShadowAtten = MainLightShadow(shadowCoord, WorldPos, Shadowmask, _MainLightOcclusionProbes);
	#endif
}

void MainLightShadows_float(float3 WorldPos, out float ShadowAtten){
	MainLightShadows_float(WorldPos, half4(1,1,1,1), ShadowAtten);
}