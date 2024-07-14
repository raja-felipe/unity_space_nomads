Shader "Custom/OutlineWithWhiteFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0.001, 0.1)) = 0.01
        _MaskTex ("Mask Texture", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Pass
        {
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
 
            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineWidth;
            sampler2D _MaskTex;
 
            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the main texture
                fixed4 texColor = tex2D(_MainTex, i.uv);
 
                // Calculate the outline based on the distance to edges
                float4 outline = texColor;
                float2 d = fwidth(i.uv);
                float outlineValue = _OutlineWidth;
                outlineValue = smoothstep(0.5 - d, 0.5 + d, outlineValue);
                outline.rgb = _OutlineColor.rgb * outlineValue;
                outline.a = max(texColor.a, outline.a);
 
                // Sample the mask texture (checkerboard pattern)
                fixed4 maskTexColor = tex2D(_MaskTex, i.uv);

                // Check if the pixel should be white based on the mask
                if (maskTexColor.r < 0.5)
                {
                    // Set the pixel color to white
                    texColor = fixed4(1, 1, 1, 1);
                }
 
                // Combine the main texture color and the outline
                return texColor + outline;
            }
            ENDCG
        }
    }
}
