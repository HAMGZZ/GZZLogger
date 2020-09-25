using GZZLogger.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace GZZLogger.GUI
{
    internal class GUIClass
    {
        private Database sessionDatabase;
        private Settings sessionSettings;
        private CallsignLocationLookup locationLookup;

        private Toplevel mainTop;

        private ListView logList;
        private Rect logListRect = new Rect(0, 1, 89, 21);

        private TextField callsignEntry;
        private TextField frequencyEntry;
        private TextField txrEntry;
        private TextField rxrEntry;
        private TextField rxExchangeEntry;
        private TextField txExchangeEntry;
        private TextField commentEntry;
        private TextField modeEntry;

        private bool enterKeyEventEdit;

        public GUIClass(Settings settings, Database database, CallsignLocationLookup locationLookup)
        {
            this.sessionSettings = settings;
            this.sessionDatabase = database;
            this.locationLookup = locationLookup;
        }

        public Toplevel settingsUI()
        {
            var settingsTop = new Toplevel();
            var window = new Window("Settings")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 40,
                Height = 20,
            };
            //var labelLength = new Label(1, 1, "Length (hours):");
            var labelName = new Label(1, 3, "Contest Name:");
            var labelET = new Label(1, 5, "Exchange type:");
            var labelMode = new Label(1, 8, "Mode:");
            var labelPower = new Label(20, 8, "Power:");
            var labelBand = new Label(1, 12, "Band:");
            var ContestName = new TextField(20, 3, 15, "");
            var exchange = new RadioGroup(20, 5, new[] { "_Constant", "_Serial" });
            var mode = new RadioGroup(7, 8, new[] { "PH", "CW", "DIGI" });
            var power = new RadioGroup(27, 8, new[] { "HIGH", "LOW", "QRP" });
            var band = new TextField(20, 12, 15, "40M");
            var ok = new Button(30, 17, "OK")
            {
                Clicked = () =>
                {
                    sessionSettings.Length = 0;
                    sessionSettings.ContestName = ContestName.Text.ToString();
                    sessionSettings.Band = band.Text.ToString();
                    sessionSettings.IncramentalExchg = Convert.ToBoolean(exchange.Selected);
                    switch (mode.Selected)
                    {
                        case 0:
                            sessionSettings.Mode = "PH";
                            break;

                        case 1:
                            sessionSettings.Mode = "CW";
                            break;

                        case 2:
                            sessionSettings.Mode = "DIGI";
                            break;
                    }
                    switch (power.Selected)
                    {
                        case 0:
                            sessionSettings.Power = "HIGH";
                            break;

                        case 1:
                            sessionSettings.Power = "LOW";
                            break;

                        case 2:
                            sessionSettings.Power = "QRP";
                            break;
                    }
                    var top = new Toplevel();
                    top = MainUIToplevel();
                    Application.Run(top);
                }
            };
            window.Add(//labelLength,
                       labelName,
                       labelET,
                       labelMode,
                       labelPower,
                       labelBand,
                       ContestName,
                       exchange,
                       mode,
                       power,
                       band,
                       ok);
            settingsTop.Add(window);
            return settingsTop;
        }

        public Toplevel MainUIToplevel()
        {
            mainTop = new Toplevel();
            mainTop.ColorScheme.Normal = Application.Driver.MakeAttribute(Color.Brown, Color.Black);
            mainTop.ColorScheme.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Brown);
            mainTop.ColorScheme.HotNormal = Application.Driver.MakeAttribute(Color.Brown, Color.Black);
            mainTop.ColorScheme.HotFocus = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Brown);

            // LOG WINDOW:
            var logWindow = new Window("log")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(70),
                Height = Dim.Percent(80)
            };

            var logListTitles = new Label(" ID     DATE     TIME    FREQ   MODE   CALLSIGN  TXE    RXE    TXR    RXR    COMMENT")
            {
                X = 0,
                Y = 0
            };
            logList = new ListView(logListRect, sessionDatabase.Records.Reverse().ToList());
            logWindow.Add(logListTitles, logList);

            // CONTACT ENTRY:
            var entryWindow = new Window("New Contact")
            {
                X = 0,
                Y = Pos.Percent(80),
                Width = Dim.Percent(70),
                Height = Dim.Fill()
            };

            var callsignLabel = new Label("CALLSIGN:")
            {
                X = 0,
                Y = 1
            };
            callsignEntry = new TextField("")
            {
                X = Pos.Right(callsignLabel),
                Y = 1,
                Width = 10
            };

            var frequencyLabel = new Label("FREQ:")
            {
                X = 20,
                Y = 1
            };

            string tmp_freq;
            try
            {
                tmp_freq = sessionDatabase.Records.Last().FrequencyBand.ToString();
            }
            catch
            {
                tmp_freq = "";
            }

            frequencyEntry = new TextField(tmp_freq)
            {
                X = Pos.Right(frequencyLabel),
                Y = 1,
                Width = 6
            };

            var txExchangeLabel = new Label("TXE:")
            {
                X = 32,
                Y = 1
            };

            string tmp_exchg = "";
            try
            {
                var txExchg = Int32.Parse(sessionDatabase.Records.Last().TransmittedExchange);
                if (sessionSettings.IncramentalExchg)
                {
                    try
                    {
                        txExchg++;
                        tmp_exchg = txExchg.ToString().PadLeft(4, '0');
                    }
                    catch
                    {
                        tmp_exchg = sessionDatabase.Records.Last().TransmittedExchange.ToString();
                    }
                }
                else
                {
                    tmp_exchg = sessionDatabase.Records.Last().TransmittedExchange.ToString();
                }
            }
            catch
            {
                tmp_exchg = "0001";
            }
            txExchangeEntry = new TextField(tmp_exchg)
            {
                X = Pos.Right(txExchangeLabel),
                Y = 1,
                Width = 6
            };

            var rxExchangeLabel = new Label("RXE:")
            {
                X = 43,
                Y = 1
            };

            rxExchangeEntry = new TextField("")
            {
                X = Pos.Right(rxExchangeLabel),
                Y = 1,
                Width = 6
            };

            var tmpx = 0;
            if (sessionSettings.ContestMode)
            {
                tmpx = 65;
            }
            else
            {
                tmpx = 32;
            }

            var txrLabel = new Label("TXR:")
            {
                X = tmpx,
                Y = 1
            };

            txrEntry = new TextField("59")
            {
                X = Pos.Right(txrLabel),
                Y = 1,
                Width = 6
            };

            var rxrLabel = new Label("RXR:")
            {
                X = tmpx + 11,
                Y = 1
            };

            rxrEntry = new TextField("59")
            {
                X = Pos.Right(rxrLabel),
                Y = 1,
                Width = 6
            };

            var modeLabel = new Label("MODE:")
            {
                X = 0,
                Y = 3
            };

            string tmp_mode;
            try
            {
                tmp_mode = sessionDatabase.Records.Last().Mode;
            }
            catch
            {
                tmp_mode = "";
            }

            modeEntry = new TextField(tmp_mode)
            {
                X = Pos.Right(modeLabel),
                Y = 3,
                Width = 5
            };

            var commentLabel = new Label("COMMENT:")
            {
                X = 15,
                Y = 3
            };

            commentEntry = new TextField("")
            {
                X = Pos.Right(commentLabel),
                Y = 3,
                Width = 30
            };

            var contactInsert = new Button(75, 3, "Insert")
            {
                Clicked = () =>
                {
                    bool okpressed = false;
                    bool dupContact = false;
                    foreach (var log in sessionDatabase.Records.Reverse().ToList())
                    {
                        if ((log.Callsign.Contains(callsignEntry.Text.ToString().ToUpper())) && log.FrequencyBand.ToString().Substring(0, 2).Contains(frequencyEntry.Text.ToString().Substring(0, 2)))
                        {
                            dupContact = true;
                        }
                    }

                    if (dupContact)
                    {
                        var ok = new Button("Yes")
                        {
                            Clicked = () => { Application.RequestStop(); okpressed = true; }
                        };
                        var cancel = new Button("No")
                        {
                            Clicked = () => Application.RequestStop()
                        };
                        var dialog = new Dialog("Error", 60, 7, ok, cancel);
                        var lable = new Label("Duplicate contact, add anyway?");
                        dialog.Add(lable);
                        Application.Run(dialog);
                    }
                    if (okpressed || !dupContact)
                    {
                        GUIAddRecord();
                        updateStats(callsignEntry.Text.ToString().ToUpper());
                    }
                    callsignEntry.Used = false;
                    frequencyEntry.Used = false;
                    txrEntry.Used = false;
                    rxrEntry.Used = false;
                    txExchangeEntry.Used = false;
                    rxExchangeEntry.Used = false;
                    commentEntry.Used = false;
                    modeEntry.Used = false;
                    callsignEntry.Text = "";
                    frequencyEntry.Text = sessionDatabase.Records.Last().FrequencyBand.ToString();
                    try
                    {
                        var txExchg = Int32.Parse(sessionDatabase.Records.Last().TransmittedExchange);
                        if (sessionSettings.IncramentalExchg)
                        {
                            try
                            {
                                txExchg++;
                                tmp_exchg = txExchg.ToString().PadLeft(4, '0');
                            }
                            catch
                            {
                                tmp_exchg = sessionDatabase.Records.Last().TransmittedExchange.ToString();
                            }
                        }
                        else
                        {
                            tmp_exchg = sessionDatabase.Records.Last().TransmittedExchange.ToString();
                        }
                    }
                    catch
                    {
                        tmp_exchg = "";
                    }
                    txExchangeEntry.Text = tmp_exchg;
                    rxExchangeEntry.Text = "";
                    commentEntry.Text = "";
                    modeEntry.Text = sessionDatabase.Records.Last().Mode;
                    logList.SetSource(sessionDatabase.Records.Reverse().ToList());
                    logList.Redraw(logListRect);
                    mainTop.SetFocus(callsignEntry);
                    Application.Refresh();
                }
            };
            if (sessionSettings.ContestMode)
            {
                entryWindow.Add(callsignLabel,
                            callsignEntry,
                            frequencyLabel,
                            frequencyEntry,
                            txExchangeLabel,
                            txExchangeEntry,
                            rxExchangeLabel,
                            rxExchangeEntry,
                            modeLabel,
                            modeEntry,
                            commentLabel,
                            commentEntry,
                            contactInsert,
                            txrLabel,
                            txrEntry,
                            rxrLabel,
                            rxrEntry);
            }
            else
            {
                entryWindow.Add(callsignLabel,
                            callsignEntry,
                            frequencyLabel,
                            frequencyEntry,
                            txrLabel,
                            txrEntry,
                            rxrLabel,
                            rxrEntry,
                            modeLabel,
                            modeEntry,
                            commentLabel,
                            commentEntry,
                            contactInsert);
            }

            //STAT Window
            var statWindow = new Window("Statistics")
            {
                X = Pos.Percent(70),
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var callsignStatLabel = new Label("CALLSIGN: ")
            {
                X = 0,
                Y = 0
            };

            var continentStatLabel = new Label("CONTINENT: ")
            {
                X = 0,
                Y = 1
            };

            var countryStatLabel = new Label("COUNTRY: ")
            {
                X = 0,
                Y = 2
            };

            var ituStatLabel = new Label("ITU: ")
            {
                X = 0,
                Y = 3
            };

            var cqStatLabel = new Label("CQ: ")
            {
                X = 0,
                Y = 4
            };

            var countStatLabel = new Label("CONTACT #:")
            {
                X = 0,
                Y = 7
            };

            statWindow.Add(callsignStatLabel,
                           continentStatLabel,
                           countryStatLabel,
                           ituStatLabel,
                           cqStatLabel,
                           countStatLabel);

            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_New", "Creates new file", () =>
                    {
                        var newFileTop = new introGui(sessionSettings, locationLookup);
                        Application.Run(newFileTop.introUITopLevel());
                    }),
                    new MenuItem ("_Open", "Open old database", null),
                    new MenuItem ("_Export", "Export Cabrilo", () =>
                    {

                        var name = new Label(1, 1, "Name: ");
                        var address1 = new Label(1, 3, "Address: ");
                        var email = new Label(1, 9, "Email: ");
                        var loaction = new Label(1, 11, "Location: ");
                        var operatorNum = new Label(1, 13, "Operator: ");
                        var assisted = new Label(1, 15, "Assisted: ");
                        var stationType = new Label(1, 17, "Station: ");
                        var transmitterNum = new Label(1, 19, "TX Num.:");

                        var nameInput = new TextField(sessionSettings.UserName)
                        {
                            X = 10,
                            Y = 1,
                            Width = 45
                        };
                        var address1Input = new TextField(sessionSettings.Address[0])
                        {
                            X = 10,
                            Y = 3,
                            Width = 45
                        };
                        var address2Input = new TextField(sessionSettings.Address[1])
                        {
                            X = 10,
                            Y = 4,
                            Width = 45
                        };
                        var address3Input = new TextField(sessionSettings.Address[2])
                        {
                            X = 10,
                            Y = 5,
                            Width = 45
                        };
                        var address4Input = new TextField(sessionSettings.Address[3])
                        {
                            X = 10,
                            Y = 6,
                            Width = 45
                        };
                        var address5Input = new TextField(sessionSettings.Address[4])
                        {
                            X = 10,
                            Y = 7,
                            Width = 45
                        };
                        var emailInput = new TextField(sessionSettings.Email)
                        {
                            X = 10,
                            Y = 9,
                            Width = 45
                        };
                        var locationInput = new TextField("DX")
                        {
                            X = 10,
                            Y = 11,
                            Width = 45
                        };
                        var operatorNumInput = new TextField("SINGLE-OP")
                        {
                            X = 10,
                            Y = 13,
                            Width = 45
                        };
                        var assistedInput = new TextField("NON-ASSISTED")
                        {
                            X = 10,
                            Y = 15,
                            Width = 45
                        };
                        var stationInput = new TextField("FIXED")
                        {
                            X = 10,
                            Y = 17,
                            Width = 45
                        };
                        var transInput = new TextField("ONE")
                        {
                            X = 10,
                            Y = 19,
                            Width = 45
                        };
                        var ok = new Button("Ok")
                        {
                            Clicked = () =>
                            {
                                sessionSettings.Location = locationInput.Text.ToString().ToUpper();
                                sessionSettings.OpNum = operatorNumInput.Text.ToString().ToUpper();
                                sessionSettings.Assisted = assistedInput.Text.ToString().ToUpper();
                                sessionSettings.StationType = stationInput.Text.ToString().ToUpper();
                                sessionSettings.TransNum = transInput.Text.ToString().ToUpper();
                                sessionSettings.UserName = nameInput.Text.ToString().ToUpper();
                                sessionSettings.Email = emailInput.Text.ToString().ToUpper();
                                sessionSettings.Address = new string[] {address1Input.Text.ToString(), address2Input.Text.ToString(), address3Input.Text.ToString(), address4Input.Text.ToString(), address5Input.Text.ToString() };
                                var export = new ImportExport(sessionDatabase, sessionSettings);
                                if(sessionSettings.ContestMode)
                                    export.exportCabrillo();
                                else
                                    export.exportNormal();
                                Application.RequestStop();
                                var ok2 = new Button("OK")
                                {
                                    Clicked = () =>
                                    {
                                        Application.RequestStop();
                                    }
                                };
                                var doneDialog = new Dialog("Export", 70, 7, ok2);
                                Label doneText;
                                if(sessionSettings.ContestMode)
                                    doneText = new Label("Exported to \"" + sessionSettings.ContestName + ".txt\" in your Documents folder.");
                                else
                                    doneText = new Label("Exported to \"" + DateTime.Now.ToString("yyyy-MM-dd")+ ".txt\" in your Documents folder.");
                                doneDialog.Add(doneText);
                                Application.Run(doneDialog);
                            }
                        };
                        var dialog = new Dialog("Export", 60, 30, ok);
                        if(sessionSettings.ContestMode)
                        {
                            dialog.Add(name,
                                        address1,
                                        email,
                                        loaction,
                                        operatorNum,
                                        assisted,
                                        stationType,
                                        transmitterNum,
                                        nameInput,
                                        address1Input,
                                        address2Input,
                                        address3Input,
                                        address4Input,
                                        address5Input,
                                        emailInput,
                                        locationInput,
                                        operatorNumInput,
                                        assistedInput,
                                        stationInput,
                                        transInput
                                        );
                        }
                        else
                        {
                            dialog.Add(name,
                                        address1,
                                        email,
                                        nameInput,
                                        address1Input,
                                        address2Input,
                                        address3Input,
                                        address4Input,
                                        address5Input,
                                        emailInput
                                        );
                        }

                        Application.Run(dialog);
                    }),
                    new MenuItem ("_Quit", "", () =>
                    {
                        mainTop.Running = false;
                    })
                }),
                new MenuBarItem ("_Setting", new MenuItem [] {
                    new MenuItem ("_Contest mode >", sessionSettings.ContestMode.ToString(), () =>
                    {
                        sessionSettings.ContestMode = !sessionSettings.ContestMode;
                        mainTop.Running = false;
                    }),
                    new MenuItem ("_Edit Settings", "Not yet implamented", () =>
                    {

                    })
                })
               });
            menu.ColorScheme.Normal = Application.Driver.MakeAttribute(Color.Brown, Color.Black);
            menu.ColorScheme.Focus = Application.Driver.MakeAttribute(Color.Brown, Color.Black);
            menu.ColorScheme.HotFocus = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);


            //menu bar info:
            var logFileNameLabel = new Label(30, 0, "File: " + sessionSettings.CurrentDatabaseName);
            var userCallsignLabel = new Label(60, 0, sessionSettings.Callsign);
            var contestNameLabel = new Label(80, 0, "");
            if (sessionSettings.ContestMode)
            {
                
                contestNameLabel = new Label(80, 0, "Contest: " + sessionSettings.ContestName);
            }

            
            enterKeyEventEdit = logList.ProcessKey(new KeyEvent(Key.Enter));
            logList.SelectedChanged += LogList_SelectedChanged;

            callsignEntry.Changed += CallsignEntry_Changed;
            string tmp_callsign;
            try
            {
                tmp_callsign = sessionDatabase.Records.Last().Callsign;
            }
            catch
            {
                tmp_callsign = "";
            }
            updateStats(tmp_callsign);


            mainTop.Add(menu, entryWindow, logWindow, statWindow, logFileNameLabel, userCallsignLabel, contestNameLabel);
            return (mainTop);
        }

        private void CallsignEntry_Changed(object sender, NStack.ustring e)
        {
            var enteredCallsignText = callsignEntry.Text.ToString().ToUpper();
            var workableList = new List<ContestLogRecord>();
            foreach (var log in sessionDatabase.Records.Reverse().ToList())
            {
                if (log.Callsign.Contains(enteredCallsignText))
                {
                    workableList.Add(log);
                }
            }

            logList.SetSource(workableList);
            logList.Redraw(logListRect);

            updateStats(enteredCallsignText);
        }

        private void LogList_SelectedChanged()
        {
            if (sessionDatabase.Records.Count() == logList.Source.Count)
            {
                var data = sessionDatabase.Records.Reverse().ToList()[logList.SelectedItem];
                updateStats(data.Callsign);
            }
            if(enterKeyEventEdit)
            {
                mainTop.Running = false;
            }
        }

        private void GUIAddRecord()
        {
            var log = new ContestLogRecord();
            log.Callsign = callsignEntry.Text.ToString().ToUpper();
            if (Int32.TryParse(frequencyEntry.Text.ToString(), out int i))
            {
                log.FrequencyBand = i;
            }
            log.TransmittedReport = txrEntry.Text.ToString();
            log.ReceivedReport = rxrEntry.Text.ToString();
            log.Comments = commentEntry.Text.ToString();
            log.Mode = modeEntry.Text.ToString().ToUpper();
            log.ReceivedExchange = rxExchangeEntry.Text.ToString();
            log.TransmittedExchange = txExchangeEntry.Text.ToString();
            sessionDatabase.AddRecord(log);
        }

        private void updateStats(string callsign)
        {
            var callsignData = locationLookup.GetCallsingComponents(callsign);
            if (callsignData != null)
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
                    Text = sessionDatabase.Records.Count().ToString()
                };

                mainTop.Add(callsignStat, continentStat, countryStat, ituStat, cqStat, countStat);
            }
        }
    }
}