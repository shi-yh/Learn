
Shader "Graph/Point Surface GPU"{

	Properties{
		
		_Smoothness("Sommthness",Range(0,1))=0.5
		
	}


	///子着色器定义
	SubShader{
		
		///��ʼcg����
		CGPROGRAM

		///下面几个是啥意思看不太懂，应该是声明了ConfigureSurface和ConfigureProcedural函数
		#pragma surface ConfigureSurface Standard fullforwardshadows addshadow
		
		#pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural
		///表明我们至少需要OpenGL ES 3.1的功能
		#pragma target 4.5
		
		#include "PointGPU.hlsl"


		float _Smoothness;

		struct Input{
			float3 worldPos;
		};

		void ConfigureSurface(Input input,inout SurfaceOutputStandard surface){
			
			//surface.
			//surface.Albedo= Input.worldPos;
			surface.Smoothness=_Smoothness;
			surface.Albedo.rg=input.worldPos.xy*0.5+0.5;
		}

		
		ENDCG
	}

	///���׼����������ɫ������һ����
	FallBack "DIFFUSE"

}