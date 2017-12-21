Shader "CapitalismTrap/PostProcess/Hit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_HitColor ("HitColor", Color) = (1.0, 0.0, 0.0, 1.0)
		_HitParams ("XY = Vignet Smoothstep, Z = Noise Speed, W = Chromatic Aberration", Vector) = (0.0, 0.0, 0.0, 0.0)

	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float _Hit;
			float4 _HitParams;
			fixed4 _HitColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed v = smoothstep(_HitParams.x, _HitParams.y, length(i.uv - 0.5));
				fixed4 n = tex2D(_NoiseTex, float2(_Time.y, 0.0) * _HitParams.z)-0.5;

				fixed4 col = tex2D(_MainTex, i.uv);
				fixed colR = tex2D(_MainTex, i.uv + v*n.x*_HitParams.w*_Hit).x;
//				fixed colG = tex2D(_MainTex, i.uv + v*n.y*_HitParams.w*_Hit).y;
//				fixed colB = tex2D(_MainTex, i.uv + v*n.z*_HitParams.w*_Hit).z;
				fixed4 colHit = fixed4(colR, col.y, col.z, 1.0);
				return lerp(col, colHit * (1.0 + v * _HitColor * 2.0), _Hit);
			}
			ENDCG
		}
	}
}
