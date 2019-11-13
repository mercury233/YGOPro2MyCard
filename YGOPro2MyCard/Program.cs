using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using YGO233;

namespace YGOPro2MyCard
{
    public class Program
    {
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, int uType);

        const int MB_ICONERROR = 0x00000010;

        static void ErrorMsgBox(string message)
        {
            MessageBox((IntPtr)0, message, "YGOPro2", MB_ICONERROR);
        }

        public static string YGOProDir = "";
        public static string YGOPro2Dir = "";
        public static ConfParser Config = new ConfParser();


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (File.Exists("ygopro2.conf"))
            {
                Config.Load("ygopro2.conf");
                string path = Config.GetStringValue("path");

                if (!Directory.Exists(path))
                {
                    ErrorMsgBox("启动 YGOPro2 失败，请重新安装。");
                    return;
                }

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = path;
                startInfo.FileName = "ygopro2.exe";

                if (args.Length > 0)
                {
                    string cmd = args[0];
                    switch (cmd){
                        case "-j":
                            ConfParser pro1Config = new ConfParser();
                            pro1Config.Load("system.conf");
                            string host = pro1Config.GetStringValue("lasthost");
                            string port = pro1Config.GetStringValue("lastport");
                            string roompass = pro1Config.GetStringValue("roompass");
                            string nickname = pro1Config.GetStringValue("nickname").Replace(" ","　");

                            string cmdFile = Path.Combine(path, "commamd.shell");

                            string[] cmds = { "online", nickname, host, port, "0x233", roompass };

                            File.WriteAllText(cmdFile, String.Join(" ", cmds));
                            break;
                        default:
                            break;
                    }
                }

                Process process = Process.Start(startInfo);
                process.WaitForExit();
            }
            else if (File.Exists("YGOPro2.exe"))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            else
            {
                ErrorMsgBox("请解压到 YGOPro2 文件夹使用。");
                return;
            }
        }
    }
}
