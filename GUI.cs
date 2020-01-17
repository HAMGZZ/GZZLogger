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
            top.ColorScheme.Normal = Application.Driver.MakeAttribute(Color.Brown, Color.Black);
            top.ColorScheme.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Brown);
            top.ColorScheme.HotNormal = Application.Driver.MakeAttribute(Color.Red, Color.Black);
            top.ColorScheme.HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Red);

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
            var rect = new Rect(1, 3, 89, 21);
            logList = new ListView(rect, database.Records.Reverse().ToList());

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
                    logList.SetSource(database.Records.Reverse().ToList());
                    logList.Redraw(rect);
                    top.SetFocus(callsignEntry);
                    Application.Refresh();

                }
            };

            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_New", "Creates new file", null),
                    new MenuItem ("_Close", "", null),
                    new MenuItem ("_Quit", "", () => { top.Running = false; })
                }),
                new MenuBarItem ("_Edit", new MenuItem [] {
                    new MenuItem ("_Copy", "", null),
                    new MenuItem ("C_ut", "", null),
                    new MenuItem ("_Paste", "", null)
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
                    comment,
                    commentEntry,
                    mode,
                    modeEntry,
                    contactInsert,
                    logWindow,
                    logListTitles,
                    logList);



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
