TKS.Common    �������� 
TKS.Controls  �ؼ� 
TKS.Interface �ӿ�
TKS.Service   ����
TKS.DTO       �������
TKS.Core      ��ܸ������Ŀ�


Tools �ļ�����һ��  FCOPY.bat �ļ�������������Ŀ��DLL��ָ���ļ��е�

���÷�����
		    ****Ҫ���Ƶ���ĿDLL
			SET COPY_FROM="$(TargetDir)*.dll"  

			****Ŀ���ļ��У��˴�Ϊ����DLL�����ļ���ַ
			SET COPY_TO=$(SolutionDir)Tools\CreateDllXML\bin\Debug\dll\mainpage2013225.dll 

			****����
			CALL $(SolutionDir)Tools\fcopy.bat   

������ķ����������ճ����  ���������¼���������  ������Ŀ����ɹ����Զ����ã�