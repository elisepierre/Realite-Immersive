Shader "UI/FlameDissolve"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NoiseTex("Noise", 2D) = "white" {}
        _DissolveColor("Edge Color", Color) = (1,0.5,0,1)
        _DissolveAmount("Dissolve Amount", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float _DissolveAmount;
            float4 _DissolveColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed noise = tex2D(_NoiseTex, i.uv).r;

                // Dissolve effect
                if(noise < _DissolveAmount)
                {
                    // Bord de la brûlure
                    col.rgb = lerp(col.rgb, _DissolveColor.rgb, 1.0);
                    col.a = 1;
                }

                // Transparence progressive
                if(noise < _DissolveAmount - 0.1)
                    col.a = 0;

                return col * col.a;
            }
            ENDCG
        }
    }
}

