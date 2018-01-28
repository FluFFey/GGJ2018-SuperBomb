// Upgrade NOTE: commented out 'float4x4 _CameraToWorld', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ShowPos Effect Shader"
{
    Properties
    {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,1) 
		_LinesSize("LinesSize", Range(1,10)) = 1 
		_ScaledTime("ScaledTime", float) = 1
    }
 
    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }
               
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
 
			fixed4 _Color;
			half _LinesSize;
			uniform sampler2D _MainTex;
			float _ScaledTime;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float3 ray : TEXCOORD1;
            };
 
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = ComputeScreenPos (o.pos);   
                return o;
            }
 
            float4 frag (v2f i) : COLOR
            {
				float4 c = tex2D(_MainTex, i.uv);

				//if (i.uv.x %5 ==0) discard;
                float2 uv = (i.uv.y+_ScaledTime*0.1f) / i.uv.w;
				if((int)(uv*_ScreenParams.y/floor(_LinesSize))%2==0)
				{
					return c;
				}
				return c*0.75f;
				//return c_Color; 
            }
 
            ENDCG
        }
    }
 
    Fallback off
}