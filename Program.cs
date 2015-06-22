using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Updater
{
    class Program
    {
        public const string ClientVersion = "1.0";

        static void Main(string[] args)
        {
            CheckForUpdates();

            Console.WriteLine("Update completed.");
            Console.ReadLine();

        }

        public static void CheckForUpdates()
        {
            System.Net.WebClient client = new System.Net.WebClient(); // <-- your web client object.

            string serverVersion = client.DownloadString(new Uri("https://raw.githubusercontent.com/lsdclown/UpdaterEx/master/latestversion.ver"));

            if (ClientVersion == serverVersion)
            {
                Console.WriteLine("No updated needed.");
                return; // versions match, so get the heck outta here.
            }

            bool updateApplied = false;

            while (!updateApplied)
            {
                client.DownloadFile(new Uri("https://raw.githubusercontent.com/lsdclown/UpdaterEx/master/update/" + "UpdateMe.exe"), "UpdateMe.exe.new");

                if (File.Exists("UpdateMe.exe.new"))
                {
                    Console.WriteLine("got updated file.. swap out.");
                    var replaced = ReplaceWithNewVersion();
                    if (replaced)
                    {
                        Console.WriteLine("Replaced successfully.");
                        updateApplied = true;
                        continue;
                    }

                    Console.WriteLine("Update failed could not replace the old file.. trying again.");
                }
              
            }
        }

        public static bool ReplaceWithNewVersion()
        {
            if (File.Exists("UpdateMe.exe"))
            {
                try
                {
                    // Take the old version and rename it to UpdateMe.exe.old or something like that.
                    File.Move("UpdateMe.exe", "UpdateMe.exe.old");

                    // Take the new version which you downloaded and rename it to UpdateMe.exe
                    File.Move("UpdateMe.exe.new", "UpdateMe.exe");
                    return true;
                }
                catch (IOException exception)
                {
                    Console.WriteLine("Something went wrong... here is what I know..\n" + exception.Message);
                    return false;
                }
                finally
                {
                    if (File.Exists("UpdateMe.exe.old") && File.Exists("UpdateMe.exe"))
                    {
                        // do whatever you want with the old versions, probably just delete them though.
                        File.Delete("UpdateMe.exe.old");
                    }
                }
            }
            Console.WriteLine("File doesn't even exist, what am I updating? Redownload the program.");
            return false;
        }
    }
}
