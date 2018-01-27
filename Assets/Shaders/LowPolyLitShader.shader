Shader "LowPoly/LowPolyTerrain"
{
	//UNITY_SHADER_NO_UPGRADE
	Properties
	{
        _MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		

		Pass
		{
			Tags { "Queue" = "Geometry" "LightMode" = "ForwardBase" "RenderType" = "Opaque" }
			//LOD 100 //dunno what this is atm
			//Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			
			#pragma multi_compile_fwdbase

			// make fog work
			//#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			uniform float4 _LightColor0;
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;

			};

			struct v2g
			{
				float4 pos : SV_POSITION;
				float4 vertex : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float light : TEXCOORD0;
				float ambientLight : TEXCOORD4; //NOTE: this is my weird way of slightly lighting up the caves un-equally
				//float4 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD1;
				LIGHTING_COORDS(2,3)

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2g vert (appdata v)
			{

				v2g o;
				o.vertex = v.vertex;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);			
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
				normal = normalize(mul(normal, (float3x3) unity_WorldToObject));

				// Compute diffuse light
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				o.light = max(0., dot(normal, lightDir));
				o.ambientLight = max(0.0, dot(normal, float3(0.58,0.58,0.58)));
				o.ambientLight += max(0.0, dot(normal, float3(-0.58,-0.58,-0.58)));
				float4 squarePos = (IN[1].worldPos + IN[2].worldPos)/2;
				float heightDiff = abs(IN[1].vertex.y - IN[2].vertex.y);
				o.uv = IN[1].uv;

				for (int i = 0; i < 3; i++)
				{
					//o.worldPos = IN[i].worldPos;
					
					o.pos = IN[i].pos;
					TRANSFER_VERTEX_TO_FRAGMENT(o)
					//o.fogCoordy = IN[i].fogCoord;
					triStream.Append(o);
				}
			}

			fixed4 frag (g2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float _GridSpacing = 5.0f;
				float _GridThickness = .02f;
				return col*i.light*(LIGHT_ATTENUATION(i)) +
					   col*i.ambientLight*UNITY_LIGHTMODEL_AMBIENT;
			}
			ENDCG
			}
		Pass
		{
			Tags { "Queue" = "Geometry" "LightMode" = "ForwardAdd" /*"RenderType" = "Opaque"*/ }
			//LOD 100 //dunno what this is atm
			Blend One One
			CGPROGRAM
			
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			
			//#pragma multi_compile_fwdadd

			// make fog work
			//#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

			//uniform float4 _LightColor0; 
			uniform float4x4 _LightMatrix0;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2g
			{
				float4 pos : SV_POSITION;
				float4 vertex : TEXCOORD0;
				float4 worldPos : TEXCOORD1;

				float3 normalDir : TEXCOORD2;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float4 col : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float4 vertex : TEXCOORD2;
				LIGHTING_COORDS(3,4)
			};

			v2g vert (appdata v)
			{
				v2g o;
				o.vertex = v.vertex;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);

				return o;
			}

			//https://en.wikibooks.org/wiki/Cg_Programming/Unity/Light_Attenuation and gamedesign project shader
			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
			{
				g2f o;

				// Compute the normal
				float3 vecA = IN[1].vertex - IN[0].vertex;
				float3 vecB = IN[2].vertex - IN[0].vertex;
				float3 normal = cross(vecA, vecB);

				//find centre of square
				o.worldPos = (IN[1].worldPos + IN[2].worldPos)/2;
				fixed3 vertexToLightSource = _WorldSpaceLightPos0.xyz - o.worldPos.xyz;
				float3 lightDir = normalize(vertexToLightSource);
				float3 diffuseReflectionWithoutAttenuation =  _LightColor0.rgb* max(0.0, dot(normal, lightDir));
				o.col = fixed4(diffuseReflectionWithoutAttenuation,1);
				for (int i = 0; i < 3; i++)
				{
					o.pos = IN[i].pos;
					o.vertex = IN[i].vertex;
					TRANSFER_VERTEX_TO_FRAGMENT(o)
					triStream.Append(o);
				}
			}

			fixed4 frag (g2f i) : SV_Target
			{
				fixed4 vertexWorld = mul(unity_ObjectToWorld, i.vertex);
				fixed3 lightCoord = mul(_LightMatrix0, vertexWorld).xyz;
				float3 toLight = _WorldSpaceLightPos0.xyz - vertexWorld.xyz;
				fixed lightRange = length(toLight) / length(lightCoord);

				fixed3 vertexToLightSource = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
				float distance = length(vertexToLightSource);

				float attenuation = 1.0f-(distance / lightRange);
				return (attenuation)*i.col*LIGHT_ATTENUATION(i);
			}
		ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}