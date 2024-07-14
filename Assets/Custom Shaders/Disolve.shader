Shader "Unlit/Disolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TextureForDisolve("disolveTexture", 2D) = "white" {}
        _DisolveY("texture disolve from Y", Float) = 0
        _DisolveSize("how big of the disolve", Float) = 5
        _Starting("Starting point", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _TextureForDisolve;
            float _DisolveY;
            float _DisolveSize;
            float _Starting;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float transition = _DisolveY - i.worldPos.y;
                clip(_Starting + (transition + (tex2D(_TextureForDisolve, i.uv)) * _DisolveSize));
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
