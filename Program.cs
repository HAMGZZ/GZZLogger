using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Newtonsoft.Json;
using CsvHelper.TypeConversion;
using Terminal.Gui;

namespace GZZLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(130, 33);
            Console.WriteLine("GZZ Logger 2020");
            Console.WriteLine("Loading...");
            var SessionSettings = new Settings();                                       //Load Settings from file
            

            if (SessionSettings.Callsign == null)
            {
                Console.Write("Enter your callsign> ");
                var tmp_call = Console.ReadLine();
                SessionSettings.Callsign = tmp_call;
            }

            if (SessionSettings.ItuZone == 0)
            {
                Console.Write("Enter your ITU zone> ");
                int tmp_itu_zone = Convert.ToInt32(Console.ReadLine());
                SessionSettings.ItuZone = tmp_itu_zone;
            }

            if (SessionSettings.CqZone == 0)
            {
                Console.Write("Enter your CQ zone> ");
                int tmp_cq_zone = Convert.ToInt32(Console.ReadLine());
                SessionSettings.CqZone = tmp_cq_zone;
            }

            if(SessionSettings.CurrentDatabaseName == "")
            {
                Console.Write("Enter new database name (with ext)> ");
                var tmp_name = Console.ReadLine();
                SessionSettings.CurrentDatabaseName = tmp_name;
                SessionSettings.PreviousDatabaseName = tmp_name;
            }


            var SessionDatabase = new Database(SessionSettings.CurrentDatabaseName);    //Load DB from DB name in settings
            var LocationRecords = new CallsignLocationLookup();                         //Load location DB for use with callsign prefixes



            var MainGui = new GUI(SessionDatabase, SessionSettings, LocationRecords);
            Application.Init();
            var top = MainGui.MainUITopLevel();
            Application.Run(top);


        }


    }
}



// Dictonairy locationlookup, tryget