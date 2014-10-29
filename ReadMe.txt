TKS.Common    公共方法 
TKS.Controls  控件 
TKS.Interface 接口
TKS.Service   服务
TKS.DTO       传输对象
TKS.Core      框架辅助核心库


Tools 文件夹有一个  FCOPY.bat 文件是用来拷贝项目的DLL到指定文件夹的

调用方法：
		    ****要复制的项目DLL
			SET COPY_FROM="$(TargetDir)*.dll"  

			****目标文件夹，此处为生成DLL配置文件地址
			SET COPY_TO=$(SolutionDir)Tools\CreateDllXML\bin\Debug\dll\mainpage2013225.dll 

			****调用
			CALL $(SolutionDir)Tools\fcopy.bat   

将上面的方法设置完后，粘贴到  后期生成事件命令行里  ，当项目编译成功会自动调用，