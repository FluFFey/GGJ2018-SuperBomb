Shader "Unlit/WallShader"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		_StripeColor ("Color", Color) = (0,0,0,0)
		[Toggle]_IsDoor("IsDoor", Float) = 0
		_ScaledTime("ScaledTime", float) = 1

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
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
			float _IsDoor;
			float _ScaledTime;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.posWorld = mul(UNITY_MATRIX_M, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float zMod = i.posWorld.z%5;
				float xMod = i.posWorld.x%5;
				_StripeColor*= ((cos(8*_ScaledTime+i.posWorld.x+i.posWorld.z)+1)/2.0f) + fixed4(_StripeColor.x,_StripeColor.y*0.9f,_StripeColor.z*0.9f,_StripeColor.a);
				
				if (_IsDoor)
				{
					_StripeColor*=1.15f;
					if (i.posWorld.y > 2.4f && i.posWorld.y < 2.7f && (xMod > 1.8f && xMod < 3.2f))
					{
						return _StripeColor;	
					}
					
					if (i.posWorld.y > 1.7f && i.posWorld.y < 2.0f && (xMod > 1.4f && xMod < 3.6f))
					{
						return _StripeColor;	
					}
					
					if (i.posWorld.y > 1.7f && i.posWorld.y < 4.0f && 
					xMod > 1.2 &&  xMod < 1.5)
					{
						return _StripeColor;
					}
					
					if (i.posWorld.y > 2.5f && i.posWorld.y < 4.6f && 
					xMod > 1.8 &&  xMod < 2.1)
					{
						return _StripeColor;
					}

					if (i.posWorld.y > 1.7f && i.posWorld.y < 4.0f && 
					xMod > 3.5 &&  xMod < 3.8)
					{
						return _StripeColor;
					}
					if (i.posWorld.y > 2.5f && i.posWorld.y < 4.6f && 
					xMod > 2.9 &&  xMod < 3.2)
					{
						return _StripeColor;
					}

					if (i.posWorld.y > 4.3f && i.posWorld.y < 4.6f && (xMod < 1.8f || xMod > 3.2f))
					{
						return _StripeColor;	
					}
					
					if (i.posWorld.y > 3.7f && i.posWorld.y < 4.0f && (xMod < 1.4f || xMod > 3.6f))
					{
						return _StripeColor;	
					}
					discard;

				}

				if (i.posWorld.y > 1.7f && i.posWorld.y < 2.0f ||
					i.posWorld.y > 2.4f && i.posWorld.y < 2.7f
				)
				{
					return _StripeColor;
				}
				discard;
				// apply fog
				return fixed4(0,0,0,0);
			}
			ENDCG
		}
	}
}