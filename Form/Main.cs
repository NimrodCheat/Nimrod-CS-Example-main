using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nimrod
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void siticoneControlBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        string chatchannel = "testing"; // chat channel name

        private void Main_Load(object sender, EventArgs e)
        {
            Login.NimrodApp.check();
            siticoneLabel1.Text = $"Current Session Validation Status: {Login.NimrodApp.response.success}";
            key.Text = "Username: " + Login.NimrodApp.user_data.username;
            expiry.Text = "Expiry: " + UnixTimeToDateTime(long.Parse(Login.NimrodApp.user_data.subscriptions[0].expiry));
            subscription.Text = "Subscription: " + Login.NimrodApp.user_data.subscriptions[0].subscription;
            ip.Text = "IP Address: " + Login.NimrodApp.user_data.ip;
            hwid.Text = "HWID: " + Login.NimrodApp.user_data.hwid;
            createDate.Text = "Creation date: " + UnixTimeToDateTime(long.Parse(Login.NimrodApp.user_data.createdate));
            lastLogin.Text = "Last login: " + UnixTimeToDateTime(long.Parse(Login.NimrodApp.user_data.lastlogin));
            subscriptionDaysLabel.Text = "Expiry in Days: "+ Login.NimrodApp.expirydaysleft();
            numUsers.Text = "Number of users: " + Login.NimrodApp.app_data.numUsers;
            numOnlineUsers.Text = "Number of online users: " + Login.NimrodApp.app_data.numOnlineUsers;
            numKeys.Text = "Number of licenses: " + Login.NimrodApp.app_data.numKeys;
            version.Text = "Current version: " + Login.NimrodApp.app_data.version;
            customerPanelLink.Text = "Customer panel: " + Login.NimrodApp.app_data.customerPanelLink;
        }

        public DateTime UnixTimeToDateTime(long unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
            return dtDateTime;
        }

        private void sendmsg_Click(object sender, EventArgs e)
        {
            if (Login.NimrodApp.chatsend(chatmsg.Text, chatchannel))
            {
                dataGridView1.Rows.Insert(0, Login.NimrodApp.user_data.username, chatmsg.Text, UnixTimeToDateTime(DateTimeOffset.Now.ToUnixTimeSeconds()));
            }
            else
                chatmsg.Text = "Status: " + Login.NimrodApp.response.message;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            timer1.Interval = 15000; // get chat messages every 15 seconds
            if (!String.IsNullOrEmpty(chatchannel))
            {
                var messages = Login.NimrodApp.chatget(chatchannel);
                if (messages == null || !messages.Any())
                {
                    dataGridView1.Rows.Insert(0, "Nimrod", "No Chat Messages", UnixTimeToDateTime(DateTimeOffset.Now.ToUnixTimeSeconds()));
                }
                else
                {
                    foreach (var message in messages)
                    {
                        dataGridView1.Rows.Insert(0, message.author, message.message, UnixTimeToDateTime(long.Parse(message.timestamp)));
                    }
                }
            }
            else
            {
                dataGridView1.Rows.Insert(0, "Nimrod", "No Chat Messages", UnixTimeToDateTime(DateTimeOffset.Now.ToUnixTimeSeconds()));
            }
        }
    }
}
