using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GZZLogger
{
    class CallsignLocationLookup
    {

        private Dictionary<string, CallsignLocationRecord> dictionay;

        public CallsignLocationLookup()
        {
            dictionay = new Dictionary<string, CallsignLocationRecord>();
            try
            {
                import();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void import()
        {
            using (var reader = new StreamReader("callsignLocationLookup.csv"))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                dictionay = csv.GetRecords<CallsignLocationRecord>().ToDictionary(k => k.Prefix,
                    x => new CallsignLocationRecord { Country = x.Country, Continent = x.Continent, CQ = x.CQ, ITU = x.ITU });
            }
        }

        /*Pass in a callsign and retrieve all the location data involved with the prefix*/
        public CallsignLocationRecord GetCallsingComponents(string callsign)
        {
            // TODO: This should be made more robust...
            var lengthOfString = Math.Min(4,callsign.Length);
            CallsignLocationRecord returnedResult;
            while (lengthOfString > 0)
            {
                var callsignSubString = callsign.Substring(0,lengthOfString);
                if(dictionay.TryGetValue(callsignSubString, out returnedResult))
                {
                    return returnedResult;
                }

                lengthOfString--;

            }
            return null;
            
        }
    }
}
