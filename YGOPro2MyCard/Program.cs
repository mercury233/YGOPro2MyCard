using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
                    string cmdFile = Path.Combine(path, "commamd.shell");
                    /*string cmd = args[0];
                    string[] cmds;
                    switch (cmd){
                        case "-j":
                            ConfParser pro1Config = new ConfParser();
                            pro1Config.Load("system.conf");
                            string host = pro1Config.GetStringValue("lasthost");
                            string port = pro1Config.GetStringValue("lastport");
                            string roompass = pro1Config.GetStringValue("roompass");
                            string nickname = pro1Config.GetStringValue("nickname").Replace(" ","　");

                            cmds = new []{ "online", nickname, host, port, "0x233", roompass };

                            File.WriteAllText(cmdFile, String.Join(" ", cmds));
                            break;

                        case "-d":
                            if (args.Length == 2)
                            {
                                cmds = new[] { "edit", args[1] };

                                File.WriteAllText(cmdFile, String.Join(" ", cmds));
                            }
                            break;

                        default:
                            break;
                    }*/
                    string nick = null;
                    string host = null;
                    string port = null;
                    string password = null;
                    string deck = null;
                    string replay = null;
                    string puzzle = null;
                    bool join = false;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].ToLower() == "-n" && args.Length > i + 1)
                        {
                            nick = args[++i];
                        }
                        if (args[i].ToLower() == "-h" && args.Length > i + 1)
                        {
                            host = args[++i];
                        }
                        if (args[i].ToLower() == "-p" && args.Length > i + 1)
                        {
                            port = args[++i];
                        }
                        if (args[i].ToLower() == "-w" && args.Length > i + 1)
                        {
                            password = args[++i];
                        }
                        if (args[i].ToLower() == "-d" && args.Length > i + 1)
                        {
                            deck = args[++i];
                        }
                        if (args[i].ToLower() == "-r" && args.Length > i + 1)
                        {
                            replay = args[++i];
                        }
                        if (args[i].ToLower() == "-s" && args.Length > i + 1)
                        {
                            puzzle = args[++i];
                        }
                        if (args[i].ToLower() == "-j")
                        {
                            join = true;
                        }
                    }
                    if (join)
                    {
                        File.WriteAllText(cmdFile, "online " + nick + " " + host + " " + port + " 0x233 " + password, Encoding.UTF8);
                    }
                    else if (deck != null)
                    {
                        File.WriteAllText(cmdFile, "edit " + deck, Encoding.UTF8);
                    }
                    else if (replay != null)
                    {
                        File.WriteAllText(cmdFile, "replay " + replay, Encoding.UTF8);
                    }
                    else if (puzzle != null)
                    {
                        File.WriteAllText(cmdFile, "puzzle " + puzzle, Encoding.UTF8);
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
