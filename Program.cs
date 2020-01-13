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

            while (true)
            {
                ContestLogRecord newlog = new ContestLogRecord();
                Console.Write("CALL: ");
                newlog.Callsign = Console.ReadLine();
                Console.Write("TX SER: ");
                newlog.TransmittedSerial = Console.ReadLine();
                Console.Write("Comment> ");
                newlog.Comments = Console.ReadLine();
                SessionDatabase.AddRecord(newlog);
                var lastContact = SessionDatabase.Records.Last();
                Console.WriteLine();
                Console.WriteLine(
                    "LAST>> Callsing: {0} TX SER: {1} Comment: {2}",
                    lastContact.Callsign,
                    lastContact.TransmittedSerial,
                    lastContact.Comments);
            }


        }


    }
}
