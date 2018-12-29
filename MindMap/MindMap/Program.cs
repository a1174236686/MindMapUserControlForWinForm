using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMap
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                rightConmandKey(args);  //文件夹添加右键菜单
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static string MindMapRightMenu= "用思维导图打开(&Z)";
        //public static string ParamePath = @"C:\Users\wayne\Desktop";
        //public static string ParamePath = @"C:\Users\wayne";
        public static string ParamePath = @"d:\";

        private static void rightConmandKey(string[] args)
        {

            //删除注册表

            //return;

            if (args.Length <= 0)
            {
                string str = System.Environment.CurrentDirectory + "\\MindMap.exe" + " %1";//当前exe文件夹路径
                AddFileContextMenuItem(MindMapRightMenu, str, 2);
                str = System.Environment.CurrentDirectory + "\\MindMap.exe" + " %V";//当前exe文件夹路径
                AddFileContextMenuItem(MindMapRightMenu, str, 1);
                Recod(@"C:\Menu.txt", "注册右键菜单");
            }
            else
            {
                if (args.Length > 0 && args[0] != "")
                {
                    ParamePath = args[0].ToString();
                    Recod(@"C:\Menu.txt", string.Join("-", args));
                }
                else
                {
                    Recod(@"C:\Menu.txt", "args参数没有数据");
                }
            }
        }
        /// <summary>
        /// 注册右键菜单
        /// </summary>
        /// <param name="itemName">右键菜单名称</param>
        /// <param name="assoCreatedProgramFullPath">程序所在路径</param>
        private static void AddFileContextMenuItem(string itemName, string assoCreatedProgramFullPath,int RightTarget)
        {
            try
            {
                RegistryKey shell = null;
                switch (RightTarget)
                {
                    case 1:
                        //注册到所有目录
                        shell = Registry.ClassesRoot.OpenSubKey(@"directory\Background\shell", true);
                        break;
                    case 2:
                        //注册到文件夹
                        shell = Registry.ClassesRoot.OpenSubKey("directory", true).OpenSubKey("shell", true);
                        break;

                }
                //注册到所有文件
                //RegistryKey shell = Registry.ClassesRoot.OpenSubKey(@"*\shellQ", true);



                if (shell == null)
                {
                    shell = Registry.ClassesRoot.CreateSubKey(@"*\shell");
                }
                RegistryKey rightCommondKey = shell.OpenSubKey(itemName);
                if (rightCommondKey == null)
                {
                    rightCommondKey = shell.CreateSubKey(itemName);
                }
                RegistryKey assoCreatedProgramKey = rightCommondKey.CreateSubKey("command");
                assoCreatedProgramKey.SetValue(string.Empty, assoCreatedProgramFullPath);
                assoCreatedProgramKey.Close();
                rightCommondKey.Close();
                shell.Close();
            }
            catch (Exception ex)
            {
                Recod(@"E:\Menu.txt", "注册右键菜单异常");
            }
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="noteName"></param>
        /// <param name="content"></param>
        private static void Recod(string noteName, string content)
        {
            try
            {
                FileStream fs = new FileStream(@noteName, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                sw.WriteLine(content);
                sw.Close();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="itemName"></param>
        private static void DeleteContextMenu(string itemName)
        {
            try
            {

                RegistryKey shellKey = Registry.ClassesRoot.OpenSubKey(@"directory\Background\shell", true);
                RegistryKey rightCommondKey = shellKey.OpenSubKey(itemName, true);
                if (rightCommondKey != null)
                {
                    rightCommondKey.DeleteSubKeyTree("");
                }
            }
            catch (Exception ex)
            {

                // throw;
            }
        }


    }
}
