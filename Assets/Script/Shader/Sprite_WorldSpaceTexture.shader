// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "CapitalismTrap/Sprite/WorldSpaceTexture"
{
    Properties
    {
    	_TextureSize ("TextureSize", Vector) = (1.0, 1.0, 1.0, 1.0)
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
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
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

        float4 _TextureSize;

        v2f MySpriteVert(appdata_t IN)
		{
		    v2f OUT;

		    UNITY_SETUP_INSTANCE_ID (IN);
		    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

		#ifdef UNITY_INSTANCING_ENABLED
		    IN.vertex.xy *= _Flip.xy;
		#endif

		    OUT.vertex = UnityObjectToClipPos(IN.vertex);
		    OUT.texcoord = mul(unity_ObjectToWorld, IN.vertex).xy / _TextureSize.xy;
		    OUT.color = IN.color * _Color * _RendererColor;

		    #ifdef PIXELSNAP_ON
		    OUT.vertex = UnityPixelSnap (OUT.vertex);
		    #endif

		    return OUT;
		}

        ENDCG
        }
    }
}

