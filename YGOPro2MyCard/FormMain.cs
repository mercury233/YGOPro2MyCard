using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace YGOPro2MyCard
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Program.YGOProDir = CheckMyCard();
            if (Program.YGOProDir == "")
            {
                btnSetup.Text = "请先在萌卡中安装 YGOPro";
                btnSetup.Enabled = false;
            }
            else
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(Program.YGOProDir, "ygopro.exe"));
                if(versionInfo.ProductName == "YGOPro2MyCard")
                {
                    btnSetup.Text = "重新安装";
                }
            }
        }

        public string CheckMyCard()
        {
            string adir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyCardLibrary\\ygopro");
            if (IsYGOProDir(adir))
            {
                return adir;
            }
            else
            {
                string[] disks = Directory.GetLogicalDrives();
                foreach (string disk in disks)
                {
                    string dir= Path.Combine(disk, "MyCardLibrary\\ygopro");
                    if (IsYGOProDir(dir))
                    {
                        return dir;
                    }
                }
            }
            return "";
        }

        private bool IsYGOProDir(string dir)
        {
            if (!File.Exists(Path.Combine(dir, "ygopro.exe"))) return false;
            if (!File.Exists(Path.Combine(dir, "system.conf"))) return false;
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            if (Program.YGOProDir == "")
                return;

            string confFile = Path.Combine(Program.YGOProDir, "ygopro2.conf");
            string exeFile = Path.Combine(Program.YGOProDir, "ygopro.exe");
            string bakFile = Path.Combine(Program.YGOProDir, "ygopro.bak");

            File.WriteAllText(confFile, "#");
            Program.Config.Load(confFile);
            Program.Config.SetStringValue("path", Application.StartupPath);

            File.Copy(exeFile, bakFile, overwrite: true);
            File.Copy(Application.ExecutablePath, exeFile, overwrite: true);

            btnSetup.Text = "已安装";
            btnSetup.Enabled = false;
            btnClose.Focus();
        }
    }
}
