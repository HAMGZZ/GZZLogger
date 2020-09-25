using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GZZLogger
{
    class Database
    {
        static private int currentRecordId;
        private int lastStoredRecordId;
        private string databaseName;
        private List<ContestLogRecord> records;

        public IEnumerable<ContestLogRecord> Records { get => records; }
        public Database(string currentDatabaseName = "test.db")
        {
            this.databaseName = currentDatabaseName;
            currentRecordId = 0;
            lastStoredRecordId = 0;
            records = new List<ContestLogRecord>();
            try
            {
                import();

            }
            catch (Exception)
            {
                
            }
        }

        public void write()
        {
            using (var writer = new StreamWriter(databaseName, append: true))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
            {
                if (lastStoredRecordId > 0)
                {
                    csv.Configuration.HasHeaderRecord = false;
                }
                else
                {
                    csv.Configuration.HasHeaderRecord = true;
                }
                csv.Configuration.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new string[] { "o" };
                var r = records.Where(x => x.RecordId > lastStoredRecordId);
                csv.WriteRecords(r);
                lastStoredRecordId = currentRecordId;
            }
        }


        public void import()
        {

            using (var reader = new StreamReader(databaseName))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                records = csv.GetRecords<ContestLogRecord>().ToList();
            }
            try
            {
                currentRecordId = records.Last().RecordId;
                lastStoredRecordId = currentRecordId;
            }
            catch (Exception)
            {
                currentRecordId = 0;
            }

        }

        public int AddRecord(ContestLogRecord value)
        {
            currentRecordId++;
            value.RecordId = currentRecordId;
            value.TimeStamp = DateTime.UtcNow;
            records.Add(value);
            write();
            return (value.RecordId);
        }


    }
}
