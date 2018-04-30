Shader "test"
{
	Properties
	{
		_ActiveColor ("Active Color", Color) = (1,1,1,1)
		_ChargeColor ("Charge Color", Color) = (0,0,0,1)
		_InactiveColor ("Inactive Color", Color) = (0,0,0,1)
		_IsActive ("Is Active", Float) = 1
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
			};

			sampler2D _ActiveTex;
			sampler2D _InactiveTex;
			float4 _ActiveTex_ST;
			float _charged;
			float selected;

			fixed4 _ActiveColor;
			fixed4 _InactiveColor;
			fixed4 _ChargeColor;
			float _IsActive;
			
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _ActiveTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				selected = step(_charged, i.uv.y);
				fixed4 picCol = (1-selected) * tex2D(_ActiveTex, i.uv) + 
								selected * tex2D(_InactiveTex, i.uv);
				fixed4 backCol = (1 - selected) * ((_IsActive)*_ActiveColor + (1-_IsActive)*_ChargeColor)
									+ selected * _InactiveColor;
				return (picCol + (1 - picCol.w) * backCol);
			}
			ENDCG
		}
	}
}
