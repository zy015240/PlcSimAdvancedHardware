﻿//
// PlcSimAdvanced Hardware Simulation (Siemens TIA Portal)
// Mark König, 05/2022
//
// Main form for testing
//

using System;
using System.Windows.Forms;

using System.Threading;
using Siemens.Simatic.Simulation.Runtime;

using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Drawing;

namespace PlcSimAdvSimulator
{
    public partial class frmMain : Form
    {
        private IInstance myInstance;
        private STagInfo[] myData;
        private string PlcName = string.Empty;

        private long aktTicks;

        private Thread tFeedbacks;

        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region read json config

            try
            {
                string json = File.ReadAllText(Application.StartupPath + "\\elements.json");
                List<Dictionary<string, string>> myList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

                foreach (Dictionary<string, string> item in myList)
                {
                    if (item["Control"] == "Settings")
                    {
                        PlcName = item["PLC"];
                    }
                    else if (item["Control"] == "cButton")
                    {
                        cButton t = new cButton();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("Output_Q"))
                            if (!String.IsNullOrEmpty(item["Output_Q"]))
                                t.PlcOutputTag = item["Output_Q"];
                        if (item.ContainsKey("Output_nQ"))
                            if (!String.IsNullOrEmpty(item["Output_nQ"]))
                                t.PlcnOutputTag = item["Output_nQ"];

                        if (item.ContainsKey("ActiveColor"))
                            if (!String.IsNullOrEmpty(item["ActiveColor"]))
                                t.PlcActiveColor = ColorTranslator.FromHtml(item["ActiveColor"]);

                        if (item.ContainsKey("Output_Q"))
                            t.ToolTip = "OUT: " + (string)item["Output_Q"];
                        else if (item.ContainsKey("Output_nQ"))
                            t.ToolTip = "OUT: " + (string)item["Output_nQ"];

                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cToggleButton")
                    {
                        cToggleButton t = new cToggleButton();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("Output_Q"))
                            if (!String.IsNullOrEmpty(item["Output_Q"]))
                                t.PlcOutputTag = item["Output_Q"];
                        if (item.ContainsKey("Output_nQ"))
                            if (!String.IsNullOrEmpty(item["Output_nQ"]))
                                t.PlcnOutputTag = item["Output_nQ"];

                        if (item.ContainsKey("ActiveColor"))
                            if (!String.IsNullOrEmpty(item["ActiveColor"]))
                                t.PlcActiveColor = ColorTranslator.FromHtml(item["ActiveColor"]);

                        if (item.ContainsKey("Value"))
                            if (!String.IsNullOrEmpty(item["Value"]))
                                t.PlcButtonValue = bool.Parse(item["Value"]);

                        if (item.ContainsKey("Output_Q"))
                            t.ToolTip = "OUT: " + (string)item["Output_Q"];
                        else if (item.ContainsKey("Output_nQ"))
                            t.ToolTip = "OUT: " + (string)item["Output_nQ"];

                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cButtonLamp")
                    {
                        cButtonLamp t = new cButtonLamp();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("Output_Q"))
                            if (!String.IsNullOrEmpty(item["Output_Q"]))
                                t.PlcOutputTag = item["Output_Q"];
                        if (item.ContainsKey("Output_nQ"))
                            if (!String.IsNullOrEmpty(item["Output_nQ"]))
                                t.PlcnOutputTag = item["Output_nQ"];

                        if (item.ContainsKey("Lamp"))
                            if (!String.IsNullOrEmpty(item["Lamp"]))
                                t.PlcLampTag = item["Lamp"];

                        if (item.ContainsKey("ActiveColor"))
                            if (!String.IsNullOrEmpty(item["ActiveColor"]))
                                t.PlcActiveColor = ColorTranslator.FromHtml(item["ActiveColor"]);

                        t.ToolTip = "IN: " + (string)item["Lamp"] + " - OUT: " + (string)item["Output_Q"];
                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cLamp")
                    {
                        cLamp t = new cLamp();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("Output_Q"))
                            if (!String.IsNullOrEmpty(item["Output_Q"]))
                                t.PlcOutputTag = item["Output_Q"];
                        if (item.ContainsKey("Output_nQ"))
                            if (!String.IsNullOrEmpty(item["Output_nQ"]))
                                t.PlcnOutputTag = item["Output_nQ"];

                        if (item.ContainsKey("ActiveColor"))
                            if (!String.IsNullOrEmpty(item["ActiveColor"]))
                                t.PlcActiveColor = ColorTranslator.FromHtml(item["ActiveColor"]);

                        t.AutoSize = false;
                        t.BorderStyle = BorderStyle.FixedSingle;
                        t.TextAlign = ContentAlignment.MiddleCenter;
                        if (!String.IsNullOrEmpty(item["Lamp"]))
                            t.PlcLampTag = item["Lamp"];

                        t.ToolTip = "OUT: " + (string)item["Lamp"];

                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cCheckBox")
                    {
                        cCheckBox t = new cCheckBox();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("Output_Q"))
                            if (!String.IsNullOrEmpty(item["Output_Q"]))
                                t.PlcOutputTag = item["Output_Q"];
                        if (item.ContainsKey("Output_nQ"))
                            if (!String.IsNullOrEmpty(item["Output_nQ"]))
                                t.PlcnOutputTag = item["Output_nQ"];

                        if (item.ContainsKey("Value"))
                            if (item.ContainsKey("Value"))
                                t.PlcButtonValue = bool.Parse(item["Value"]);

                        t.ToolTip = "OUT: " + (string)item["Output_Q"];
                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cPulse")
                    {
                        cPulse t = new cPulse();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("Output_Q"))
                            if (!String.IsNullOrEmpty(item["Output_Q"]))
                                t.PlcOutputTag = item["Output_Q"];
                        if (item.ContainsKey("Output_nQ"))
                            if (!String.IsNullOrEmpty(item["Output_nQ"]))
                                t.PlcnOutputTag = item["Output_nQ"];

                        if (item.ContainsKey("TimeMS"))
                            if (!String.IsNullOrEmpty(item["TimeMS"]))
                                t.PlcTimeMS = Int16.Parse(item["TimeMS"]);

                        t.ToolTip = "OUT: " + (string)item["Output_Q"];
                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cTrackBar")
                    {
                        cTrackBar t = new cTrackBar();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("Output"))
                            if (!String.IsNullOrEmpty(item["Output"]))
                                t.PlcOutputTag = item["Output"];

                        if (item.ContainsKey("Max"))
                            if (!String.IsNullOrEmpty(item["Max"]))
                                t.PlcMaxValue = Int16.Parse(item["Max"]);
                        if (item.ContainsKey("Min"))
                            if (!String.IsNullOrEmpty(item["Min"]))
                                t.PlcMinValue = Int16.Parse(item["Min"]);

                        if (item.ContainsKey("Value"))
                            if (!String.IsNullOrEmpty(item["Value"]))
                                t.PlcOutputValue = Int16.Parse(item["Value"]);

                        t.ToolTip = "OUT: " + (string)item["Output"];
                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cLabel")
                    {
                        Label t = new Label();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("FontSize"))
                            t.Font = new System.Drawing.Font("Arial", float.Parse(item["FontSize"]));

                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cIntregrator")
                    {
                        cIntregrator t = new cIntregrator();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("Output"))
                            if (!String.IsNullOrEmpty(item["Output"]))
                                t.PlcActualValueTag = item["Output"];


                        if (item.ContainsKey("Value"))
                            if (!String.IsNullOrEmpty(item["Value"]))
                                t.PlcActualValue = Int16.Parse(item["Value"]);
                        if (item.ContainsKey("Gradiant"))
                            if (!String.IsNullOrEmpty(item["Gradiant"]))
                                t.PlcGradientTag = item["Gradiant"];
                        if (item.ContainsKey("SetPoint"))
                            if (!String.IsNullOrEmpty(item["SetPoint"]))
                                t.PlcSetValueTag = item["SetPoint"];
                        if (item.ContainsKey("Target"))
                            if (!String.IsNullOrEmpty(item["Target"]))
                                t.PlcTargetValueTag = item["Target"];

                        if (item.ContainsKey("Set"))
                            if (!String.IsNullOrEmpty(item["Set"]))
                                t.PlcSetTag = item["Set"];
                        if (item.ContainsKey("Start"))
                            if (!String.IsNullOrEmpty(item["Start"]))
                                t.PlcStartTag = item["Start"];

                        t.ToolTip = "OUT: " + item["Text"];
                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cTableSet")
                    {
                        cTableSet t = new cTableSet();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        t.PlcValueTag = item["Output"];

                        if (item.ContainsKey("Step01"))
                            t.PlcTagStep01 = item["Step01"];
                        if (item.ContainsKey("Step02"))
                            t.PlcTagStep02 = item["Step02"];
                        if (item.ContainsKey("Step03"))
                            t.PlcTagStep03 = item["Step03"];
                        if (item.ContainsKey("Step04"))
                            t.PlcTagStep04 = item["Step04"];
                        if (item.ContainsKey("Step05"))
                            t.PlcTagStep05 = item["Step05"];
                        if (item.ContainsKey("Step06"))
                            t.PlcTagStep06 = item["Step06"];
                        if (item.ContainsKey("Step07"))
                            t.PlcTagStep07 = item["Step07"];
                        if (item.ContainsKey("Step08"))
                            t.PlcTagStep08 = item["Step08"];
                        if (item.ContainsKey("Step09"))
                            t.PlcTagStep09 = item["Step09"];
                        if (item.ContainsKey("Step10"))
                            t.PlcTagStep10 = item["Step10"];

                        if (item.ContainsKey("Value01"))
                            t.PlcValueStep01 = Int16.Parse(item["Value01"]);
                        if (item.ContainsKey("Value02"))
                            t.PlcValueStep02 = Int16.Parse(item["Value02"]);
                        if (item.ContainsKey("Value03"))
                            t.PlcValueStep03 = Int16.Parse(item["Value03"]);
                        if (item.ContainsKey("Value04"))
                            t.PlcValueStep04 = Int16.Parse(item["Value04"]);
                        if (item.ContainsKey("Value05"))
                            t.PlcValueStep05 = Int16.Parse(item["Value05"]);
                        if (item.ContainsKey("Value06"))
                            t.PlcValueStep06 = Int16.Parse(item["Value06"]);
                        if (item.ContainsKey("Value07"))
                            t.PlcValueStep07 = Int16.Parse(item["Value07"]);
                        if (item.ContainsKey("Value08"))
                            t.PlcValueStep08 = Int16.Parse(item["Value08"]);
                        if (item.ContainsKey("Value09"))
                            t.PlcValueStep09 = Int16.Parse(item["Value09"]);
                        if (item.ContainsKey("Value10"))
                            t.PlcValueStep01 = Int16.Parse(item["Value10"]);

                        t.ToolTip = "OUT: " + item["Text"];
                        this.Controls.Add(t);
                    }
                    else if (item["Control"] == "cInput")
                    {
                        cInput t = new cInput();
                        t.Text = item["Text"];
                        t.Size = GetSize(item["Size"]);
                        t.Location = GetLocation(item["Location"]);

                        if (item.ContainsKey("Output"))
                            if (!String.IsNullOrEmpty(item["Output"]))
                                t.PlcOutputTag = item["Output"];

                        if (item.ContainsKey("Value"))
                            if (!String.IsNullOrEmpty(item["Value"]))
                                t.PlcOutputValue = Int16.Parse(item["Value"]);

                        t.ToolTip = "OUT: " + (string)item["Output"];
                        this.Controls.Add(t);
                    }
                    else
                    {
                        MessageBox.Show("Unknown control: " + item["Control"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading JSON:" + ex.Message + "\n" + ex.StackTrace);
                Application.Exit();
                return;
            }

            #endregion

            try
            {
                //Connect local to PlcSimAdvanced
                Console.WriteLine("Starting simulation");
                myInstance = SimulationRuntimeManager.CreateInterface(PlcName);

                //Update tag list from API
                Console.WriteLine("Tags synchronization");
                myInstance.UpdateTagList();

                // write taglist as xml for test
                //myInstance.CreateConfigurationFile(Application.StartupPath + "\\test.xml");

                // get all vars for test
                Console.WriteLine("Get tag info's");
                myData = myInstance.TagInfos;

                Console.WriteLine("End synchronization - start simulator");
                //Start a thread to synchronize feedbacks inputs 
                tFeedbacks = new Thread(() => synchroFeedbacks(myInstance));
                tFeedbacks.IsBackground = true;
                tFeedbacks.Start();
            }
            catch
            {
                MessageBox.Show("Could not start PLC instance " + PlcName);
                Application.Exit();
            }
        }

        private System.Drawing.Size GetSize(string value)
        {
            return new System.Drawing.Size(Convert.ToInt16(value.Split('x')[0]), Convert.ToInt16(value.Split('x')[1]));
        }
        private System.Drawing.Point GetLocation(string value)
        {
            return new System.Drawing.Point(Convert.ToInt16(value.Split(',')[0]), Convert.ToInt16(value.Split(',')[1]));
        }

        private void synchroFeedbacks(IInstance myInstance)
        {
            while (true)
            {
                // actual system time
                aktTicks = myInstance.SystemTime.Ticks;

                try
                {
                    // enumerate through controls
                    foreach (Control crtl in this.Controls)
                    {
                        #region work on controls

                        if (crtl is cButton)
                        {
                            cButton c = (cButton)crtl;
                            if (!String.IsNullOrEmpty(c.PlcOutputTag))
                                myInstance.WriteBool(c.PlcOutputTag, c.PlcButtonValue);
                            if (!String.IsNullOrEmpty(c.PlcnOutputTag))
                                myInstance.WriteBool(c.PlcnOutputTag, !c.PlcButtonValue);
                        }
                        else if (crtl is cToggleButton)
                        {
                            cToggleButton c = (cToggleButton)crtl;
                            if (!String.IsNullOrEmpty(c.PlcOutputTag))
                                myInstance.WriteBool(c.PlcOutputTag, c.PlcButtonValue);
                            if (!String.IsNullOrEmpty(c.PlcnOutputTag))
                                myInstance.WriteBool(c.PlcnOutputTag, !c.PlcButtonValue);
                        }
                        else if (crtl is cCheckBox)
                        {
                            cCheckBox c = (cCheckBox)crtl;
                            if (!String.IsNullOrEmpty(c.PlcOutputTag))
                                myInstance.WriteBool(c.PlcOutputTag, c.PlcButtonValue);
                            if (!String.IsNullOrEmpty(c.PlcnOutputTag))
                                myInstance.WriteBool(c.PlcnOutputTag, !c.PlcButtonValue);
                        }
                        else if (crtl is cLamp)
                        {
                            cLamp c = (cLamp)crtl;
                            if (!String.IsNullOrEmpty(c.PlcLampTag))
                                c.PlcLampValue = myInstance.ReadBool(c.PlcLampTag);
                            if (!String.IsNullOrEmpty(c.PlcOutputTag))
                                myInstance.WriteBool(c.PlcOutputTag, c.PlcLampValue);
                            if (!String.IsNullOrEmpty(c.PlcnOutputTag))
                                myInstance.WriteBool(c.PlcnOutputTag, !c.PlcLampValue);
                        }
                        else if (crtl is cButtonLamp)
                        {
                            cButtonLamp c = (cButtonLamp)crtl;
                            // input
                            if (c.PlcOutputTag != null)
                            {
                                myInstance.WriteBool(c.PlcOutputTag, c.PlcButtonValue);
                            }
                            // output
                            if (!String.IsNullOrEmpty(c.PlcLampTag))
                            {
                                c.PlcLampValue = myInstance.ReadBool(c.PlcLampTag);
                            }

                            if (!String.IsNullOrEmpty(c.PlcOutputTag))
                                myInstance.WriteBool(c.PlcOutputTag, c.PlcButtonValue);
                            if (!String.IsNullOrEmpty(c.PlcnOutputTag))
                                myInstance.WriteBool(c.PlcnOutputTag, !c.PlcButtonValue);
                        }
                        else if (crtl is cPulse)
                        {
                            cPulse c = (cPulse)crtl;
                            c.PlcTicks = aktTicks;

                            if (!String.IsNullOrEmpty(c.PlcOutputTag))
                                myInstance.WriteBool(c.PlcOutputTag, c.PlcOutputValue);
                            if (!String.IsNullOrEmpty(c.PlcnOutputTag))
                                myInstance.WriteBool(c.PlcnOutputTag, !c.PlcOutputValue);
                        }
                        else if (crtl is cTrackBar)
                        {
                            cTrackBar c = (cTrackBar)crtl;

                            if (c.PlcOutputTag != null)
                                if (!String.IsNullOrEmpty(c.PlcOutputTag))
                                {
                                    foreach (STagInfo s in myData)
                                    {
                                        if (s.Name == c.PlcOutputTag)
                                        {
                                            if (s.DataType == EDataType.Word)
                                                myInstance.WriteUInt16(c.PlcOutputTag, (UInt16)c.PlcOutputValue);
                                            if (s.DataType == EDataType.Int)
                                                myInstance.WriteInt16(c.PlcOutputTag, (Int16)c.PlcOutputValue);
                                        }
                                    }
                                }
                        }
                        else if (crtl is cIntregrator)
                        {
                            cIntregrator c = (cIntregrator)crtl;
                            c.PlcTicks = aktTicks;

                            if (c.PlcSetValueTag != null)
                                c.PlcSetValue = myInstance.ReadInt32(c.PlcSetValueTag);
                            if (c.PlcTargetValueTag != null)
                                c.PlcTargetValue = myInstance.ReadInt32(c.PlcTargetValueTag);
                            if (c.PlcActualValueTag != null)
                                myInstance.WriteInt16(c.PlcActualValueTag, Convert.ToInt16(c.PlcActualValue));
                            if (c.PlcGradientTag != null)
                                c.PlcGradient = myInstance.ReadInt32(c.PlcGradientTag);

                            if (c.PlcSetTag != null)
                            {
                                bool b = myInstance.ReadBool(c.PlcSetTag);
                                if (b)
                                    c.PlcReset();
                            }
                            if (c.PlcStartTag != null)
                            {
                                bool b = myInstance.ReadBool(c.PlcStartTag);
                                if (b)
                                    c.PlcStart();
                                else
                                    c.PlcStop();
                            }
                        }
                        else if (crtl is cTableSet)
                        {
                            cTableSet c = (cTableSet)crtl;
                            if (c.PlcTagStep01 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep01))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep01));
                            }
                            else if (c.PlcTagStep02 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep02))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep02));
                            }
                            else if (c.PlcTagStep03 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep03))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep03));
                            }
                            else if (c.PlcTagStep04 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep04))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep04));
                            }
                            else if (c.PlcTagStep05 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep05))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep05));
                            }
                            else if (c.PlcTagStep06 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep06))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep06));
                            }
                            else if (c.PlcTagStep07 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep07))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep07));
                            }
                            else if (c.PlcTagStep08 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep08))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep08));
                            }
                            else if (c.PlcTagStep09 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep09))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep09));
                            }
                            else if (c.PlcTagStep10 != null)
                            {
                                if (myInstance.ReadBool(c.PlcTagStep10))
                                    myInstance.WriteUInt16(c.PlcValueTag, Convert.ToUInt16(c.PlcValueStep10));
                            }
                        }
                        else if (crtl is cInput)
                        {
                            cInput c = (cInput)crtl;

                            if (c.PlcOutputTag != null)
                                if (!String.IsNullOrEmpty(c.PlcOutputTag))
                                {
                                    foreach (STagInfo s in myData)
                                    {
                                        if (s.Name == c.PlcOutputTag)
                                        {
                                            if (s.DataType == EDataType.Word)
                                                myInstance.WriteUInt16(c.PlcOutputTag, (UInt16)c.PlcOutputValue);
                                            if (s.DataType == EDataType.Int)
                                                myInstance.WriteInt16(c.PlcOutputTag, (Int16)c.PlcOutputValue);
                                        }
                                    }
                                }
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Exception: " + ex.Message);

                    Console.WriteLine("Restart simulation");
                    myInstance = SimulationRuntimeManager.CreateInterface(PlcName);
                    myInstance.UpdateTagList();


                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // abort reading
            if (tFeedbacks != null)
                tFeedbacks.Abort();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // display system time
            DateTime dt = new DateTime(aktTicks);
            txtTime.Text = "Sytemzeit: " + dt.ToLongTimeString();
        }
    }
}
