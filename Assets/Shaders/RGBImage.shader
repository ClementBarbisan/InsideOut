// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RGBImage" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader 
	{
		
		Pass
		{
			Cull Off
			CGPROGRAM
			#pragma vertex verts
			#pragma fragment frags
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			
			sampler2D _MainTex;
			
			struct VertexPos 
			{
				float4 pos : SV_POSITION;
				float4 wpos : TEXCOORD0;
			};
			
			VertexPos verts(float4 vertex: POSITION)
			{
				VertexPos outVer;
				outVer.pos = UnityObjectToClipPos(vertex);
				outVer.wpos = outVer.pos;
				return outVer;
			}
			
			half4 frags(VertexPos inpVer) : COLOR
			{
				float2 uv;
				uv.x = (0.5 + (inpVer.wpos.x / (inpVer.wpos.w * 2)));
				uv.y = 1.0 - (0.5 - (inpVer.wpos.y / (inpVer.wpos.w * 2)));
				float3 rgbPixel = tex2D(_MainTex, uv);	
				return half4(rgbPixel, 1.0f);				
			}
			ENDCG
		}
	}
}