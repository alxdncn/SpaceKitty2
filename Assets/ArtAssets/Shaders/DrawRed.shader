Shader "Hidden/DrawRed"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			uniform sampler2D _WebCamTex;
			uniform float _SaturationThreshold;
			uniform fixed4 _RenderColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed2 newUV = i.uv;
				newUV.x = 1 - newUV.x;
				fixed4 webCamCol = tex2D(_WebCamTex, newUV);

				float saturation = min((webCamCol.r + webCamCol.g + webCamCol.b) / 3, 1);

				//pink is 1, blue is 0
				float webCamAdjuster = clamp(ceil(saturation - _SaturationThreshold), 0, 1);

				col = col * (1 - webCamAdjuster) + _RenderColor * (webCamAdjuster);
				return col;
			}
			ENDCG
		}
	}
}
