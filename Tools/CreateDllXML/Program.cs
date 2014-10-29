using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace CreateDllXML
{
    class Program
    {
        static void Main(string[] args)
        {
            string errormsg = "";
            string path = AppDomain.CurrentDomain.BaseDirectory;

            DirectoryInfo directory = new DirectoryInfo(path + @"dll\");
            DirectoryInfo[] dlls = directory.GetDirectories();
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //创建一个根节点（一级）
            try
            {
                XmlElement root = doc.CreateElement("dllroot");
                doc.AppendChild(root);

                foreach (DirectoryInfo die in dlls)
                {
                    XmlElement assembly = doc.CreateElement("assembly");
                    assembly.SetAttribute("name", die.Name);
                 

                    FileInfo[] files = die.GetFiles();
                    foreach (FileInfo dll in files)
                    {
                        if (dll.Extension.ToLower() != ".dll") continue;
                        errormsg = dll.FullName;
                        System.IO.FileStream stream = new System.IO.FileStream(dll.FullName ,
                          System.IO.FileMode.Open, System.IO.FileAccess.Read);

                        byte[] bs = new byte[stream.Length];
                        stream.Read(bs, 0, bs.Length);
                        System.Reflection.Assembly asm = System.Reflection.Assembly.Load(bs);

                        if (dll.Name == die.Name)
                        {
                            assembly.SetAttribute("version", asm.GetName().Version.ToString());
                            assembly.SetAttribute("size", dll.Length.ToString());
                        }
                        else
                        {
                            XmlElement reference = doc.CreateElement("reference");
                            reference.SetAttribute("name", dll.Name);
                            reference.SetAttribute("version", asm.GetName().Version.ToString());
                            reference.SetAttribute("size", dll.Length.ToString());

                            assembly.AppendChild(reference);
                        }
                        stream.Close();
                    }
                    root.AppendChild(assembly);
                  
                }

                doc.Save("DllConfig.xml");

                Console.Write(doc.OuterXml);
                Console.WriteLine("保存成功");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+errormsg);
            }
          
        }
    }
}
