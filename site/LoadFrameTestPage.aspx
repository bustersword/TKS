<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LoadFrame</title>
    <style type="text/css">
        html, body
        {
            height: 100%;
            overflow: auto;
        }

        body
        {
            padding: 0;
            margin: 0;
        }

        #silverlightControlHost
        {
            height: 100%;
            text-align: center;
        }
    </style>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript" src="splash/SplashScreen.js"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Silverlight 应用程序中未处理的错误 " + appSource + "\n";

            errMsg += "代码: " + iErrorCode + "    \n";
            errMsg += "类别: " + errorType + "       \n";
            errMsg += "消息: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "文件: " + args.xamlFile + "     \n";
                errMsg += "行: " + args.lineNumber + "     \n";
                errMsg += "位置: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "行: " + args.lineNumber + "     \n";
                    errMsg += "位置: " + args.charPosition + "     \n";
                }
                errMsg += "方法名称: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }

      

    </script>
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:HiddenField ID="HiddenField3" runat="server" />
        <asp:HiddenField ID="HiddenField4" runat="server" />
        <asp:HiddenField ID="HiddenField5" runat="server" />
        <% 
            try
            {
                HiddenField2.Value = ConfigurationManager.AppSettings["functionid"].ToString();
                HiddenField3.Value = ConfigurationManager.AppSettings["entid"].ToString();
                HiddenField4.Value = ConfigurationManager.AppSettings["dd_function"].ToString();
                HiddenField5.Value = ConfigurationManager.AppSettings["incomfunctionid"].ToString();
            }
            catch
            {
                HiddenField2.Value = "";
                HiddenField3.Value = "";
                HiddenField4.Value = "";
                HiddenField5.Value = "";
            }
            AppDomain ap = AppDomain.CurrentDomain;
            System.IO.FileInfo fi = new System.IO.FileInfo(ap.BaseDirectory + "\\web.config");
            if (fi.Exists)
            {
                System.IO.StreamReader sr = fi.OpenText();
                if (sr != null)
                {
                    string xml = sr.ReadToEnd();

                    System.Xml.Linq.XDocument xdoc = System.Xml.Linq.XDocument.Load(new System.IO.StringReader(xml));
                    if (xdoc != null)
                    {
                        System.Xml.Linq.XElement xServiceModle = xdoc.Root.Element("system.serviceModel");
                        if (xServiceModle != null)
                        {
                            //return xServiceModle.ToString();
                            HiddenField1.Value = HttpUtility.HtmlEncode(xServiceModle.ToString());

                        }
                    }
                }
            }
            else
            {
                HiddenField1.Value = "";
            } 
        %>
        <div id="silverlightControlHost">
            <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
                <param name="source" value="ClientBin/LoadFrame.xap" />
                <param name="onError" value="onSilverlightError" />
                <param name="background" value="white" />
                <param name="minRuntimeVersion" value="5.0.61118.0" />
                <param name="autoUpgrade" value="true" />
                <param name="splashscreensource" value="splash/SplashScreen.xaml" />
                <param name="onSourceDownloadProgressChanged" value="onSourceDownloadProgressChanged" />
                <param name="initParams" value="config=<%=HiddenField1.Value%>,token=<%= Request["token"] %>,companycode=<%= Request["companycode"] %>,companyname=<%= Request["companyname"] %>,functionid=<%=HiddenField2.Value %>,entid=<%=HiddenField3.Value %>,dd_functionid=<%=HiddenField4.Value %>,incomfunctionid=<%=HiddenField5.Value %>,UploadPage=FileUpload.ashx,Filter=Images (*.jpg)|*.jpg" />

                <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.0.61118.0" style="text-decoration: none">
                    <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="获取 Microsoft Silverlight" style="border-style: none" />
                </a>
            </object>
            <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px; border: 0px"></iframe>
        </div>
    </form>
</body>
</html>
