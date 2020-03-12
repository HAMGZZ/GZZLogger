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
        private CallsignLocationLookup locationLookup;

        private Toplevel top;
        private Rect rect = new Rect(1, 3, 89, 21); //TODO: PLEASE RENAME THIS LEWIS!
        private Window logWindow;
        private Label logListTitles;
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
        private Label exchange;
        private TextField exchangeEntry;
        private Label comment;
        private TextField commentEntry;
        private Label mode;                 //TODO: I ALSO DONT NEED LABELS UP HERE?
        private TextField modeEntry;
        private Label callsignStatLabel;
        private Label continentStatLabel;
        private Label countryStatLabel;
        private Label ituStatLabel;
        private Label cqStatLabel;


        public GUI(Database database, Settings settings, CallsignLocationLookup locationLookup)
        {
            this.database = database;
            this.settings = settings;
            this.locationLookup = locationLookup;
        }

        public Toplevel MainUITopLevel()
        {

            top = new Toplevel();
            top.ColorScheme.Normal = Application.Driver.MakeAttribute(Color.Brown, Color.Black);
            top.ColorScheme.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Brown);
            top.ColorScheme.HotNormal = Application.Driver.MakeAttribute(Color.Red, Color.Black);
            top.ColorScheme.HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Red);

            //LOG WINDOW
            logWindow = new Window("Log")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(70),
                Height = Dim.Percent(80)
            };


            logListTitles = new Label(" ID     DATE     TIME    FREQ   MODE   CALLSIGN  TXS    RXS    COMMENT")
            {
                X = 1,
                Y = 2,
            };
            logList = new ListView(rect, database.Records.Reverse().ToList());

            // CONTACT ENTRY WINDOW
            entryWindow = new Window("New Contact")
            {
                X = 0,
                Y = Pos.Percent(80),
                Width = Dim.Percent(70),
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
            string tmp_freq;
            try
            {
                tmp_freq = database.Records.Last().FrequencyBand.ToString();
            }
            catch
            {
                tmp_freq = "";
            }
            frequencyEntry = new TextField(tmp_freq)
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

            exchange = new Label("RX EXCHG:")
            {
                X = Pos.Percent(44),
                Y = Pos.Percent(85)
            };

            exchangeEntry = new TextField("")
            {
                X = Pos.Right(exchange),
                Y = Pos.Percent(85),
                Width = 23
            };


            mode = new Label("MODE:")
            {
                X = 1,
                Y = Pos.Percent(93)
            };
            string tmp_mode;
            try
            {
                tmp_mode = database.Records.Last().Mode;
            }
            catch
            {
                tmp_mode = "";
            }
            modeEntry = new TextField(tmp_mode)
            {
                X = Pos.Right(mode),
                Y = Pos.Percent(93),
                Width = 5
            };


            comment = new Label("COMMENT:")
            {
                X = Pos.Percent(15),
                Y = Pos.Percent(93)
            };

            commentEntry = new TextField("")
            {
                X = Pos.Right(comment),
                Y = Pos.Percent(93),
                Width = 30
            };

            var contactInsert = new Button(80, 29, "Insert")
            {
                Clicked = () =>
                {
                    GUIAddRecord();
                    updateStats(callsignEntry.Text.ToString().ToUpper());
                    callsignEntry.Used = false;
                    frequencyEntry.Used = false;
                    txsEntry.Used = false;
                    rxsEntry.Used = false;
                    commentEntry.Used = false;
                    modeEntry.Used = false;
                    callsignEntry.Text = "";
                    frequencyEntry.Text = database.Records.Last().FrequencyBand.ToString();
                    //? txs = default
                    //? rxs = default
                    commentEntry.Text = "";
                    modeEntry.Text = database.Records.Last().Mode;
                    logList.SetSource(database.Records.Reverse().ToList());
                    logList.Redraw(rect);
                    top.SetFocus(callsignEntry);
                    Application.Refresh();

                }
            };




            //STAT WINDOW

            statWindow = new Window("Statistics")
            {
                X = Pos.Percent(70),
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            callsignStatLabel = new Label("CALLSIGN: ")
            {
                X = Pos.Left(statWindow) + 1,
                Y = 2
            };

            continentStatLabel = new Label("CONTINENT: ")
            {
                X = Pos.Left(statWindow) + 1,
                Y = 3
            };

            countryStatLabel = new Label("COUNTRY: ")
            {
                X = Pos.Left(statWindow) + 1,
                Y = 4
            };

            ituStatLabel = new Label("ITU: ")
            {
                X = Pos.Left(statWindow) + 1,
                Y = 5
            };

            cqStatLabel = new Label("CQ: ")
            {
                X = Pos.Left(statWindow) + 1,
                Y = 6
            };



            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_New", "Creates new file", null),
                    new MenuItem ("_Close", "", null),
                    new MenuItem ("_Quit", "", () => { top.Running = false; })
                }),
                new MenuBarItem ("_Setting", new MenuItem [] {
                    new MenuItem ("_TX-Type", "", null),
                    new MenuItem ("_Your callsign", "", null)
                })
               });


            menu.ColorScheme.Normal = Application.Driver.MakeAttribute(Color.Brown, Color.Black);
            menu.ColorScheme.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Brown);
            menu.ColorScheme.HotNormal = Application.Driver.MakeAttribute(Color.Red, Color.Black);
            menu.ColorScheme.HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Red);

            top.Add(
                    menu,
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
                    exchange,
                    exchangeEntry,
                    mode,
                    modeEntry,
                    comment,
                    commentEntry,
                    contactInsert,
                    logWindow,
                    logListTitles,
                    logList,
                    callsignStatLabel,
                    continentStatLabel,
                    countryStatLabel,
                    ituStatLabel,
                    cqStatLabel);


            callsignEntry.Changed += CallsignEntry_Changed;
            string tmp_callsign;
            try
            {
                tmp_callsign = database.Records.Last().Callsign;
            }
            catch
            {
                tmp_callsign = "";
            }
            updateStats(tmp_callsign);
            return top;
        }

        private void CallsignEntry_Changed(object sender, EventArgs e)
        {
            var enteredCallsignText = callsignEntry.Text.ToString().ToUpper();
            var workableList = new List<ContestLogRecord>();
            foreach (var log in database.Records.Reverse().ToList())
            {
                if (log.Callsign.StartsWith(enteredCallsignText))
                {
                    workableList.Add(log);
                }
            }

            logList.SetSource(workableList);
            logList.Redraw(rect);

            updateStats(enteredCallsignText);

        }

        private void GUIAddRecord()
        {
            var log = new ContestLogRecord();
            log.Callsign = callsignEntry.Text.ToString().ToUpper();
            if (Int32.TryParse(frequencyEntry.Text.ToString(), out int i))
            {
                log.FrequencyBand = i;
            }
            log.TransmittedSerial = txsEntry.Text.ToString();
            log.ReceivedSerial = rxsEntry.Text.ToString();
            log.Comments = commentEntry.Text.ToString();
            log.Mode = modeEntry.Text.ToString().ToUpper();

            database.AddRecord(log);
        }

        private void updateStats(string callsign) // TODO: WHY DOES THIS NOT WORK?
        {
            var callsignData = locationLookup.GetCallsingComponents(callsign);
            if(callsignData != null)
            {
                var callsignStat = new TextView(new Rect(105, 2, 15, 1))
                {
                    CanFocus = false,
                    ReadOnly = true,
                    Text = callsign
                };

                var continentStat = new TextView(new Rect(105, 3, 15, 1))
                {
                    CanFocus = false,
                    ReadOnly = true,
                    Text = callsignData.Continent
                };

                var countryStat = new TextView(new Rect(105, 4, 15, 1))
                {
                    CanFocus = false,
                    ReadOnly = true,
                    Text = callsignData.Country
                };

                var ituStat = new TextView(new Rect(105, 5, 15, 1))
                {
                    CanFocus = false,
                    ReadOnly = true,
                    Text = callsignData.ITU
                };

                var cqStat = new TextView(new Rect(105, 6, 15, 1))
                {
                    CanFocus = false,
                    ReadOnly = true,
                    Text = callsignData.CQ
                };

                var countStat = new TextView(new Rect(105, 9, 15, 1))
                {
                    CanFocus = false,
                    ReadOnly = true,
                    Text = database.Records.Count().ToString()
                };

                top.Add(callsignStat, continentStat, countryStat, ituStat, cqStat);
            }
            

        }

    }
}
