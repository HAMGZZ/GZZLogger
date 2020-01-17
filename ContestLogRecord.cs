using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace GZZLogger
{
    //Contains all the variables for each contact.
    public class ContestLogRecord
    {
        public int RecordId{get;set;}
        [DateTimeStyles(DateTimeStyles.AdjustToUniversal)]
            public DateTime TimeStamp{get;set;}
        public int FrequencyBand{get;set;}
        public string Mode{get;set;}
        public string Callsign{get;set;}
        public string TransmittedSerial{get;set;}
        public string ReceivedSerial{get;set;}
        public string Comments{get;set;}

        public override string ToString()
        {
            string data = RecordId.ToString("0000") + "  " + TimeStamp.ToString("dd-MM-yy HH:mm:ss") + "  " + FrequencyBand.ToString("00000") + "  " + Mode.PadRight(5) + "  " + Callsign.PadRight(8) + "  " + TransmittedSerial.PadRight(5) + "  " + ReceivedSerial.PadRight(5) + "  " + Comments;
            return data;
        }
    } 
}
