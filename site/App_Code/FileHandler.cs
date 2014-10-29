using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

public class FileHandler
{
    public string Move(string sourceFile)
    {
        FileInfo file = new FileInfo(sourceFile);

        string destFolder = string.Empty;

        DateTime dataTime = DateTime.Now;
        string dataFolder = dataTime.Year.ToString() + Text.AddZero(dataTime.Month) + Text.AddZero(dataTime.Day);

        bool isImageFile = file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".png";
        bool isMusicFile = file.Extension.ToLower() == ".mp3" || file.Extension.ToLower() == ".wma";

        if (isImageFile)
            destFolder = Path.Combine(rootPath, "Files", "UploadImages", dataFolder);
        else if (isMusicFile)
            destFolder = Path.Combine(rootPath, "Files", "UploadMusics", dataFolder);
        else
            destFolder = Path.Combine(rootPath, "Files", "UploadFiles", dataFolder);

        if (!Directory.Exists(destFolder))
            Directory.CreateDirectory(destFolder);

        string destFile = Path.Combine(destFolder, file.Name);

        File.Copy(sourceFile, destFile, true);

        if (isImageFile)
        {
            MakeThumbnail(destFile);
        }

        return destFile.Remove(0, rootPath.Length).Replace('\\', '/');
    }

    protected void MakeThumbnail(string imageFile)
    {
        //FileInfo file = new FileInfo(imageFile);

        //string thumbFolder = Path.Combine(file.DirectoryName, "Thumb");
        //if (!Directory.Exists(thumbFolder))
        //    Directory.CreateDirectory(thumbFolder);

        //string mThumbFileName = Path.Combine(thumbFolder, @"m_" + file.Name);
        //string sThumbFileName = Path.Combine(thumbFolder, @"s_" + file.Name);

        //ImageUtil.MakeThumbnail(imageFile, mThumbFileName, 100, 100, "Cut", false);
        //ImageUtil.MakeThumbnail(imageFile, sThumbFileName, 50, 50, "Cut", false);
    }

    public readonly string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
}

public class Text
{
    public static string AddZero(int i)
    {
        return (((i > 9) ? "" : "0") + i);
    }
}
