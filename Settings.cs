using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GZZLogger
{
    public class Settings
    {
        private string currentDatabaseName = "";
        private string previousDatabaseName = "";
        private string fileName;
        private string callsign;
        private int ituZone;
        private int cqZone;
        private bool initializing;
        public string CurrentDatabaseName
        {
            get => currentDatabaseName;
            set
            {
                previousDatabaseName = currentDatabaseName;
                currentDatabaseName = value;
                Persist();
            }
        }

        public string PreviousDatabaseName
        {
            get => previousDatabaseName;
            set
            {
                previousDatabaseName = value;
                Persist();
            }
        }

        public string Callsign 
        { 
            get => callsign; 
            set
            {
                callsign = value;
                Persist();
            }
        }
        public int ItuZone
        {
            get => ituZone; 
            set
            {
                ituZone = value;
                Persist();
            }
        }
        public int CqZone 
        { 
            get => cqZone;
            set
            {
                cqZone = value;
                Persist();
            }
        }

        public Settings(string fileName = "settings.json")
        {
            this.fileName = fileName;
            this.initializing = true;
            try
            {
                JsonConvert.PopulateObject(File.ReadAllText(fileName), this);

            }
            catch (Exception)
            {
                this.initializing = false;
                Persist(); //? This will never run? due to initialize bool.
            }
           this.initializing = false;
        }

        public void Persist()
        {
            if (!initializing)
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }

    }
}
