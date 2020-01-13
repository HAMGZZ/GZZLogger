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
    }
}
