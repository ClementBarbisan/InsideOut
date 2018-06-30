Shader "Custom/DistordShader" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_ColorHSBC ("HSBC", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Off
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _ColorHSBC;
		fixed4 _Color;

		//
		// Noise Shader Library for Unity - https://github.com/keijiro/NoiseShader
		//
		// Original work (webgl-noise) Copyright (C) 2011 Stefan Gustavson
		// Translation and modification was made by Keijiro Takahashi.
		//
		// This shader is based on the webgl-noise GLSL shader. For further details
		// of the original shader, please see the following description from the
		// original source code.
		//

		//
		// GLSL textureless classic 2D noise "cnoise",
		// with an RSL-style periodic variant "pnoise".
		// Author:  Stefan Gustavson (stefan.gustavson@liu.se)
		// Version: 2011-08-22
		//
		// Many thanks to Ian McEwan of Ashima Arts for the
		// ideas for permutation and gradient selection.
		//
		// Copyright (c) 2011 Stefan Gustavson. All rights reserved.
		// Distributed under the MIT license. See LICENSE file.
		// https://github.com/ashima/webgl-noise
		//

		float4 mod(float4 x, float4 y)
		{
			return x - y * floor(x / y);
		}

		float4 mod289(float4 x)
		{
			return x - floor(x / 289.0) * 289.0;
		}

		float4 permute(float4 x)
		{
			return mod289(((x*34.0) + 1.0)*x);
		}

		float4 taylorInvSqrt(float4 r)
		{
			return (float4)1.79284291400159 - r * 0.85373472095314;
		}

		float2 fade(float2 t) {
			return t * t*t*(t*(t*6.0 - 15.0) + 10.0);
		}

		// Classic Perlin noise
		float cnoise(float2 P)
		{
			float4 Pi = floor(P.xyxy) + float4(0.0, 0.0, 1.0, 1.0);
			float4 Pf = frac(P.xyxy) - float4(0.0, 0.0, 1.0, 1.0);
			Pi = mod289(Pi); // To avoid truncation effects in permutation
			float4 ix = Pi.xzxz;
			float4 iy = Pi.yyww;
			float4 fx = Pf.xzxz;
			float4 fy = Pf.yyww;

			float4 i = permute(permute(ix) + iy);

			float4 gx = frac(i / 41.0) * 2.0 - 1.0;
			float4 gy = abs(gx) - 0.5;
			float4 tx = floor(gx + 0.5);
			gx = gx - tx;

			float2 g00 = float2(gx.x, gy.x);
			float2 g10 = float2(gx.y, gy.y);
			float2 g01 = float2(gx.z, gy.z);
			float2 g11 = float2(gx.w, gy.w);

			float4 norm = taylorInvSqrt(float4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11)));
			g00 *= norm.x;
			g01 *= norm.y;
			g10 *= norm.z;
			g11 *= norm.w;

			float n00 = dot(g00, float2(fx.x, fy.x));
			float n10 = dot(g10, float2(fx.y, fy.y));
			float n01 = dot(g01, float2(fx.z, fy.z));
			float n11 = dot(g11, float2(fx.w, fy.w));

			float2 fade_xy = fade(Pf.xy);
			float2 n_x = lerp(float2(n00, n01), float2(n10, n11), fade_xy.x);
			float n_xy = lerp(n_x.x, n_x.y, fade_xy.y);
			return 2.3 * n_xy;
		}

		// Classic Perlin noise, periodic variant
		float pnoise(float2 P, float2 rep)
		{
			float4 Pi = floor(P.xyxy) + float4(0.0, 0.0, 1.0, 1.0);
			float4 Pf = frac(P.xyxy) - float4(0.0, 0.0, 1.0, 1.0);
			Pi = mod(Pi, rep.xyxy); // To create noise with explicit period
			Pi = mod289(Pi);        // To avoid truncation effects in permutation
			float4 ix = Pi.xzxz;
			float4 iy = Pi.yyww;
			float4 fx = Pf.xzxz;
			float4 fy = Pf.yyww;

			float4 i = permute(permute(ix) + iy);

			float4 gx = frac(i / 41.0) * 2.0 - 1.0;
			float4 gy = abs(gx) - 0.5;
			float4 tx = floor(gx + 0.5);
			gx = gx - tx;

			float2 g00 = float2(gx.x, gy.x);
			float2 g10 = float2(gx.y, gy.y);
			float2 g01 = float2(gx.z, gy.z);
			float2 g11 = float2(gx.w, gy.w);

			float4 norm = taylorInvSqrt(float4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11)));
			g00 *= norm.x;
			g01 *= norm.y;
			g10 *= norm.z;
			g11 *= norm.w;

			float n00 = dot(g00, float2(fx.x, fy.x));
			float n10 = dot(g10, float2(fx.y, fy.y));
			float n01 = dot(g01, float2(fx.z, fy.z));
			float n11 = dot(g11, float2(fx.w, fy.w));

			float2 fade_xy = fade(Pf.xy);
			float2 n_x = lerp(float2(n00, n01), float2(n10, n11), fade_xy.x);
			float n_xy = lerp(n_x.x, n_x.y, fade_xy.y);
			return 2.3 * n_xy;
		}

		inline float3 applyHue(float3 aColor, float aHue)
		{
			float angle = radians(aHue);
			float3 k = float3(0.57735, 0.57735, 0.57735);
			float cosAngle = cos(angle);
			//Rodrigues' rotation formula
			return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
		}


		inline float4 applyHSBEffect(float4 startColor, fixed4 hsbc)
		{
			float _Hue = 360 * hsbc.r;
			float _Brightness = hsbc.g * 2 - 1;
			float _Contrast = hsbc.b * 2;
			float _Saturation = hsbc.a * 2;

			float4 outputColor = startColor;
			outputColor.rgb = applyHue(outputColor.rgb, _Hue);
			outputColor.rgb = (outputColor.rgb - 0.5f) * (_Contrast)+0.5f;
			outputColor.rgb = outputColor.rgb + _Brightness;
			float3 intensity = dot(outputColor.rgb, float3(0.299, 0.587, 0.114));
			outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);

			return outputColor;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 valueBump = tex2D(_BumpMap, IN.uv_MainTex);
			//Get displacement from normal with noise
			fixed4 c = tex2D (_MainTex, (IN.uv_MainTex + half2(unity_DeltaTime.z, _SinTime.w / 10 * _CosTime.w / 10) + half2(pnoise(float2(valueBump.x * _CosTime.x, valueBump.y * _CosTime.y), unity_DeltaTime.z) / 2, pnoise(float2(valueBump.y * _SinTime.x, valueBump.z * _SinTime.y), unity_DeltaTime.z) / 2))) * _Color;
			c = applyHSBEffect(c, _ColorHSBC);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
