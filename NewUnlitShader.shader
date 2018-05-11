Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_MainColor ("main color", Color) = (255, 255, 255, 255)
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
				half3 normal : NORMAL;
			};

			struct v2f
			{
				half diff : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float4 _MainColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				float4 objectPos = mul(unity_ObjectToWorld, v.vertex);
				float3 tocam = _WorldSpaceCameraPos - objectPos;
				float3 tolight = _WorldSpaceLightPos0.xyz - objectPos;
				o.vertex = UnityObjectToClipPos(v.vertex);
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.diff = max(0, dot(worldNormal, tolight));
				half s = max(0, dot(reflect(-tolight, worldNormal), normalize(tocam)));
				s = s * s * s * s / 2;
				o.diff += s;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _MainColor * (0.5 + .5 * i.diff);
			}
			ENDCG
		}
	}
}
