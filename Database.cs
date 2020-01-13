using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GZZLogger
{
    class Database
    {
        static private int lastRecordId;
        private string databaseName;
        private List<ContestLogRecord> records;

        public IEnumerable<ContestLogRecord> Records { get => records; }
        public Database(Settings SessionSettings)
        {
            this.databaseName = SessionSettings.CurrentDatabaseName;
            records = new List<ContestLogRecord>();
            try
            {
                import();

            }
            catch (Exception)
            {
                Console.WriteLine("Database {0} could not be opened.", databaseName);
                Console.Write("New database name (leave blank for auto-name) > ");
                var newName = Console.ReadLine();
                NewDatabse(newName, SessionSettings);
            }
        }

        public void write()
        {
            using (var writer = new StreamWriter(databaseName, append: true))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new string[] { "o" };
                csv.WriteRecords(records);
            }
        }


        public void import()
        {

            using (var reader = new StreamReader(databaseName))
            using (var csv = new CsvReader(reader))
            {
                records = csv.GetRecords<ContestLogRecord>().ToList();
            }
            try
            {
                lastRecordId = records.Last().RecordId;
            }
            catch (Exception)
            {
                lastRecordId = 0;
            }

        }

        public int AddRecord(ContestLogRecord value)
        {
            lastRecordId++;
            value.RecordId = lastRecordId;
            records.Add(value);
            write();
            return (value.RecordId);
        }

        public void NewDatabse(string newDatabaseName, Settings SessionSettings)
        {
            if (string.IsNullOrEmpty(newDatabaseName))
            {
                newDatabaseName = DateTime.Now.ToString("yyyy-MM-dd") + "-LOG.csv";
            }
            databaseName = newDatabaseName;
            SessionSettings.CurrentDatabaseName = newDatabaseName;
            try
            {
                import();
            }
            catch (Exception)
            {

                write();
            }

        }

    }
}
