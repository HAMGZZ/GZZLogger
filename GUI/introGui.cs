using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace GZZLogger.GUI
{
    class introGui
    {
        private Settings settings;
        private CallsignLocationLookup locationLookup;
        private Window window;
        private TextField callsignEntry;
        private TextField ITUEntry;
        private TextField CQEntry;
        private RadioGroup exchange;
        private TextField DatabaseName;
        private TextField StartDate;
        private TextField StartTime;
        private RadioGroup contestMode;

        public introGui(Settings settings, CallsignLocationLookup locationLookup)
        {
            this.settings = settings;
            this.locationLookup = locationLookup;
        }



        public Toplevel introUITopLevel()
        {
            window = new Window("New Log")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 40,
                Height = 20,
            };
            var labelCallsign = new Label(1, 1, "Your Callsign:");
            var labelITU = new Label(1, 3, "ITU Zone:");
            var labelCQ = new Label(1, 5, "CQ Zone:");
            var labelDB = new Label(1, 7, "Database Name:");
            var labelSD = new Label(1, 9, "Log Start Date:");
            var labelContestMode = new Label(1, 11, "Contest Mode?:");


            callsignEntry = new TextField(20, 1, 12, "");
            DatabaseName = new TextField(20, 7, 12, "");
            StartDate = new DateField(20, 9, DateTime.Now, false);
            contestMode = new RadioGroup(20, 11, new[] { "_No", "_Yes" });


            var ok = new Button(30, 17, "OK")
            {
                Clicked = () =>
                {
                    if (callsignEntry.Text != string.Empty && DatabaseName.Text != string.Empty)
                    {
                        settings.Callsign = callsignEntry.Text.ToString().ToUpper();
                        settings.ItuZone = Convert.ToInt32(ITUEntry.Text.ToString());
                        settings.CqZone = Convert.ToInt32(CQEntry.Text.ToString());
                        settings.CurrentDatabaseName = DatabaseName.Text.ToString();
                        settings.PreviousDatabaseName = settings.CurrentDatabaseName;
                        //settings.LogStartDate = null;
                        var SessionDatabase = new Database(settings.CurrentDatabaseName);    //Load DB from DB name in settings
                        var MainGui = new GUI.GUIClass(settings, SessionDatabase, locationLookup);
                        Application.RequestStop();
                        Application.Init();
                        if(contestMode.Selected == 1)
                        {
                            settings.ContestMode = true;
                            var top = MainGui.settingsUI();
                            Application.Run(top);
                        }
                        else
                        {
                            settings.ContestMode = false;
                            var top = MainGui.MainUIToplevel();
                            Application.Run(top);
                        }
                    }
                    else
                    {

                        Label label;
                        var ok = new Button("ok")
                        {
                            Clicked = () => { Application.RequestStop(); }
                        };
                        var dialog = new Dialog("Error", 60, 7, ok);
                        if (callsignEntry.Text == string.Empty)
                        {
                            label = new Label("No callsign included");
                        }
                        else if (DatabaseName.Text == string.Empty)
                        {
                            label = new Label("No database name included");
                        }
                        else
                        {
                            label = new Label("Undefined error");
                        }
                        dialog.Add(label);
                        Application.Run(dialog);
                    }
                }
            };
            var cancel = new Button(45, 17, "Cancel")
            {
                Clicked = () =>
                {
                    Application.RequestStop();
                    Environment.Exit(1);
                }
            };
            window.Add(
                        labelCallsign,
                        labelITU,
                        labelCQ,
                        labelDB,
                        labelSD,
                        labelContestMode,
                        callsignEntry,
                        DatabaseName,
                        StartDate,
                        contestMode,
                        ok,
                        cancel);

            callsignEntry.Changed += CallsignEntry_Changed;
            var introTop = Application.Top;
            introTop.Add(window);
            return (introTop);
        }


        private void CallsignEntry_Changed(object sender, NStack.ustring e)
        {
            var callsignData = locationLookup.GetCallsingComponents(callsignEntry.Text.ToString().ToUpper());
            if (callsignData != null)
            {
                ITUEntry = new TextField(20, 3, 12, callsignData.ITU);
                CQEntry = new TextField(20, 5, 12, callsignData.CQ);
                window.Add(ITUEntry, CQEntry);
            }

        }

    }
}
