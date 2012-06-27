using System;
using System.Deployment.Application;
using System.IO;
using System.Windows.Forms;
using EomApp1.Formss.PubRep1.Data;
using EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1TableAdapters;
using EomApp1.Formss.PubRep1.Utils;
using EomApp1.Properties;
using EomApp1.Formss.Common;

namespace EomApp1.Formss.PubRep1.Controls
{
    public partial class Report2 : UserControl
    {

        private string _publisher;
        private bool _payMode;

        public Report2()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;
        }

        public void Initialize()
        {
            splitContainer1.Panel2Collapsed = true;
        }

        public string Publisher
        {
            set
            {
                var adapter = new CampaignsPublisherReportDetailsTableAdapter();
                var pr = new PubRepTemplate();
                var data = adapter.GetData(value);
                pr.Data = data;
                bindingSource1.DataSource = data;
                pr.Publisher = value;
                pr.FromDate = new DateTime(
                                Properties.Settings.Default.StatsYear,
                                Properties.Settings.Default.StatsMonth,
                                1);
                pr.ToDate = new DateTime(
                                Properties.Settings.Default.StatsYear,
                                Properties.Settings.Default.StatsMonth,
                                Properties.Settings.Default.StatsDaysInMonth);
                string text = pr.TransformText();
                webBrowser1.DocumentText = text;
                ReportText = text;
                SendToEmail = (new QueriesTableAdapter()).EmailFromAffiliateAddCodeScalarQuery(pr.Data[0].AddCode);
                SentStatus = (new QueriesTableAdapter()).PubReportSentStatusFromVendorScalarQuery(value);
                _publisher = value;
            }
        }

        public string SendToEmail
        {
            get
            {
                return _sendToEmail.Text;
            }
            set
            {
                _sendToEmail.Text = value;
            }
        }

        public string ReportText { get; set; }

        public string ReportSavePath
        {
            get
            {
                string s = @"\pub_report_" + _publisher + "_" + DateTime.Now.Ticks;
                return Application.UserAppDataPath + s;
            }
        }

        public string SentStatus
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value ?? "unsent";
            }
        }

        public bool PayMode
        {
            get
            {
                return _payMode;
            }
            set
            {
                _payMode = value;
            }
        }

        // Email
        void _emailButton_Click(object sender, EventArgs e)
        {
            if (_emailButton.Text.Length < 1)
            {
                MessageBox.Show("Missing email address");
                return;
            }
            try
            {
                _emailButton.Enabled = false;

                EmailUtility.SendNotificationEmail(
                    SendToEmail,
                    Properties.Settings.Default.PubReportSubjectLine,
                    ReportText,
                    Save()
                );
            }
            finally
            {
                _emailButton.Enabled = true;
            }
        }

        int Save()
        {
            //File.WriteAllText(ReportSavePath, ReportText);
            Data.PRDataDataContext db = new Data.PRDataDataContext(DAgents.Common.Properties.Settings.Default.ConnStr);
            var o = new PubReportInstance
            {
                created_by_user_name = DAgents.Common.Utilities.GetWindowsIdentityNameLower(),
                path_to_hard_copy = ReportSavePath,
                report_content = ReportText,
                vendor_id = Data.Vendor.GetOrCreate(_publisher),
                email_status_msg = "unsent",
                saved = DateTime.Now
            };
            db.PubReportInstances.InsertOnSubmit(o);
            db.SubmitChanges();
            Publisher = _publisher; // refresh report
            return o.id;
        }

        // NOTE: This is called from javascript in the embedded browser so don't change the signature here without changing it there.
        public void PayItems(string message)
        {
            var db = new PRDataDataContext();
            var query = "update Item set item_accounting_status_id=5 where id in (" + message + ")";
            db.ExecuteCommand(query);

            //if (RefreshData != null) RefreshData(this, EventArgs.Empty);
            //DoGenerateReport(true);
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            webBrowser1.Print();
        }

        private void print2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }

        private void print3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPageSetupDialog();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
        }

        // Email w/Message button
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            bool b = false;
            var x = new
            {
                f = new Form
                {
                    Text = "Message",
                    Width = 300,
                    Height = 350
                },
                tb = new TextBox
                {
                    Multiline = true,
                    Height = 275,
                    Dock = DockStyle.Top
                },
                bt = new Button
                {
                    Text = "Send",
                    Height = 25,
                    Dock = DockStyle.Top,
                }
            };
            x.f.Controls.Add(x.bt);
            x.f.Controls.Add(x.tb);
            x.bt.Click += (z1, z2) => { b = true; x.f.Close(); };
            x.f.ShowDialog();
            if (b) MessageBox.Show("vioollla");
        }

        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            webBrowser1.Print();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            splitContainer1.TogglePanel2();
            toolStripButton1.Image = splitContainer1.Panel2Collapsed ?
                global::EomApp1.Properties.Resources.uparr : global::EomApp1.Properties.Resources.dnarr;
        }
    }
    static class MyExt2
    {
        public static void TogglePanel2(this SplitContainer sc)
        {
            sc.Panel2Collapsed = !sc.Panel2Collapsed;
        }
    }
}
