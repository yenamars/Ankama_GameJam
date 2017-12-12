Shader "Sprites/Real Pixel Snap"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
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
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFrag
            #pragma target 2.0

            inline float4 PixelSnap (float4 pos)
			{
			    float2 hpc = _ScreenParams.xy * 0.5f;
			    float2 pixelPos = round ((pos.xy / pos.w) * hpc);
			    pos.xy = pixelPos / hpc * pos.w;
			    return pos;
			}

			struct appdata_t
			{
			    float4 vertex   : POSITION;
			    float4 color    : COLOR;
			    float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
			    float4 vertex   : SV_POSITION;
			    fixed4 color    : COLOR;
			    float2 texcoord : TEXCOORD0;
			};

			v2f SpriteVert(appdata_t IN)
			{
			    v2f OUT;

			    OUT.vertex = UnityObjectToClipPos(IN.vertex);
			    OUT.texcoord = IN.texcoord;
			    OUT.color = IN.color;
			    OUT.vertex = PixelSnap (OUT.vertex);

			    return OUT;
			}

			sampler2D _MainTex;

			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
			    fixed4 c = tex2D (_MainTex, IN.texcoord) * IN.color;
			    c.rgb *= c.a;
			    return c;
			}

        ENDCG
        }
    }
}
