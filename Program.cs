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
            var SessionSettings = new Settings();                                       //Load Settings from file
            var SessionDatabase = new Database(SessionSettings.CurrentDatabaseName);                                       //Load DB from DB name in settings
            var LocationRecords = new CallsignLocationLookup();                         //Load location DB for use with callsign prefixes
         
            
            var MainGui = new GUI(SessionDatabase, SessionSettings);


            Application.Init();
            var top = MainGui.MainUITopLevel();
            Application.Run(top);



        }


    }
}



// Dictonairy locationlookup, tryget