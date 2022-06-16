using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Nimrod
{
    class Program
    {




        public static api NimrodApp = new api(
            name: "",
            ownerid: "",
            secret: "",
            version: "1.0"
        );

        static void Main(string[] args)
        {

            Console.Title = "Loader";
            Console.WriteLine("\n\n Connecting..");
            NimrodApp.init();

            autoUpdate();

            if (!NimrodApp.response.success)
            {
                Console.WriteLine("\n Status: " + NimrodApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            // app data
            Console.WriteLine("\n App data:");
            Console.WriteLine(" Number of users: " + NimrodApp.app_data.numUsers);
            Console.WriteLine(" Number of online users: " + NimrodApp.app_data.numOnlineUsers);
            Console.WriteLine(" Number of keys: " + NimrodApp.app_data.numKeys);
            Console.WriteLine(" Application Version: " + NimrodApp.app_data.version);
            Console.WriteLine(" Customer panel link: " + NimrodApp.app_data.customerPanelLink);
            NimrodApp.check();
            Console.WriteLine($" Current Session Validation Status: {NimrodApp.response.message}"); // you can also just check the status but ill just print the message

            Console.WriteLine("\n [1] Login\n [2] Register\n [3] Upgrade\n [4] License key only\n\n Choose option: ");

            string username;
            string password;
            string key;

            int option = int.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                    Console.WriteLine("\n\n Enter username: ");
                    username = Console.ReadLine();
                    Console.WriteLine("\n\n Enter password: ");
                    password = Console.ReadLine();
                    NimrodApp.login(username, password);
                    break;
                case 2:
                    Console.WriteLine("\n\n Enter username: ");
                    username = Console.ReadLine();
                    Console.WriteLine("\n\n Enter password: ");
                    password = Console.ReadLine();
                    Console.WriteLine("\n\n Enter license: ");
                    key = Console.ReadLine();
                    NimrodApp.register(username, password, key);
                    break;
                case 3:
                    Console.WriteLine("\n\n Enter username: ");
                    username = Console.ReadLine();
                    Console.WriteLine("\n\n Enter license: ");
                    key = Console.ReadLine();
                    NimrodApp.upgrade(username, key);
                    break;
                case 4:
                    Console.WriteLine("\n\n Enter license: ");
                    key = Console.ReadLine();
                    NimrodApp.license(key);
                    break;
                default:
                    Console.WriteLine("\n\n Invalid Selection");
                    Thread.Sleep(1500);
                    Environment.Exit(0);
                    break; // no point in this other than to not get error from IDE
            }

            if (!NimrodApp.response.success)
            {
                Console.WriteLine("\n Status: " + NimrodApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }

            Console.WriteLine("\n Logged In!"); // at this point, the client has been authenticated. Put the code you want to run after here

            // user data
            Console.WriteLine("\n User data:");
            Console.WriteLine(" Username: " + NimrodApp.user_data.username);
            Console.WriteLine(" IP address: " + NimrodApp.user_data.ip);
            Console.WriteLine(" Hardware-Id: " + NimrodApp.user_data.hwid);
            Console.WriteLine(" Created at: " + UnixTimeToDateTime(long.Parse(NimrodApp.user_data.createdate)));
            if (!String.IsNullOrEmpty(NimrodApp.user_data.lastlogin)) // don't show last login on register since there is no last login at that point
                Console.WriteLine(" Last login at: " + UnixTimeToDateTime(long.Parse(NimrodApp.user_data.lastlogin)));
            Console.WriteLine(" Your subscription(s):");
            for (var i = 0; i < NimrodApp.user_data.subscriptions.Count; i++)
            {
                Console.WriteLine(" Subscription name: " + NimrodApp.user_data.subscriptions[i].subscription + " - Expires at: " + UnixTimeToDateTime(long.Parse(NimrodApp.user_data.subscriptions[i].expiry)) + " - Time left in seconds: " + NimrodApp.user_data.subscriptions[i].timeleft);
            }

            /*
                  NimrodApp.web_login();

                  Console.WriteLine("\n Waiting for button to be clicked");
                  NimrodApp.button("close");
            */
            
            #region extras
            /*
            // set user variable 'discord' to 'test#0001' (if the user variable with name 'discord' doesn't exist, it'll be created)
            NimrodApp.setvar("discord", "test#0001");
            if (!NimrodApp.response.success)
            {
                Console.WriteLine("\n Status: " + NimrodApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            else
                Console.WriteLine("\n Successfully set user variable");
            */

            /*
            // display the user variable 'discord'
            string uservar = NimrodApp.getvar("discord");
            if (!NimrodApp.response.success)
            {
                Console.WriteLine("\n Status: " + NimrodApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            else
                Console.WriteLine("\n User variable value: " + uservar);
            */

            // NimrodApp.log("user logged in"); // log text to website and discord webhook (if set)

            /*

            // example to send normal request with no POST data
            string resp = NimrodApp.webhook("7kR0UedlVI", "&type=black&ip=1.1.1.1&hwid=abc");

            // example to send form data
            resp = NimrodApp.webhook("7kR0UedlVI", "", "type=init&name=test&ownerid=j9Gj0FTemM", "application/x-www-form-urlencoded");

            // example to send JSON
            resp = NimrodApp.webhook("aM0MA1Ipqz", "", "{\"content\": \"webhook message here\",\"embeds\": null}", "application/json"); // if Discord webhook message successful, response will be empty

            if (!NimrodApp.response.success)
            {
                Console.WriteLine("\n Status: " + NimrodApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            else
                Console.WriteLine("\n Response recieved from webhook request: " + resp);
            */

            /*
            // downloads application file to current folder Loader is running, feel free to change to whatever.
            byte[] result = NimrodApp.download("385624");
            if (!NimrodApp.response.success)
            {
                Console.WriteLine("\n Status: " + NimrodApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            else
                File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\test.txt", result);
            */

            /*
            string appvar = NimrodApp.var("test");
            if (!NimrodApp.response.success)
            {
                Console.WriteLine("\n Status: " + NimrodApp.response.message);
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            else
                Console.WriteLine("\n App variable data: " + appvar);
            */

            // NimrodApp.ban(); // ban the current user, must be logged in
            #endregion extras
            NimrodApp.check();
            Console.WriteLine($"Current Session Validation Status: {NimrodApp.response.message}"); // you can also just check the status but ill just print the message
            Console.WriteLine("\n Closing in ten seconds...");
            Thread.Sleep(10000);
            Environment.Exit(0);
        }

        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
            return dtDateTime;
        }

        static void autoUpdate()
        {
            if (NimrodApp.response.message == "invalidver")
            {
                if (!string.IsNullOrEmpty(NimrodApp.app_data.downloadLink))
                {
                    Console.WriteLine("\n Auto update avaliable!");
                    Console.WriteLine(" Choose how you'd like to auto update:");
                    Console.WriteLine(" [1] Open file in browser");
                    Console.WriteLine(" [2] Download file directly");
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            Process.Start(NimrodApp.app_data.downloadLink);
                            Environment.Exit(0);
                            break;
                        case 2:
                            Console.WriteLine(" Downloading file directly..");
                            Console.WriteLine(" New file will be opened shortly..");

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
                            Console.WriteLine(" Invalid selection, terminating program..");
                            Thread.Sleep(1500);
                            Environment.Exit(0);
                            break;
                    }
                }
                Console.WriteLine("\n Status: Version of this program does not match the one online. Furthermore, the download link online isn't set. You will need to manually obtain the download link from the developer.");
                Thread.Sleep(2500);
                Environment.Exit(0);
            }
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
    }
}
