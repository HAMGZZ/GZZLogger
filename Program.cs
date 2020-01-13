using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Newtonsoft.Json;
using CsvHelper.TypeConversion;

namespace GZZLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GZZ Logger 2020");
            var SessionSettings = new Settings();                                       //Load Settings from file
            var SessionDatabase = new Database(SessionSettings);                        //Load DB from DB name in settings

            for (int i = 0; i < 100000; i++)
            {
                var newRec = new ContestLogRecord
                {
                    Callsign = "VK2GZZ",
                    FrequencyBand = 7000,
                    Mode = "SSB",
                    TransmittedSerial = "AUS",
                    ReceivedSerial = "59",
                    Comments = "Really good signal"
                };
                SessionDatabase.AddRecord(newRec);


            }
        }


    }
}
