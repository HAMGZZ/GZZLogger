using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GZZLogger
{
    public class Settings
    {
        private string currentDatabaseName = "database.db";
        private string previousDatabaseName = "";
        private bool contestMode = false;
        private string contestName = "CQWW";
        private string fileName;
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
        public bool ContestMode
        {
            get => contestMode;
            set
            {
                contestMode = value;
                Persist();
            }
        }

        public string ContestName
        {
            get => contestName;
            set
            {
                contestName = value;
                Persist();
            }
        }

        public string PreviousDatabaseName
        {
            get => previousDatabaseName; set
            {
                previousDatabaseName = value;
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
