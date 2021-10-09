

Shader "Graph/Point Surface"{

	Properties{
		
		_Smoothness("Sommthness",Range(0,1))=0.5
		
	}


	///����ɫ��
	SubShader{
		
		///��ʼcg����
		CGPROGRAM

		///��ɫ�����������ɾ��б�׼��������ȫ֧����Ӱ�ı�����ɫ��
		#pragma surface ConfigureSurface Standard fullforwardshadows 
		#pragma target 3.0

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