using Nimrod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nimrod
{
    public partial class Login : Form
    {

        public static api NimrodApp = new api(
            name: "",
            ownerid: "",
            secret: "",
            version: "1.0"
        );

        public Login()
        {
            InitializeComponent();
        }

        private void siticoneControlBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            NimrodApp.init();

            if (NimrodApp.response.message == "invalidver")
            {
                if (!string.IsNullOrEmpty(NimrodApp.app_data.downloadLink))
                {
                    DialogResult dialogResult = MessageBox.Show("Yes to open file in browser\nNo to download file automatically", "Auto update", MessageBoxButtons.YesNo);
                    switch (dialogResult)
                    {
                        case DialogResult.Yes:
                            Process.Start(NimrodApp.app_data.downloadLink);
                            Environment.Exit(0);
                            break;
                        case DialogResult.No:
                            WebClient webClient = new WebClient();
                            string destFile = Application.ExecutablePath;

                            string rand = random_string();

                            destFile = destFile.Replace(".exe", $"-{rand}.exe");
                            webClient.DownloadFile(NimrodApp.app_data.downloadLink, destFile);

                            Process.Start(destFile);
                            Process.Start(new ProcessStartInfo()
                            {
                                Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + Application.ExecutablePath + "\"",
                                WindowStyle = ProcessWindowStyle.Hidden,
                                CreateNoWindow = true,
                                FileName = "cmd.exe"
                            });
                            Environment.Exit(0);

                            break;
                        default:
                            MessageBox.Show("Invalid option");
                            Environment.Exit(0);
                            break;
                    }
                }
                MessageBox.Show("Version of this program does not match the one online. Furthermore, the download link online isn't set. You will need to manually obtain the download link from the developer");
                Thread.Sleep(2500);
                Environment.Exit(0);
            }
            
            if (!NimrodApp.response.success)
            {
                MessageBox.Show(NimrodApp.response.message);
                Environment.Exit(0);
            }
            // if(NimrodApp.checkblack())
            // {
            //     MessageBox.Show("user is blacklisted");
            // }
            // else
            // {
            //     MessageBox.Show("user is not blacklisted");
            // }
            NimrodApp.check();
            siticoneLabel1.Text = $"Current Session Validation Status: {NimrodApp.response.success}";
        }

        static string random_string()
        {
            string str = null;

            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                str += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))).ToString();
            }
            return str;

        }

        private void UpgradeBtn_Click(object sender, EventArgs e)
        {
            NimrodApp.upgrade(username.Text, key.Text); 
            status.Text = "Status: " + NimrodApp.response.message;
           
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            NimrodApp.login(username.Text,password.Text);
            if (NimrodApp.response.success)
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
                status.Text = "Status: " + NimrodApp.response.message;
        }

        private void RgstrBtn_Click(object sender, EventArgs e)
        {
            NimrodApp.register(username.Text, password.Text, key.Text);
            if (NimrodApp.response.success)
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
                status.Text = "Status: " + NimrodApp.response.message;
        }

        private void LicBtn_Click(object sender, EventArgs e)
        {
            NimrodApp.license(key.Text);
            if (NimrodApp.response.success)
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
                status.Text = "Status: " + NimrodApp.response.message;
        }
    }
}
