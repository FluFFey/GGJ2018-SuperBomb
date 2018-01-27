Shader "Unlit/PathShader"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		_StripeColor ("StripeColor", Color) = (0,0,0,0)
		_GridColor ("GridColor", Color) = (0,0,0,0)
		[Toggle]_IsCornerPiece("Corner", Float) = 0
		[Toggle]_CornerLU("CornerLU", Float) = 0
		[Toggle]_CornerLD("CornerLD", Float) = 0
		[Toggle]_CornerRU("CornerRU", Float) = 0
		[Toggle]_CornerRD("CornerRD", Float) = 0
		[Toggle]_VerticalLine("Vertical", Float) = 0
		[Toggle]_IsCrossSection("Cross", Float) = 0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Blend One One
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
			fixed4 _GridColor;
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
				float zGridMod = i.posWorld.z%3;
				float xGridMod = i.posWorld.x%3;

				if (zGridMod < 0.03f|| xGridMod < 0.03f)
				{
					return _GridColor*(_CosTime.x+1)/1.8f+0.2f;
				}

				//if (zMod > 2.125f && zMod < 2.375f || xMod > 2.125f && xMod < 2.375f)
				//{
				//	return _StripeColor;
				//}



				float zMod = i.posWorld.z%5;
				float xMod = i.posWorld.x%5;
				// sample the texture
				_StripeColor*= (cos(_Time*3.14f)+1)/2.0f + _StripeColor*0.2f;
				if (_CornerRD ==1.0f)
				{
				if (((zMod > 4.7f && zMod < 4.9f && 
						xMod < 2.5f && xMod > 0.1f )
						||
						(zMod > 2.5f && zMod < 4.9f &&
						 xMod > 0.1f && xMod < 0.3f))
						||
						(zMod > 0.1f && zMod < 0.3f &&
						(xMod < 2.5f ||xMod > 4.9f))
						||
						(zMod > 2.5f || zMod < 0.3f) && 
						 (xMod > 4.7f && xMod < 4.9f))
						{ 
							return _StripeColor;
						}
				}

				if (_CornerRU ==1.0f)
				{
					if (((zMod > 4.7f && zMod < 4.9f && 
						(xMod < 2.5f ||xMod > 4.9f))
						||
						(zMod > 0.1f && zMod < 2.5f &&
						 xMod > 0.1f && xMod < 0.3f))
						||
						(zMod > 0.1f && zMod < 0.3f &&
						(xMod < 2.5f && xMod > 0.1f))
						||
						(zMod > 4.7f || zMod < 2.5f) && 
						 (xMod > 4.7f && xMod < 4.9f))
						{ 
							return _StripeColor;
						}
				}

				if (_CornerLD ==1.0f)
				{
					if (((zMod > 4.7f && zMod < 4.9f && 
						(xMod > 2.5f && xMod < 4.9f)))
						||
						((zMod < 0.2f || zMod > 2.5f) &&
						 (xMod > 0.1f && xMod < 0.3f))
						||
						(zMod > 0.1f && zMod < 0.3f &&
						(xMod > 2.5f || xMod < 0.3f))
						||
						(zMod > 2.5f && zMod < 4.9f) && 
						 (xMod > 4.7f && xMod < 4.9f))
						{ 
							return _StripeColor;
						}
				}

				if (_CornerLU ==1.0f)
				{
					if (((zMod > 4.7f && zMod < 4.9f && 
						(xMod > 2.5f || xMod < 0.3f)))
						||
						((zMod > 4.7f || zMod < 2.5f) &&
						 (xMod > 0.1f && xMod < 0.3f))
						||
						(zMod > 0.1f && zMod < 0.3f &&
						(xMod > 2.5f && xMod < 4.9f))
						||
						(zMod < 2.5f && zMod > 0.3f) && 
						 (xMod > 4.7f && xMod < 4.9f))
						{ 
							return _StripeColor;
						}
				}

				if (_IsCrossSection ==1.0f)
				{
					if ((zMod > 0.1f && zMod < 0.3f ||
						 zMod > 4.7f && zMod < 4.9f) ||
						(xMod > 0.1f && xMod < 0.3f ||
						 xMod > 4.7f && xMod < 4.9f))
					{ 
						return _StripeColor;
					}
				}
				if (_HorizontalLine ==1.0f)
				{
					if (zMod > 0.1f && zMod < 0.3f ||
					zMod > 4.7f && zMod < 4.9f)
					{ 
						return _StripeColor;
					}
				}
				else if (_VerticalLine == 1.0f)
				{
					if(xMod > 0.1f && xMod < 0.3f ||
					xMod > 4.7f && xMod < 4.9f)
					{ 
						return _StripeColor;
					}
				}
				return fixed4(0,0,0,0);
			}
			ENDCG
		}
	}
}
