
Shader "Custom/FresnelTest"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    	_ScalingFactor ("Scaling",float)  = 1
    	_PowerFactor ("power",float)  = 1
		_UseTexture ("useTexture", float) = 0
		_MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass{
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            			
			#include "UnityCG.cginc"
			uniform sampler2D _MainTex;
			uniform float4 _Color;
			uniform float _UseTexture;
            uniform float _ScalingFactor;
            uniform float _PowerFactor;

            struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Convert Vertex position and corresponding normal into world coords.
				// Note that we have to multiply the normal by the transposed inverse of the world 
				// transformation matrix (for cases where we have non-uniform scaling; we also don't
				// care about the "fourth" dimension, because translations don't affect the normal) 
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
				
				float3 differenceVector = normalize(worldVertex.xyz - _WorldSpaceCameraPos);

				float fresnelRate = dot(differenceVector,worldNormal);
				o.color = v.color - float4(1,1,1,0) + _ScalingFactor *  _Color * pow(1 + fresnelRate,_PowerFactor);
				// Transform vertex in world coordinates to camera coordinates
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				float4 color = v.color;
				if(_UseTexture > 0.5)
				{
					color = v.color + tex2D(_MainTex, v.uv);
				}
				return color;
			}
			ENDCG
        }
    }
    FallBack "Diffuse"
}
