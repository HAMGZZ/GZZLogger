using System;
using System.Collections.Generic;
using System.Text;

namespace GZZLogger.ScoringEngine
{
    class CQWWEngine
    {
        private CallsignLocationRecord callsignLocation;
        private Database database;
        private int score;
        public int Score { get => score; private set => score = value; }
        public CQWWEngine(CallsignLocationRecord callsignLocation, Database database)
        {
            this.callsignLocation = callsignLocation;
            this.database = database;
        }

        public void NewScore()
        {
            foreach (var callsign in database.Records)
            {
                
            }
        }
        
    }
}
