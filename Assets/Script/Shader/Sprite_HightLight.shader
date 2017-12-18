Shader "CapitalismTrap/Sprite/HightLight"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _HightLightTexture ("HightLightTexture", 2D) = "white" {}
        _HightLightParams ("X = HightLight Intensity, Y = HightLight Speed", Vector) = (1.0, 1.0, 0.0, 0.0)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
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
            #pragma vertex MySpriteVert
            #pragma fragment MySpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            sampler2D _HightLightTexture;
            float4 _HightLightParams;

            struct Myv2f
			{
			    float4 vertex   : SV_POSITION;
			    fixed4 color    : COLOR;
			    float3 texcoord : TEXCOORD0;
			    UNITY_VERTEX_OUTPUT_STEREO
			};

            Myv2f MySpriteVert(appdata_t IN)
			{
			    Myv2f OUT;

			    UNITY_SETUP_INSTANCE_ID (IN);
			    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

				#ifdef UNITY_INSTANCING_ENABLED
				    IN.vertex.xy *= _Flip.xy;
				#endif

			    OUT.vertex = UnityObjectToClipPos(IN.vertex);
			    OUT.texcoord.xy = IN.texcoord;
			    OUT.texcoord.z = _Time.y * _HightLightParams.y;
			    OUT.color = IN.color;

			    #ifdef PIXELSNAP_ON
			    OUT.vertex = UnityPixelSnap (OUT.vertex);
			    #endif

			    return OUT;
			}

            fixed4 MySpriteFrag(Myv2f IN) : SV_Target
			{
			    fixed4 c = SampleSpriteTexture (IN.texcoord);

			    fixed4 h = tex2D(_HightLightTexture, float2(IN.texcoord.z, 0.0)).x * IN.color;
			    c.rgb += h.rgb * h.a * _HightLightParams.x;
			    c.rgb *= c.a;
			   	
			    return c;
			}

        ENDCG
        }
    }
}

