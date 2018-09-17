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

			sampler2D _MainTex;
			uniform sampler2D _WebCamTex;
			uniform float _SaturationThreshold;
			uniform fixed4 _RenderColor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed2 newUV = i.uv;
				newUV.x = 1 - newUV.x;
				fixed4 webCamCol = tex2D(_WebCamTex, newUV);

				float saturation = min((webCamCol.r + webCamCol.g + webCamCol.b) / 3, 1);

				// red is 1, black is 0
				float webCamAdjuster = clamp(ceil(saturation - _SaturationThreshold), 0, 1);

				col = col * (1 - webCamAdjuster) + _RenderColor * (webCamAdjuster);
				return col;
			}
			ENDCG
		}
		GrabPass
        {
            "_BackgroundTexture"
        }
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
			
			sampler2D _BackgroundTexture;
			float4 _OutlineColor;
			uniform fixed4 _RenderColor;
			float _Increment;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 colMine = tex2D(_BackgroundTexture, i.uv);
				fixed4 c = _RenderColor;
				_Increment = 0.005;

				if(colMine.r != c.r && colMine.g != c.g && colMine.b != c.b){
					fixed4 col1 = tex2D(_BackgroundTexture, i.uv.xy + half2(_Increment, 0));
					if(col1.r == c.r && col1.g == c.g && col1.b == c.b)
						return _OutlineColor;
					fixed4 col2 = tex2D(_BackgroundTexture, i.uv.xy + half2(_Increment, -_Increment));
					if(col2.r == c.r && col2.g == c.g && col2.b == c.b)
						return _OutlineColor;
					fixed4 col3 = tex2D(_BackgroundTexture, i.uv.xy + half2(0, -_Increment));
					if(col3.r == c.r && col3.g == c.g && col3.b == c.b)
						return _OutlineColor;
					fixed4 col4 = tex2D(_BackgroundTexture, i.uv.xy + half2(-_Increment, -_Increment));
					if(col4.r == c.r && col4.g == c.g && col4.b == c.b)
						return _OutlineColor;
					fixed4 col5 = tex2D(_BackgroundTexture, i.uv.xy + half2(-_Increment, 0));
					if(col5.r == c.r && col5.g == c.g && col5.b == c.b)
						return _OutlineColor;
					fixed4 col6 = tex2D(_BackgroundTexture, i.uv.xy + half2(-_Increment, _Increment));
					if(col6.r == c.r && col6.g == c.g && col6.b == c.b)
						return _OutlineColor;
					fixed4 col7 = tex2D(_BackgroundTexture, i.uv.xy + half2(0, _Increment));
					if(col7.r == c.r && col7.g == c.g && col7.b == c.b)
						return _OutlineColor;
					fixed4 col8 = tex2D(_BackgroundTexture, i.uv.xy + half2(_Increment, _Increment));
					if(col8.r == c.r && col8.g == c.g && col8.b == c.b)
						return _OutlineColor;

				}
				
				return colMine;
			}
			ENDCG
		}
	}
}
