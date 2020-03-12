using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZZLogger
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

        public void export()
        {
            string[] header = 
            {
                    "START-OF-LOG: 2.0",
                    "LOCATION: " + settings.Location,
                    "CALLSIGN: " + settings.Callsign,
                    "CATEGORY: " + settings.Op + " " + settings.Band + " " + settings.Power,
                    "CLAIMED-SCORE: ",
                    "CONTEST: " + settings.ContestName,
                    "CREATED-BY: " + settings.SoftwareName,
                    "NAME: " + settings.UserName,
                    "EMAIL: " + settings.Email,
                    "OPERATORS: " + settings.Operator,
                    "SOAPBOX: ",
            };
            
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(documentsFolder, "CABRILO_OUT.txt")))
            {
                foreach (string line in header)
                {
                    outputFile.WriteLine(line);
                }

                foreach (ContestLogRecord qso in database.Records)
                {
                    string output = "QSO: " + qso.FrequencyBand + " " + qso.Mode + " " + qso.TimeStamp.ToString("yyyy/MM/dd HHmm ") + settings.Callsign.PadLeft(14) + qso.TransmittedReport.PadLeft(4) + qso.TransmittedExchange.PadLeft(7) + qso.Callsign.PadLeft(14) + qso.ReceivedReport.PadLeft(4) + qso.ReceivedExchange;
                    outputFile.WriteLine(output);
                }

                outputFile.WriteLine("END-OF-LOG:");
            }
        }


    }
}
