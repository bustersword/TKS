using TKS.Controls.Common;
using System;
using System.IO;
using System.Windows;
 
using System.Windows.Controls;

namespace TKS.Controls
{
    public class UploadEventArgs : EventArgs
    {
        public UploadEventArgs(string url)
        {
            this.Url = url;
        }

        public string Url { get; set; }
    }

    public class Uploader : Control
    {
        public Uploader()
        {
            this.DefaultStyleKey = typeof(Uploader);
        }

        public event EventHandler<UploadEventArgs> UploadCompleted;

        public int ImageSize
        {
            get { return (int)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSizeProperty =
            DependencyProperty.Register("ImageSize", typeof(int), typeof(Uploader), 
            new PropertyMetadata(1000));

        public bool ResizeImage
        {
            get { return (bool)GetValue(ResizeImageProperty); }
            set { SetValue(ResizeImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ResizeImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResizeImageProperty =
            DependencyProperty.Register("ResizeImage", typeof(bool), typeof(Uploader), new PropertyMetadata(false));

        public Uri UploadUrl
        {
            get { return (Uri)GetValue(UploadUrlProperty); }
            set { SetValue(UploadUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UploadUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UploadUrlProperty =
            DependencyProperty.Register("UploadUrl", typeof(Uri), typeof(Uploader),
            new PropertyMetadata(OnUploadUrlPropertyChanged));

        private static void OnUploadUrlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Uploader up = d as Uploader;
            if (up != null)
            {
                up.UploadUrl = e.NewValue as Uri;
            }
        }

        public long UploadChunkSize
        {
            get { return (long)GetValue(UploadChunkSizeProperty); }
            set { SetValue(UploadChunkSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UploadChunkSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UploadChunkSizeProperty =
            DependencyProperty.Register("UploadChunkSize", typeof(long), typeof(Uploader),
            new PropertyMetadata((long)4194304));

        public long MaximumUpload
        {
            get { return (long)GetValue(MaximumUploadProperty); }
            set { SetValue(MaximumUploadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaximumUpload.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumUploadProperty =
            DependencyProperty.Register("MaximumUpload", typeof(long), typeof(Uploader),
            new PropertyMetadata((long)-1));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Filter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(Uploader), 
            new PropertyMetadata("All Files|*.*"));

        public string AddText
        {
            get { return (string)GetValue(AddTextProperty); }
            set { SetValue(AddTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddTextProperty =
            DependencyProperty.Register("AddText", typeof(string), typeof(Uploader),
            new PropertyMetadata("add file",OnAddTextPropertyChanged));

        private static void OnAddTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Uploader upload = d as Uploader;
            if (upload != null)
            {
                upload.AddText = e.NewValue.ToString ();
            }
        }

        public string RemoveText
        {
            get { return (string)GetValue(RemoveTextProperty); }
            set { SetValue(RemoveTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveTextProperty =
            DependencyProperty.Register("RemoveText", typeof(string), typeof(Uploader), 
            new PropertyMetadata("cancel",OnRemoveTextPropertyChanged));

        private static void OnRemoveTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Uploader upload = d as Uploader;
            if (upload != null)
            {
                upload.RemoveText = e.NewValue.ToString();
            }
        }

        public string ReturnUrl
        {
            get { return (string)GetValue(ReturnUrlProperty); }
            set { SetValue(ReturnUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReturnUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReturnUrlProperty =
            DependencyProperty.Register("ReturnUrl", typeof(string), typeof(Uploader),
            new PropertyMetadata(string.Empty));

        private HyperlinkButton addFile;
        private HyperlinkButton remove;

     

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            addFile = this.GetTemplateChild("addFile") as HyperlinkButton;
            addFile.Click += new RoutedEventHandler(addFile_Click);

            remove = this.GetTemplateChild("remove") as HyperlinkButton;
            remove.Click += new RoutedEventHandler(remove_Click);
        }

        void remove_Click(object sender, RoutedEventArgs e)
        {
            FileUpload fu = DataContext as FileUpload;
            if (fu != null)
                fu.RemoveUpload();
        }

        void addFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Filter;
            dlg.Multiselect = false;

            if ((bool)dlg.ShowDialog())
            {
                this.Focus();

                foreach (FileInfo file in dlg.Files)
                {
                    FileUpload upload = new FileUpload(this.Dispatcher, UploadUrl, file);
                    if (UploadChunkSize > 0)
                        upload.ChunkSize = UploadChunkSize;
                    if (ResizeImage)
                    {
                        upload.ResizeImage = ResizeImage;
                        upload.ImageSize = ImageSize;
                    }


                    if (MaximumUpload >= 0 && upload.FileLength > MaximumUpload)
                    {
                        MessageBox.Show(string.Format("文件 '{0}' 超过最大的上传大小.", upload.Name));
                        continue;
                    }

                    //upload.StatusChanged += new EventHandler(upload_StatusChanged);
                    //upload.UploadProgressChanged += new ProgressChangedEvent(upload_UploadProgressChanged);
                    upload.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(upload_PropertyChanged);

                    this.DataContext = upload;

                    upload.Upload();
                }
            }
        }

        void upload_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var fu = sender as FileUpload;

            if (e.PropertyName == "Status")
            {
                if (fu.Status == FileUploadStatus.Complete)
                {
                    if (UploadCompleted != null)
                        UploadCompleted(this, new UploadEventArgs(fu.RetureUrl));

                    this.ReturnUrl = fu.RetureUrl;
                }


                switch (fu.Status)
                {
                    case FileUploadStatus.Pending:
                        VisualStateManager.GoToState(this, "Pending", true);
                        break;
                    case FileUploadStatus.Uploading:
                        VisualStateManager.GoToState(this, "Uploading", true);
                        break;
                    case FileUploadStatus.Complete:
                        VisualStateManager.GoToState(this, "Complete", true);
                        break;
                    case FileUploadStatus.Error:
                        VisualStateManager.GoToState(this, "Error", true);
                        break;
                    case FileUploadStatus.Canceled:
                        VisualStateManager.GoToState(this, "Pending", true);
                        break;
                    case FileUploadStatus.Removed:
                        VisualStateManager.GoToState(this, "Pending", true);
                        break;
                    case FileUploadStatus.Resizing:
                        VisualStateManager.GoToState(this, "Resizing", true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
