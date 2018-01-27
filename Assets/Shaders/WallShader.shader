﻿Shader "Unlit/WallShader"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		_StripeColor ("Color", Color) = (0,0,0,0)
		//[Toggle]_IsCornerPiece("Corner", Float) = 0
		//[Toggle]_CornerLU("CornerLU", Float) = 0
		//[Toggle]_CornerLD("CornerLD", Float) = 0
		//[Toggle]_CornerRU("CornerRU", Float) = 0
		//[Toggle]_CornerRD("CornerRD", Float) = 0
		//[Toggle]_VerticalLine("Vertical", Float) = 0
		//[Toggle]_IsCrossSection("Cross", Float) = 0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				//float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				//float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 posWorld : TEXCOORD0;
				
				float4 vertex : SV_POSITION;
			};

			//sampler2D _MainTex;
			fixed4 _StripeColor;
			float _CornerLU;
			float _CornerLD;
			float _CornerRU;
			float _CornerRD;
			float _HorizontalLine;
			float _VerticalLine;
			float _IsCrossSection;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.posWorld = mul(UNITY_MATRIX_M, v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			float zMod = i.posWorld.z%5;
			float xMod = i.posWorld.x%5;
				// sample the texture
				_StripeColor*= ((_SinTime.w+1)/2.0f) + fixed4(_StripeColor.x,_StripeColor.y*0.9f,_StripeColor.z*0.9f,_StripeColor.a);
				
				if (i.posWorld.y > 1.7f && i.posWorld.y < 2.0f ||
					i.posWorld.y > 2.4f && i.posWorld.y < 2.7f
				)
				{
					return _StripeColor;
				}


				// apply fog
				return fixed4(0,0,0,0);
			}
			ENDCG
		}
	}
}