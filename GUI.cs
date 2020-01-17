using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Terminal.Gui;

namespace GZZLogger
{
    class GUI
    {

        private Database database;
        private Settings settings;

        private Window logWindow;
        private ListView logList;
        private Window entryWindow;
        private Window statWindow;
        private Label callsign;
        private TextField callsignEntry;
        private Label frequency;
        private TextField frequencyEntry;
        private Label txs;
        private TextField txsEntry;
        private Label rxs;
        private TextField rxsEntry;
        private Label comment;
        private TextField commentEntry;
        private Label mode;
        private TextField modeEntry;

        public GUI(Database database, Settings settings)
        {
            this.database = database;
            this.settings = settings;

        }

        public Toplevel MainUITopLevel()
        {

            var top = new Toplevel();

            logWindow = new Window("Log")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(70),
                Height = Dim.Percent(80)
            };
            var rect = new Rect(1, 2, 80, 20);
            logList = new ListView(rect, database.Records.ToList());


            entryWindow = new Window("New Contact")
            {
                X = 0,
                Y = Pos.Percent(80),
                Width = Dim.Percent(70),
                Height = Dim.Fill()
            };
            statWindow = new Window("Statistics")
            {
                X = Pos.Percent(70),
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            callsign = new Label("CALLSIGN:")
            {
                X = 1,
                Y = Pos.Percent(85)
            };
            callsignEntry = new TextField("")
            {
                X = Pos.Right(callsign),
                Y = Pos.Percent(85),
                Width = 10

            };
            frequency = new Label("FREQ:")
            {
                X = Pos.Percent(17),
                Y = Pos.Percent(85)
            };
            frequencyEntry = new TextField("")
            {
                X = Pos.Right(frequency),
                Y = Pos.Percent(85),
                Width = 6
            };
            txs = new Label("TXS:")
            {
                X = Pos.Percent(27),
                Y = Pos.Percent(85)
            };
            txsEntry = new TextField("59")
            {
                X = Pos.Right(txs),
                Y = Pos.Percent(85),
                Width = 5
            };
            rxs = new Label("RXS:")
            {
                X = Pos.Right(txsEntry) + 2,
                Y = Pos.Percent(85)
            };
            rxsEntry = new TextField("59")
            {
                X = Pos.Right(rxs),
                Y = Pos.Percent(85),
                Width = 5
            };

            comment = new Label("COMMENT:")
            {
                X = Pos.Percent(44),
                Y = Pos.Percent(85)
            };

            commentEntry = new TextField("")
            {
                X = Pos.Right(comment),
                Y = Pos.Percent(85),
                Width = 25
            };

            mode = new Label("MODE:")
            {
                X = 1,
                Y = Pos.Percent(93)
            };

            modeEntry = new TextField("")
            {
                X = Pos.Right(mode),
                Y = Pos.Percent(93),
                Width = 5
            };

            var contactInsert = new Button(80, 29, "Insert")
            {
                Clicked = () =>
                {
                    GUIAddRecord();
                    callsignEntry.Used = false;
                    frequencyEntry.Used = false;
                    txsEntry.Used = false;
                    rxsEntry.Used = false;
                    commentEntry.Used = false;
                    modeEntry.Used = false;
                    callsignEntry.Text = ""; 
                    frequencyEntry.Text = "";
                    //? txs = default
                    //? rxs = default
                    commentEntry.Text = "";
                    modeEntry.Text = "";
                    top.SetFocus(callsignEntry);
                    Application.Refresh();
                    
                }
            };

            top.Add(logWindow,
                    logList,
                    entryWindow,
                    statWindow,
                    callsign,
                    callsignEntry,
                    frequency,
                    frequencyEntry,
                    txs,
                    txsEntry,
                    rxs,
                    rxsEntry,
                    comment,
                    commentEntry,
                    mode,
                    modeEntry,
                    contactInsert);



            return top;
        }

        private void GUIAddRecord()
        {
            var log = new ContestLogRecord();
            log.Callsign = callsignEntry.Text.ToString();
            if (Int32.TryParse(frequencyEntry.Text.ToString(), out int i))
            {
                log.FrequencyBand = i;
            }
            log.TransmittedSerial = txsEntry.Text.ToString();
            log.ReceivedSerial = rxsEntry.Text.ToString();
            log.Comments = commentEntry.Text.ToString();
            log.Mode = modeEntry.Text.ToString();

            database.AddRecord(log);
        }
    }
}
