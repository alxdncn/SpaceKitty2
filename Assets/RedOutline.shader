Shader "Hidden/RedOutline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Outline color", Color) = (0,0,1,1)
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
			float4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 colMine = tex2D(_MainTex, i.uv);

				if(colMine.r != 1 && colMine.g != 0 && colMine.b != 0){
					fixed4 col1 = tex2D(_MainTex, i.uv.xy + half2(0.1, 0));
					if(col1.r == 1 && col1.g == 0 && col1.b == 0)
						return _Color;
					fixed4 col2 = tex2D(_MainTex, i.uv.xy + half2(0.1, -0.1));
					if(col2.r == 1 && col2.g == 0 && col2.b == 0)
						return _Color;
					fixed4 col3 = tex2D(_MainTex, i.uv.xy + half2(0, -0.1));
					if(col3.r == 1 && col3.g == 0 && col3.b == 0)
						return _Color;
					fixed4 col4 = tex2D(_MainTex, i.uv.xy + half2(-0.1, -0.1));
					if(col4.r == 1 && col4.g == 0 && col4.b == 0)
						return _Color;
					fixed4 col5 = tex2D(_MainTex, i.uv.xy + half2(-0.1, 0));
					if(col5.r == 1 && col5.g == 0 && col5.b == 0)
						return _Color;
					fixed4 col6 = tex2D(_MainTex, i.uv.xy + half2(-0.1, 0.1));
					if(col6.r == 1 && col6.g == 0 && col6.b == 0)
						return _Color;
					fixed4 col7 = tex2D(_MainTex, i.uv.xy + half2(0, 0.1));
					if(col7.r == 1 && col7.g == 0 && col7.b == 0)
						return _Color;
					fixed4 col8 = tex2D(_MainTex, i.uv.xy + half2(0.1, 0.1));
					if(col8.r == 1 && col8.g == 0 && col8.b == 0)
						return _Color;
					



					// for(int i = 0; i < 8; i++){
					// 	coords.xy += half2(0.1, 0.1);
					// 	fixed4 col = tex2D(_MainTex, coords);
					// 	if(col.rgb == half3(1,0,0)){
					// 		nextToRed = 1;
					// 	}
					// }
				}
				
				
				// just invert the colors
				return colMine;
			}
			ENDCG
		}
	}
}
