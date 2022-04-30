Shader "Tetris/Block"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BoundsMap ("Bounds map", Color) = (-0.5, -0.5, 12, 12)
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="TransparentCutout"
            "RenderQueue"="AlphaTest" 
            "IgnoreProjector"="True"
            "ForceNoShadowCasting"="True"
        }

        Lighting Off

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
                float4 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BoundsMap;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord.xy = TRANSFORM_TEX(v.uv, _MainTex).xy;
                o.texcoord.zw = v.vertex.xy;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.texcoord.xy;
                float2 startMap = _BoundsMap.xy;
                float2 endMap = _BoundsMap.zw;
                float posWorldX = (i.texcoord.z - startMap.x) / (endMap.x - startMap.x);
                float posWorldY = (i.texcoord.w - startMap.y) / (endMap.y - startMap.y);

                clip(0.5 - abs(posWorldX - 0.5));          
                clip(0.5 - abs(posWorldY - 0.5));       

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
