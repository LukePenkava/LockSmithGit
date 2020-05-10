// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Clock"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,0)
		_Emission("Emission", Color) = (1,1,1,0)
		_Cutoff( "Mask Clip Value", Float ) = 0
		_FillLinear("Fill Linear", Range( -1 , 2)) = 1
		_FillSpherical("Fill Spherical", Range( -1 , 2)) = 1
		_FillClock("Fill Clock", Range( 0 , 1)) = 1
		[Toggle(_CLOCKDIRECTION_ON)] _ClockDirection("Clock Direction", Float) = 0
		_Degrees("Degrees", Float) = 0
		[Toggle]_Axis_X("Axis_X", Range( 0 , 1)) = 0
		[Toggle]_Axis_Y("Axis_Y", Range( 0 , 1)) = 1
		[Toggle]_Axis_Z("Axis_Z", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		Stencil
		{
			Ref 29
		}
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _CLOCKDIRECTION_ON
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _Color;
		uniform float4 _Emission;
		uniform float _Axis_X;
		uniform float _Axis_Y;
		uniform float _Axis_Z;
		uniform float _Degrees;
		uniform float _FillSpherical;
		uniform float _FillClock;
		uniform float _FillLinear;
		uniform float _Cutoff = 0;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color.rgb;
			o.Emission = _Emission.rgb;
			o.Alpha = 1;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 appendResult299 = (float4(ase_vertex3Pos.y , ase_vertex3Pos.z , 0.0 , 0.0));
			float4 appendResult305 = (float4(ase_vertex3Pos.x , ase_vertex3Pos.y , 0.0 , 0.0));
			float4 ifLocalVar307 = 0;
			if( _Axis_X >= _Axis_Z )
				ifLocalVar307 = appendResult299;
			else
				ifLocalVar307 = appendResult305;
			float4 appendResult298 = (float4(ase_vertex3Pos.x , ase_vertex3Pos.z , 0.0 , 0.0));
			float4 ifLocalVar317 = 0;
			if( _Axis_X >= _Axis_Y )
				ifLocalVar317 = ifLocalVar307;
			else
				ifLocalVar317 = appendResult298;
			float4 break302 = ifLocalVar317;
			float temp_output_380_0 = radians( _Degrees );
			float temp_output_383_0 = cos( temp_output_380_0 );
			float temp_output_382_0 = sin( temp_output_380_0 );
			float VP_X242 = ( ( break302.x * temp_output_383_0 ) - ( break302.y * temp_output_382_0 ) );
			float VP_Z243 = ( ( temp_output_382_0 * break302.x ) + ( break302.y * temp_output_383_0 ) );
			float4 appendResult249 = (float4(VP_X242 , 0.0 , VP_Z243 , 0.0));
			float temp_output_233_0 = ( distance( float4( (float3(0,0,0)).xz, 0.0 , 0.0 ) , appendResult249 ) + ( ( ( _FillSpherical - 1.0 ) * 0.5 ) * 1.001 ) );
			#ifdef _CLOCKDIRECTION_ON
				float staticSwitch376 = atan2( VP_Z243 , VP_X242 );
			#else
				float staticSwitch376 = atan2( VP_X242 , VP_Z243 );
			#endif
			float temp_output_239_0 = ( ( _FillClock * 2.0 ) - 1.0 );
			float temp_output_230_0 = ( staticSwitch376 + ( temp_output_239_0 * UNITY_PI ) );
			float temp_output_264_0 = (( temp_output_233_0 < temp_output_230_0 ) ? temp_output_233_0 :  temp_output_230_0 );
			float4 temp_cast_3 = (temp_output_264_0).xxxx;
			float4 appendResult272 = (float4(VP_X242 , 0.0 , VP_Z243 , 0.0));
			float4 temp_output_275_0 = ( appendResult272 + ( ( _FillLinear - 0.5 ) * 1.001 ) );
			float4 temp_cast_4 = (temp_output_264_0).xxxx;
			float4 temp_output_283_0 = (( temp_cast_3 < temp_output_275_0 ) ? temp_cast_4 :  temp_output_275_0 );
			clip( temp_output_283_0.x - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
0;75;1610;954;-4447.362;183.4852;2.081629;True;True
Node;AmplifyShaderEditor.CommentaryNode;319;2431.664,-2212.902;Inherit;False;2344.148;976.7227;;10;302;296;304;307;295;317;298;299;305;228;AXIS, VP;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;228;2436.694,-1786.171;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;304;2839.701,-2017.22;Float;False;Property;_Axis_Z;Axis_Z;12;1;[Toggle];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;296;2844.444,-2114.896;Float;False;Property;_Axis_X;Axis_X;10;1;[Toggle];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;299;2890.486,-1704.38;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;305;2885.785,-1887.372;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ConditionalIfNode;307;3301.688,-1829.934;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;381;4520.198,-1198.003;Float;False;Property;_Degrees;Degrees;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;295;3367.746,-1566.902;Float;False;Property;_Axis_Y;Axis_Y;11;1;[Toggle];Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;298;2912.113,-1493.846;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RadiansOpNode;380;4719.715,-1191.502;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;317;3704.401,-1583.495;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SinOpNode;382;4936.816,-1175.902;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;383;4940.716,-1084.903;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;302;4055.5,-1583.15;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;389;5306.014,-919.8021;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;385;5195.517,-1269.503;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;384;5189.013,-1374.801;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;388;5287.814,-1053.702;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;386;5415.213,-1318.901;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;390;5519.213,-964.002;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;276;2573.337,-966.0212;Inherit;False;2022.392;561.8124;;14;233;266;265;251;252;259;250;254;226;225;249;224;247;248;SPHERICAL;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;277;3410.504,-241.6117;Inherit;False;1195.015;496.6387;;11;244;230;232;229;239;245;240;238;241;231;377;CLOCK;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;243;5826.63,-973.1738;Float;False;VP_Z;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;242;5803.549,-1315.063;Float;False;VP_X;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;241;3646.622,16.56621;Float;False;Constant;_Float35;Float 35;13;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;250;3432.445,-799.0057;Float;False;Property;_FillSpherical;Fill Spherical;4;0;Create;True;0;0;False;0;1;0;-1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;231;3521.61,-74.06392;Float;False;Property;_FillClock;Fill Clock;5;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;254;3559.629,-717.2267;Float;False;Constant;_Float36;Float 36;15;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;318;3438.088,350.8513;Inherit;False;1170.549;556.9418;;9;275;282;272;273;274;280;281;269;279;LINEAR;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;238;3854.643,117.36;Float;False;Constant;_Float34;Float 34;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;252;3550.033,-623.1348;Float;False;Constant;_Float33;Float 33;13;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;224;2748.079,-901.1962;Float;False;Constant;_Center;Center;11;0;Create;True;0;0;False;0;0,0,0;1,1,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;247;2688.859,-712.3165;Inherit;False;242;VP_X;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;245;4018.852,-121.9027;Inherit;False;243;VP_Z;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;259;3763.258,-753.8531;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;240;3842.845,-45.62555;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;244;4017.176,-201.9058;Inherit;False;242;VP_X;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;248;2691.857,-639.3134;Inherit;False;243;VP_Z;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;269;3750.93,577.0388;Float;False;Property;_FillLinear;Fill Linear;3;0;Create;True;0;0;False;0;1;0;-1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;377;4262.277,-235.6649;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;225;3024.959,-902.3685;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;251;3932.315,-639.9108;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;249;2914.858,-708.3149;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ATan2OpNode;229;4262.682,-139.9809;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;279;3862.155,655.9191;Float;False;Constant;_Float40;Float 40;16;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;266;3892.771,-517.942;Float;False;Constant;_Float37;Float 37;15;0;Create;True;0;0;False;0;1.001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;239;4044.433,99.13148;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;281;4081.821,718.1056;Float;False;Constant;_Float38;Float 38;15;0;Create;True;0;0;False;0;1.001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;280;4106.231,581.083;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;376;4438.719,-215.4733;Float;False;Property;_ClockDirection;Clock Direction;7;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;232;4222.464,98.29524;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;274;3863.154,452.4038;Inherit;False;243;VP_Z;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;4092.379,-536.5044;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;226;3270.96,-875.3685;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;273;3860.156,379.4006;Inherit;False;242;VP_X;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;272;4086.153,383.4023;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;282;4281.44,699.5432;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;230;4490.225,-57.31931;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;233;4448.257,-876.0933;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareLower;264;4858.3,-119.8966;Inherit;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;275;4462.227,554.5551;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;450;5530.3,1654.977;Float;False;Property;_DitherOpacity;DitherOpacity;16;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;415;5943.609,1445.041;Inherit;False;1;False;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;322;6152.676,653.3901;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;393;4115.663,1575.935;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;437;4116.461,1911.828;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;398;4645.403,1319.8;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareLower;283;5160.982,500.3188;Inherit;False;4;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;472;5785.465,1827.975;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;444;4821.76,1677.749;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;430;6333.681,1247.338;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;438;4414.176,1968.599;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;449;5051.632,1969.07;Float;False;Property;_Tiling;Tiling;15;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;326;6160.591,848.99;Float;False;Property;_Emission;Emission;1;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;320;4221.599,215.9974;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;474;6680.314,1690.088;Inherit;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;321;4021.991,234.5599;Float;False;Constant;_Float0;Float 0;15;0;Create;True;0;0;False;0;1.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;439;4201.276,2085.048;Float;False;Property;_HologramSpeed;HologramSpeed;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;471;6226.992,1776.784;Inherit;True;Property;_TextureSample0;Texture Sample 0;17;0;Create;True;0;0;False;0;-1;ae1de1de4f33ab04c92f6b64a2733713;ae1de1de4f33ab04c92f6b64a2733713;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;435;4615.521,1857.636;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;473;6236.64,1646.365;Inherit;False;Constant;_Float9;Float 9;18;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;433;5264.467,1487.457;Inherit;True;Property;_HologramTexture;HologramTexture;13;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;436;4327.508,1752.25;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;443;5080.846,1778.62;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;447;5344.832,1909.47;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;451;5718.218,1532.252;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;364;2830.164,1611.222;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;392;3880.127,1698.857;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;470;4255.586,1253.886;Float;False;Constant;_Float8;Float 8;15;0;Create;True;0;0;False;0;1.001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;358;2839.734,1321.425;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;357;2640.273,1316.244;Float;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;356;2650.635,1414.68;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;363;2641.064,1704.477;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;366;2630.703,1606.041;Float;False;Constant;_Float7;Float 7;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;359;3045.672,1322.721;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;354;2306.534,1436.895;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;355;2478.131,1409.909;Float;False;Constant;_Float2;Float 2;13;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareLower;405;5467.945,1277.214;Inherit;False;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ATan2OpNode;373;3277.963,1312.524;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;340;3290.093,1473.994;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;469;4455.205,1235.324;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;362;2468.563,1699.706;Float;False;Constant;_Float6;Float 6;13;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;353;2052.675,1562.529;Float;False;Constant;_Float1;Float 1;14;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;360;2049.107,1973.327;Float;False;Constant;_Float5;Float 5;14;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;339;1989.656,1841.09;Inherit;False;243;VP_Z;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;338;2013.422,1430.94;Inherit;False;242;VP_X;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;375;3485.365,1439.772;Float;False;Property;_PacmanDirection;Pacman Direction;8;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;399;3921.856,1350.454;Float;False;Property;_FillPacman;Fill Pacman;6;0;Create;True;0;0;False;0;1;0;-1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;336;3644.244,1593.099;Float;False;Constant;_Float3;Float 3;12;0;Create;True;0;0;False;0;2;180;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;402;4035.2,1251.382;Float;False;Constant;_Float11;Float 11;19;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;404;4261.652,1350.973;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;367;3864.036,1574.375;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;365;3036.101,1612.518;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;361;2302.966,1847.692;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;6599.865,753.5212;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Clock;False;False;False;False;False;False;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0;True;False;0;False;TransparentCutout;;AlphaTest;ForwardOnly;14;all;True;True;True;True;0;False;-1;True;29;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;2;False;-1;3;False;-1;0;2;False;-1;0;False;-1;0;False;-1;5;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;299;0;228;2
WireConnection;299;1;228;3
WireConnection;305;0;228;1
WireConnection;305;1;228;2
WireConnection;307;0;296;0
WireConnection;307;1;304;0
WireConnection;307;2;299;0
WireConnection;307;3;299;0
WireConnection;307;4;305;0
WireConnection;298;0;228;1
WireConnection;298;1;228;3
WireConnection;380;0;381;0
WireConnection;317;0;296;0
WireConnection;317;1;295;0
WireConnection;317;2;307;0
WireConnection;317;3;307;0
WireConnection;317;4;298;0
WireConnection;382;0;380;0
WireConnection;383;0;380;0
WireConnection;302;0;317;0
WireConnection;389;0;302;1
WireConnection;389;1;383;0
WireConnection;385;0;302;1
WireConnection;385;1;382;0
WireConnection;384;0;302;0
WireConnection;384;1;383;0
WireConnection;388;0;382;0
WireConnection;388;1;302;0
WireConnection;386;0;384;0
WireConnection;386;1;385;0
WireConnection;390;0;388;0
WireConnection;390;1;389;0
WireConnection;243;0;390;0
WireConnection;242;0;386;0
WireConnection;259;0;250;0
WireConnection;259;1;254;0
WireConnection;240;0;231;0
WireConnection;240;1;241;0
WireConnection;377;0;245;0
WireConnection;377;1;244;0
WireConnection;225;0;224;0
WireConnection;251;0;259;0
WireConnection;251;1;252;0
WireConnection;249;0;247;0
WireConnection;249;2;248;0
WireConnection;229;0;244;0
WireConnection;229;1;245;0
WireConnection;239;0;240;0
WireConnection;239;1;238;0
WireConnection;280;0;269;0
WireConnection;280;1;279;0
WireConnection;376;1;229;0
WireConnection;376;0;377;0
WireConnection;232;0;239;0
WireConnection;265;0;251;0
WireConnection;265;1;266;0
WireConnection;226;0;225;0
WireConnection;226;1;249;0
WireConnection;272;0;273;0
WireConnection;272;2;274;0
WireConnection;282;0;280;0
WireConnection;282;1;281;0
WireConnection;230;0;376;0
WireConnection;230;1;232;0
WireConnection;233;0;226;0
WireConnection;233;1;265;0
WireConnection;264;0;233;0
WireConnection;264;1;230;0
WireConnection;264;2;233;0
WireConnection;264;3;230;0
WireConnection;275;0;272;0
WireConnection;275;1;282;0
WireConnection;415;0;451;0
WireConnection;393;0;367;0
WireConnection;393;1;392;0
WireConnection;398;0;393;0
WireConnection;398;1;469;0
WireConnection;283;0;264;0
WireConnection;283;1;275;0
WireConnection;283;2;264;0
WireConnection;283;3;275;0
WireConnection;444;0;435;0
WireConnection;430;1;415;0
WireConnection;438;0;437;1
WireConnection;438;1;439;0
WireConnection;320;0;239;0
WireConnection;320;1;321;0
WireConnection;474;0;473;0
WireConnection;474;1;471;0
WireConnection;471;1;472;0
WireConnection;435;0;436;0
WireConnection;435;2;439;0
WireConnection;433;1;447;0
WireConnection;443;0;444;0
WireConnection;443;1;436;2
WireConnection;447;0;443;0
WireConnection;447;1;449;0
WireConnection;451;0;433;0
WireConnection;451;1;450;0
WireConnection;364;0;366;0
WireConnection;364;1;363;0
WireConnection;358;0;357;0
WireConnection;358;1;356;0
WireConnection;356;0;355;0
WireConnection;356;1;354;0
WireConnection;363;0;362;0
WireConnection;363;1;361;0
WireConnection;359;0;358;0
WireConnection;354;0;338;0
WireConnection;354;1;353;0
WireConnection;405;0;283;0
WireConnection;405;1;398;0
WireConnection;405;2;283;0
WireConnection;405;3;398;0
WireConnection;373;0;359;0
WireConnection;373;1;365;0
WireConnection;340;0;365;0
WireConnection;340;1;359;0
WireConnection;469;0;404;0
WireConnection;469;1;470;0
WireConnection;375;1;340;0
WireConnection;375;0;373;0
WireConnection;404;0;399;0
WireConnection;404;1;402;0
WireConnection;367;0;375;0
WireConnection;367;1;336;0
WireConnection;365;0;364;0
WireConnection;361;0;339;0
WireConnection;361;1;360;0
WireConnection;0;0;322;0
WireConnection;0;2;326;0
WireConnection;0;10;283;0
ASEEND*/
//CHKSM=5D16AC33589C34164C6489EB0C5040BC62ACD40C