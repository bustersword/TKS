using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace TKS.Controls
{
    public partial class UploaderConTaniner : UserControl
    {
        public UploaderConTaniner()
        {
            InitializeComponent();

          

        }

        Uri _uploadUri;
        public Uri UploadUri
        {
            get { return _uploadUri ; }
            set
            {
                _uploadUri = value;
                if (_uploadUri  != null)
                {
                    uploader1.UploadUrl = _uploadUri;
                    uploader2.UploadUrl = _uploadUri;
                    uploader3.UploadUrl = _uploadUri;
                }
            }
        }
    }
}
