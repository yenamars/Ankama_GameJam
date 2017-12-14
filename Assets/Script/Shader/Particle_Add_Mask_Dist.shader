Shader "CapitalismTrap/Particles/Add Mask Dist"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Noise ("Noise", 2D) = "white" {}
		_NoiseParams ("NoiseParams", Vector) = (1.0, 1.0, 0.0, 0.0)
	}
	SubShader
	{
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 custom : TEXCOORD1;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float3 custom : TEXCOORD1;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			sampler2D _Noise;
			float4 _NoiseParams;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = v.uv;
				o.uv.zw = v.uv * _NoiseParams.xy + _NoiseParams.zw * v.uv.zw;
				o.custom = v.custom;
				o.color = v.color;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 n = tex2D(_Noise, i.uv.zw) - 0.5;
				fixed4 tex = tex2D(_MainTex, i.uv.xy + n * i.custom.xy);

				tex.a = step(i.custom.z+0.01, tex.a);

				fixed4 col = i.color * tex;

				return col;
			}
			ENDCG
		}
	}
}
