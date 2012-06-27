using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DAgents.Common;

namespace EomApp1.UI
{
    public class AppFormBase : Form
    {
        public AppFormBase()
        {
            InitializeComponent();
            this.ControlAdded += new ControlEventHandler(FormBase_ControlAdded);
            this.Deactivate += new EventHandler(AppFormBase_Deactivate);
            this.FormClosing += new FormClosingEventHandler(AppFormBase_FormClosing);
            //this.Opacity = 0.1d;
        }

        public string SavePropsFileName
        {
            get
            {
                return
                    Path.GetTempPath() +
                    Utilities.MakeLegalFilename(this.Text + "_formloc.xml");
            }
        }

        void AppFormBase_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        void AppFormBase_Deactivate(object sender, EventArgs e)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            // This is important to tell the user which month's database they are working on.
            this.Text += " - " + Properties.Settings.Default.DADatabaseName;

            // This code only takes effect if no border style is set
            // It should be considered EXPERIMENTAL for now...
            if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None)
            {
                this.MouseDown += new MouseEventHandler(AppFormBase_MouseDown);
                this.MouseMove += new MouseEventHandler(AppFormBase_MouseMove);
                this.MouseUp += new MouseEventHandler(AppFormBase_MouseUp);
                this.MovesWithMouseDown = true;
            }

            base.OnLoad(e);
        }

        void AppFormBase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = new Point(e.X, e.Y);
        }

        void AppFormBase_MouseMove(object sender, MouseEventArgs e)
        {
            if (downPoint == Point.Empty)
            {
                return;
            }
            Point location = new Point(
                this.Left + e.X - downPoint.X,
                this.Top + e.Y - downPoint.Y);
            this.Location = location;
        }

        void AppFormBase_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = Point.Empty;
        }

        void FormBase_ControlAdded(object sender, ControlEventArgs e)
        {
            //e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
        }

        void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //FormBase_KeyPress(sender, e);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            //this.fadeTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // fadeTimer
            // 
            //this.fadeTimer.Tick += new System.EventHandler(this.fadeTimer_Tick);
            // 
            // AppFormBase
            // 
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Name = "AppFormBase";
            this.ShowIcon = false;
            //this.Activated += new System.EventHandler(this.AppFormBase_Activated);
            //this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormBase_KeyPress);
            this.ResumeLayout(false);

        }

        //void AppFormBase_Activated(object sender, EventArgs e)
        //{
        //fadeTimer.Enabled = true;
        //}

        private void fadeTimer_Tick(object sender, EventArgs e)
        {
            //if (this.Opacity < 1.0d)
            //{
            //    this.Opacity += 0.2d;
            //}
            //else
            //{
            //    fadeTimer.Enabled = false;
            //}
        }

        private static char[] MagicSequence = { 'a', 'l', 's', 'k' };

        private int magicSequencePosition = 0;

        //private Timer fadeTimer;

        private System.ComponentModel.IContainer components;

        private int magicSequenceLength = MagicSequence.Length;

        private void FormBase_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == MagicSequence[magicSequencePosition])
            {
                magicSequencePosition++;
            }
            else
            {
                magicSequencePosition = 0;
            }

            if (magicSequencePosition == magicSequenceLength)
            {
                magicSequencePosition = 0;
            }
        }

        public bool DrawCustomWindowBorder { get; set; }

        public bool MovesWithMouseDown { get; set; }

        public Point downPoint = Point.Empty;
    }
}
