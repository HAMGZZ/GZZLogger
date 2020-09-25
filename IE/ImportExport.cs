using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZZLogger.IE
{
    class ImportExport
    {
        private Database database;
        private Settings settings;
        public ImportExport(Database database, Settings settings)
        {
            this.database = database;
            this.settings = settings;
        }

        public void exportCabrillo()
        {

            
            string[] header = 
            {
                    "START-OF-LOG: 3.0",
                    "CREATED-BY: GZZLogger V1",
                    "CALLSIGN: " + settings.Callsign,
                    "LOCATION: " + settings.Location,
                    "CONTEST: " + settings.ContestName,
                    "CATEGORY-MODE: " + settings.Mode,
                    "CATEGORY-OPERATOR: " + settings.OpNum,
                    "CATEGORY-ASSISTED: " + settings.Assisted,
                    "CATEGORY-BAND: " + settings.Band,
                    "CATEGORY-POWER: " + settings.Power,
                    "CATEGORY-STATION: " + settings.StationType,
                    "CATEGROY-TRANSMITTER: " + settings.TransNum,
                    "CLAIMED-SCORE: ",
                    "NAME: " + settings.UserName,
                    "EMAIL: " + settings.Email,
                    "ADDRESS: " + settings.Address[0],
                    "ADDRESS: " + settings.Address[1],
                    "ADDRESS: " + settings.Address[2],
                    "ADDRESS: " + settings.Address[3],
                    "ADDRESS: " + settings.Address[4],
                    "SOAPBOX: ",
            };
            
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(documentsFolder, settings.ContestName + ".txt")))
            {
                foreach (string line in header)
                {
                    outputFile.WriteLine(line);
                }

                foreach (ContestLogRecord qso in database.Records)
                {
                    string output = "QSO: " + qso.FrequencyBand.ToString().PadLeft(5) + " " + settings.Mode + " " + qso.TimeStamp.ToString("yyyy-MM-dd HHmm ") + settings.Callsign.PadRight(14) + qso.TransmittedReport.PadLeft(3) + " " + qso.TransmittedExchange.PadRight(7) + qso.Callsign.PadRight(14) + qso.ReceivedReport.PadLeft(3) + " " + qso.ReceivedExchange;
                    outputFile.WriteLine(output);
                }

                outputFile.WriteLine("END-OF-LOG:");
            }
            
        }

        public void exportNormal()
        {


            string[] header =
            {
                    "START-OF-LOG:",
                    "CREATED-BY: GZZLogger V1",
                    "CALLSIGN: " + settings.Callsign,
                    "NAME: " + settings.UserName,
                    "EMAIL: " + settings.Email,
                    "ADDRESS: " + settings.Address[0],
                    "ADDRESS: " + settings.Address[1],
                    "ADDRESS: " + settings.Address[2],
                    "ADDRESS: " + settings.Address[3],
                    "ADDRESS: " + settings.Address[4],
            };

            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(documentsFolder, DateTime.Now.ToString("yyyy-MM-dd") + ".txt")))
            {
                foreach (string line in header)
                {
                    outputFile.WriteLine(line);
                }

                foreach (ContestLogRecord qso in database.Records)
                {
                    string output = "QSO: " + qso.FrequencyBand.ToString().PadLeft(5) + " " + settings.Mode + " " + qso.TimeStamp.ToString("yyyy-MM-dd HHmm ") + settings.Callsign.PadRight(14) + qso.TransmittedReport.PadLeft(3) + " " + qso.TransmittedExchange.PadRight(7) + qso.Callsign.PadRight(14) + qso.ReceivedReport.PadLeft(3) + " " + qso.ReceivedExchange;
                    outputFile.WriteLine(output);
                }

                outputFile.WriteLine("END-OF-LOG:");
            }

        }


    }
}
