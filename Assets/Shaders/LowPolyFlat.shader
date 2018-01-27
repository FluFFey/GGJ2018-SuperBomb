Shader "LowPoly/LowPolyObjects"
{
	Properties
	{

        _MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "Queue" = "Geometry" "LightMode" = "ForwardBase" "RenderType" = "Opaque" }
		//LOD 100 //dunno what this is atm

		Pass
		{
			//Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			#pragma multi_compile_fwdbase


		    // make fog work
			    //#pragma multi_compile_fog

			# include "UnityCG.cginc"
			# include "Lighting.cginc"
			# include "AutoLight.cginc"

			//uniform float4 _LightColor0;
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2g
			{
				float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float4 vertex : TEXCOORD1;
				};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float light : TEXCOORD1;

				LIGHTING_COORDS(2,3)

					//SHADOW_COORDS(3)
					/* I used variable name _ShadowCoord since SHADOW_ATTENUATION uses it (according to "AutoLight.cginc") */
					//float4 _ShadowCoord : TEXCOORD2;
			};

			// I put shadow stuff in separate struct because of naming in some macros
			//https://answers.unity.com/questions/973067/using-shadow-texture-to-recieve-shadows-on-grass.html
			//struct VS_SHADOW
			//{
			//  float4 pos : POSITION;
			//SHADOW_COORDS(0)
			//};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2g vert(appdata v)
			{

				v2g o;
				//v.vertex.y += sin(o.worldPos.x*o.worldPos.z + _Time.x * 10)*0.1;
				o.vertex = v.vertex;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}

			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
			{
				g2f o;

				// Compute the normal
				float3 vecA = IN[1].vertex - IN[0].vertex;
				float3 vecB = IN[2].vertex - IN[0].vertex;
				float3 normal = cross(vecA, vecB);
				normal = normalize(mul(normal, (float3x3)unity_WorldToObject));

				// Compute diffuse light
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				o.light = max(0., dot(normal, lightDir));

				for (int i = 0; i < 3; i++)
				{
					o.uv = IN[i].uv;
					o.pos = IN[i].pos;
					TRANSFER_VERTEX_TO_FRAGMENT(o);

					triStream.Append(o);
				}
			}

			fixed4 frag(g2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
			//fixed4 col = texCol*i.light + texCol*UNITY_LIGHTMODEL_AMBIENT;
			//col.a = 0;
			/*float _GridSpacing = 5.0f;
			float _GridThickness = .02f;
			if (frac(i.worldPos.x / _GridSpacing) < _GridThickness || frac(i.worldPos.z / _GridSpacing) < _GridThickness) 
			{
						col.rgb *= 0.75;
			}*/
				//float atten = LIGHT_ATTENUATION(i);
				col*=LIGHT_ATTENUATION(i);
				//if (atten > 0.0)
				//{
				//	col = fixed4(0.0,0.0f,0.0f,1.0);
				//}
                
				return col* i.light*1 + col* UNITY_LIGHTMODEL_AMBIENT*0.2;
			}
		ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

	}
}
