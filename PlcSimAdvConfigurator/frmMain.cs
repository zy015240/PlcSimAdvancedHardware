﻿using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using System.Collections.Generic;
using System.Text.Json;
using System.IO;

using Microsoft.VisualBasic;

namespace PlcSimAdvConfigurator
{
    public partial class frmMain : Form
    {
        // save the start point of the control
        Point controlPos = new Point();
        // save the start point of the mouse
        Point mousePos = new Point();

        // grid size
        const int snap = 20;
        // move is active
        bool move;

        List<Dictionary<string, string>> myList;
        string controlID = string.Empty;
        string plcName = string.Empty;
        string actID = string.Empty;

        public frmMain()
        {
            InitializeComponent();
        }

        private Size GetSize(string value)
        {
            return new System.Drawing.Size(Convert.ToInt16(value.Split('x')[0]), Convert.ToInt16(value.Split('x')[1]));
        }
        private Point GetLocation(string value)
        {
            return new System.Drawing.Point(Convert.ToInt16(value.Split(',')[0]), Convert.ToInt16(value.Split(',')[1]));
        }

        #region mouse events 

        private void CrtlMouseUp(object sender, MouseEventArgs e)
        {
            if (move)
            {
                Control b = (Control)sender;

                move = false;

                // calc difference
                int x1 = MousePosition.X - mousePos.X;
                int y1 = MousePosition.Y - mousePos.Y;
                // calc new position
                int newX = controlPos.X + x1;
                int newY = controlPos.Y + y1;

                //limit the movement to the panel
                if (newX < 0) newX = 0;
                if (newY < 0) newY = 0;
                if (newX > pMain.Width - b.Width) newX = pMain.Width - b.Width;
                if (newY > pMain.Height - b.Height) newY = pMain.Height - b.Height;

                // calc for grid
                int dX = newX / snap;
                int dy = newY / snap;
                // calc the remaining px
                int rX = newX - dX * snap;
                int rY = newY - dy * snap;

                // re-calc the position
                newX = dX * snap;
                newY = dy * snap;
                // if more the half use next grid point
                if (rX > (snap / 2)) newX += snap;
                if (rY > (snap / 2)) newY += snap;

                b.Location = new Point(newX, newY);

                GetItemByID((string)b.Tag)["Location"] = b.Location.X.ToString() + "," + b.Location.Y.ToString();

            }
        }

        private void CrtlMouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                Control b = (Control)sender;

                // calc difference
                int x1 = MousePosition.X - mousePos.X;
                int y1 = MousePosition.Y - mousePos.Y;
                // calc new position
                int newX = controlPos.X + x1;
                int newY = controlPos.Y + y1;

                //limit the movement to the panel
                if (newX < 0) newX = 0;
                if (newY < 0) newY = 0;
                if (newX > pMain.Width - b.Width) newX = pMain.Width - b.Width;
                if (newY > pMain.Height - b.Height) newY = pMain.Height - b.Height;

                b.Location = new Point(newX, newY);
            }
        }

        private void CrtlMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Control b = (Control)sender;
                controlPos = b.Location;
                mousePos = MousePosition;

                txtItemID.Text = "ItemID: " + (string)b.Tag;
                actID = (string)b.Tag;

                Dictionary<string, string> item = GetItemByID(actID);
                DataTable dt = new DataTable();

                dt.Columns.Add("Key");
                dt.Columns.Add("Value");

                foreach (KeyValuePair<string, string> v in item)
                {
                    DataRow row = dt.NewRow();
                    row[0] = v.Key;
                    row[1] = v.Value;

                    dt.Rows.Add(row);
                }

                dataProperties.DataSource = dt;

                b.BringToFront();

                move = true;
            }
        }

        #endregion

        #region new controls
        private void btnButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> newItem = Defination.getButton();
            Button t = new Button();

            t.Text = "neuer Button";
            newItem["Text"] = t.Text;

            t.Size = GetSize(newItem["Size"]);

            t.Location = new Point(snap, snap);
            newItem["Location"] = t.Location.X.ToString() + "," + t.Location.Y.ToString();

            t.Tag = controlID;
            newItem["ID"] = controlID;
            IncControlID();

            myList.Add(newItem);

            AddControl((Control)t);
        }

        private void btnToggleButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> newItem = Defination.getToggleButton();
            Button t = new Button();

            t.Text = "neuer Toggle Button";
            newItem["Text"] = t.Text;

            t.Size = GetSize(newItem["Size"]);

            t.Location = new Point(snap, snap);
            newItem["Location"] = t.Location.X.ToString() + "," + t.Location.Y.ToString();

            t.Tag = controlID;
            newItem["ID"] = controlID;
            IncControlID();

            myList.Add(newItem);

            AddControl((Control)t);
        }

        private void btnButtonLamp_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> newItem = Defination.getButtonLamp();
            Button t = new Button();

            t.Text = "neue Button - Lamp";
            newItem["Text"] = t.Text;

            t.Size = GetSize(newItem["Size"]);

            t.Location = new Point(snap, snap);
            newItem["Location"] = t.Location.X.ToString() + "," + t.Location.Y.ToString();

            t.Tag = controlID;
            newItem["ID"] = controlID;
            IncControlID();

            myList.Add(newItem);

            AddControl((Control)t);
        }

        private void btnLamp_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> newItem = Defination.getLamp();
            Button t = new Button();

            t.Text = "neue Lampe";
            newItem["Text"] = t.Text;

            t.Size = GetSize(newItem["Size"]);

            t.Location = new Point(snap, snap);
            newItem["Location"] = t.Location.X.ToString() + "," + t.Location.Y.ToString();

            t.Tag = controlID;
            newItem["ID"] = controlID;
            IncControlID();

            myList.Add(newItem);

            AddControl((Control)t);
        }

        private void btnCheckBox_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> newItem = Defination.getCheckBox();
            CheckBox t = new CheckBox();

            t.Text = "neue Chekcbox";
            newItem["Text"] = t.Text;

            t.Size = GetSize(newItem["Size"]);

            t.Location = new Point(snap, snap);
            newItem["Location"] = t.Location.X.ToString() + "," + t.Location.Y.ToString();

            t.Tag = controlID;
            newItem["ID"] = controlID;
            IncControlID();

            myList.Add(newItem);

            AddControl((Control)t);
        }

        #endregion

        private void IncControlID()
        {
            int nextID = int.Parse(controlID) + 1;
            controlID = nextID.ToString();
            myList[0]["ID"] = controlID;
        }

        private void AddControl(Control crtl)
        {
            crtl.MouseDown += CrtlMouseDown;
            crtl.MouseMove += CrtlMouseMove;
            crtl.MouseUp += CrtlMouseUp;

            pMain.Controls.Add(crtl);
            crtl.BringToFront();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            string json = JsonSerializer.Serialize<List<Dictionary<string, string>>>(myList, options);
            File.WriteAllText(Application.StartupPath + "\\elementsNeu.json", json, System.Text.Encoding.UTF8);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (actID != string.Empty)
            {
                DialogResult res = MessageBox.Show("Really delete item ?", "Delete item", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    DeleteItemByID(actID);
                }
            }
        }

        private Dictionary<string, string> GetItemByID(string id)
        {
            foreach (Dictionary<string, string> item in myList)
            {
                if (item["ID"] == id) return item;
            }
            return null;
        }

        private void DeleteItemByID(string id)
        {
            for (int i = 0; i < myList.Count; i++)
            {
                if (myList[i]["ID"] == id)
                {
                    myList.RemoveAt(i);
                    foreach (Control c in pMain.Controls)
                    {
                        if ((string)c.Tag == id)
                        {
                            pMain.Controls.Remove(c);

                            txtItemID.Text = string.Empty;
                            actID = string.Empty;

                            return;
                        }
                    }
                }
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            string json = File.ReadAllText(Application.StartupPath + "\\elements.json");
            myList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

            foreach (Dictionary<string, string> item in myList)
            {
                if (item["Control"] == "Settings")
                {
                    plcName = item["PLC"];
                    controlID = item["ID"];
                    this.Text = "Actual PLC: " + plcName;
                }
                else if (item["Control"] == "cButton")
                {
                    Button t = new Button();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
                else if (item["Control"] == "cToggleButton")
                {
                    Button t = new Button();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
                else if (item["Control"] == "cButtonLamp")
                {
                    Button t = new Button();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
                else if (item["Control"] == "cLamp")
                {
                    Button t = new Button();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
                else if (item["Control"] == "cCheckBox")
                {
                    CheckBox t = new CheckBox();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
                else if (item["Control"] == "cPulse")
                {
                    Button t = new Button();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
                else if (item["Control"] == "cTrackbar")
                {
                    TrackBar t = new TrackBar();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
                else if (item["Control"] == "cLabel")
                {
                    Label t = new Label();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);
                    t.BorderStyle = BorderStyle.FixedSingle;

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
                else if (item["Control"] == "cIntregrator")
                {
                    Button t = new Button();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
                else if (item["Control"] == "cTableSet")
                {
                    Button t = new Button();
                    t.Text = item["Text"];
                    t.Size = GetSize(item["Size"]);
                    t.Location = GetLocation(item["Location"]);

                    t.MouseDown += CrtlMouseDown;
                    t.MouseMove += CrtlMouseMove;
                    t.MouseUp += CrtlMouseUp;

                    t.Tag = item["ID"];

                    pMain.Controls.Add(t);
                }
            }
        }

        private void dataProperties_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string key = (string)dataProperties.Rows[e.RowIndex].Cells[0].Value;
            string val = (string)dataProperties.Rows[e.RowIndex].Cells[1].Value;

            if (key == "Text")
            {
                string input = Interaction.InputBox("Prompt", "Neuer Text", val, 10, 10);

                if ((input != val) && (input.Length > 0))
                {
                    GetItemByID(actID)["Text"] = input;

                    foreach (Control c in pMain.Controls)
                    {
                        if ((string)c.Tag == actID)
                        {
                            c.Text = input;
                            dataProperties.Rows[e.RowIndex].Cells[1].Value = input;
                            return;
                        }
                    }
                }
            }
            if (key == "Size")
            {
                string input = Interaction.InputBox("Prompt", "Neue Grösse", val, 10, 10);

                if ((input != val) && (input.Length > 0))
                {
                    try
                    {
                        Size newSize = GetSize(input);

                        GetItemByID(actID)["Size"] = newSize.Width.ToString() + "x" + newSize.Height.ToString();

                        foreach (Control c in pMain.Controls)
                        {
                            if ((string)c.Tag == actID)
                            {
                                c.Size = newSize;
                                dataProperties.Rows[e.RowIndex].Cells[1].Value = newSize.Width.ToString() + "x" + newSize.Height.ToString();
                                return;
                            }
                        }
                    }
                    catch { }


                }
            }

        }
    }
}
