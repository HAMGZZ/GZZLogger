using GZZLogger.GUI;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Net;
using Terminal.Gui;
using Console = System.Console;


namespace GZZLogger
{
    
    class Program
    {
        

        static void Main(string[] args)
        {
            Version GZZLoggerVersion = new Version(1, 0, 0, 0);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Console.SetWindowSize(130, 33);
            else
                Console.WriteLine("\033[8;33;130t");
            Console.WriteLine("GZZ Logger -- V " + GZZLoggerVersion.ToString());
            Console.WriteLine("Checking for updates...");
            checkUpdates(GZZLoggerVersion);
            Console.Write("Loading...");
            var SessionSettings = new Settings();                                       //Load Settings from file
            var LocationRecords = new CallsignLocationLookup();                         //Load location DB for use with callsign prefixes
            var startContestMode = SessionSettings.ContestMode;


            Application.Init();
            if (SessionSettings.Callsign == string.Empty || SessionSettings.CurrentDatabaseName == string.Empty)
            {
                var introGui = new GUI.introGui(SessionSettings, LocationRecords);
                Application.Run(introGui.introUITopLevel());
            }

            else
            {
                var SessionDatabase = new Database(SessionSettings.CurrentDatabaseName);    //Load DB from DB name in settings
                var MainGui = new GUI.GUIClass(SessionSettings, SessionDatabase, LocationRecords);
                var top = MainGui.MainUIToplevel();
                Application.Run(top);
            }

            while(startContestMode != SessionSettings.ContestMode) // REDRAW IF CHANGED MODE
            {
                startContestMode = SessionSettings.ContestMode;
                var SessionDatabase = new Database(SessionSettings.CurrentDatabaseName);
                var MainGui = new GUI.GUIClass(SessionSettings, SessionDatabase, LocationRecords);
                var top = MainGui.MainUIToplevel();
                Application.Run(top);
            }

            Console.WriteLine("Goodbye " + SessionSettings.Callsign);
            Thread.Sleep(1000);
        }

        static void checkUpdates(Version version)
        {
            var client = new WebClient();
            string url = "https://czgzz.space/gzzLoggerVersion.html";
            Console.Write("Avaible version = ...");
            string AvailVer = client.DownloadString(url);
            Console.WriteLine("\b\b\b" + AvailVer);
            if(version.ToString() != AvailVer)
            {
                Console.Write("New version available, would you like to update? (y/n): ");

            }
            
        }


        
    }
}



// Dictonairy locationlookup, tryget