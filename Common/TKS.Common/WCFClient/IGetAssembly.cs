﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TKS.Common
{

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IGetAssembly")]
    public interface IGetAssembly
    {

       
       // [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/IGetAssembly/GetAssembliesDic", ReplyAction = "http://tempuri.org/IGetAssembly/GetAssembliesDicResponse")]
       // System.IAsyncResult BeginGetAssembliesDic(System.AsyncCallback callback, object asyncState);

       //DllVersion[] EndGetAssembliesDic(System.IAsyncResult result);

       
       // [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/IGetAssembly/GetAssemblyStream", ReplyAction = "http://tempuri.org/IGetAssembly/GetAssemblyStreamResponse")]
       // System.IAsyncResult BeginGetAssemblyStream(DownloadRequest request, System.AsyncCallback callback, object asyncState);

       // RemoteFileInfo EndGetAssemblyStream(System.IAsyncResult result);

       // [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/IGetAssembly/GetFlow", ReplyAction = "http://tempuri.org/IGetAssembly/GetFlowResponse")]
       // System.IAsyncResult BeginGetFlow(System.AsyncCallback callback, object asyncState);

       // Flow[] EndGetFlow(System.IAsyncResult result);

        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/IGetAssembly/GetConfig", ReplyAction = "http://tempuri.org/IGetAssembly/GetConfigResponse")]
        System.IAsyncResult BeginGetConfig(string filePath, string outcompcode, string incompcode, string GQcode, System.AsyncCallback callback, object asyncState);

        string EndGetConfig(System.IAsyncResult result);
    }

  
}
