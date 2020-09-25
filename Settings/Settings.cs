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
        private bool contestMode;
        private bool incramentalExchg;
        private int ituZone;
        private int cqZone;
        private string callsign = "";
        private string location = "";
        private string contestName = "";
        private string mode = "";
        private string opNum = "";
        private string assisted = "";
        private string band = "";
        private string power = "";
        private string stationType = "";
        private string transNum = "";
        private string userName = "";
        private string email = "";
        private string[] address = {"", "", "", "", "" };
        private DateTime logStartDate;
        private DateTime contestStartDate;
        private DateTime contestStartTime;
        private int length;


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

        public string Location
        {
            get => location;
            set
            {
                location = value;
                Persist();
            }
        }
        public string ContestName
        {
            get
            {
                if(contestName  == "")
                {
                    return "NO NAME SET";
                }
                return contestName;
            }
            set
            {
                contestName = value;
                Persist();
            }
        }
        public string Mode
        {
            get => mode;
            set
            {
                mode = value;
                Persist();
            }
        }
        public string OpNum
        {
            get => opNum;
            set
            {
                opNum = value;
                Persist();
            }
        }
        public string Assisted
        {
            get => assisted;
            set
            {
                assisted = value;
                Persist();
            }
        }
        public string Band
        {
            get => band;
            set
            {
                band = value;
                Persist();
            }
        }
        public string Power
        {
            get => power;
            set
            {
                power = value;
                Persist();
            }
        }
        public string StationType
        {
            get => stationType;
            set
            {
                stationType = value;
                Persist();
            }
        }
        public string TransNum
        {
            get => transNum;
            set
            {
                transNum = value;
                Persist();
            }
        }
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                Persist();
            }
        }
        public string Email
        {
            get => email;
            set
            {
                email = value;
                Persist();
            }
        }
        public string[] Address
        {
            get => address;
            set
            {
                address = value;
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

        public bool IncramentalExchg
        {
            get => incramentalExchg;
            set
            {
                incramentalExchg = value;
                Persist();
            }
        }

        public DateTime LogStartDate
        {
            get => logStartDate;
            set
            {
                logStartDate = value;
                Persist();
            }
        }
        public int Length
        {
            get => length;
            set
            {
                length = value;
                Persist();
            }
        }
        public DateTime ContestStartDate
        {
            get => contestStartDate;
            set
            {
                contestStartDate = value;
                Persist();
            }
        }
        public DateTime ContestStartTime
        {
            get => contestStartTime;
            set
            {
                contestStartTime = value;
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
                Persist(); 
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
