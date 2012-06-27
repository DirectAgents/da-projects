using System;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Xml.Serialization;
using EomApp1.Formss.PubRep1.Data;
using EomApp1.Formss.PubRep1.Utils;
using EomApp1.Properties;

namespace EomApp1.Formss.PubRep1.Controls
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Report : UserControl
    {
        private Data.PublisherReportDataSet1.VerifiedLineItemsDataTable _lis;
        private Data.PublisherReportDataSet1.AffiliatesHavingReportsRow _cur;

        public event EventHandler ClickedOnEmail;
        public event EventHandler ClickedOnSave;
        public event EventHandler RefreshData;

        public Report()
        {
            InitializeComponent();
            splitContainer1.Panel2Collapsed = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            webBrowser1.ObjectForScripting = this;
        }

        // NOTE: This is called from javascript in the embedded browser so don't change the signature here without changing it there.
        public void PayItems(string message)
        {
            var db = new PRDataDataContext();
            var query = "update Item set item_accounting_status_id=5 where id in (" + message + ")";
            db.ExecuteCommand(query);

            if (RefreshData != null) RefreshData(this, EventArgs.Empty);
            DoGenerateReport(true);
        }

        public string ReportText { get; set; }
        public string ReportTempPath { get; set; }
        public string ReportSavePath { get; set; }

        public void GenerateReport(
            Data.PublisherReportDataSet1.VerifiedLineItemsDataTable lis,
            Data.PublisherReportDataSet1.AffiliatesHavingReportsRow cur,
            bool payInterface)
        {
            this._lis = lis;
            this._cur = cur;

            DoGenerateReport(payInterface);
        }

        public void Email()
        {
            try
            {
                // Disable the button
                emailToolStripMenuItem.Enabled = false;

                if (toolStripTextBox1.Text.Length < 1)
                {
                    MessageBox.Show("Missing email address");
                    return;
                }

                EmailUtility.SendNotificationEmail(
                    toolStripTextBox1.Text,
                    Properties.Settings.Default.PubReportSubjectLine,
                    ReportText,
                    Save());
            }
            finally
            {
                // Enable the button
                emailToolStripMenuItem.Enabled = true;
            }
        }

        // Saving the report makes a record of it in the database for audit purposes
        public int Save()
        {
            // Copy from temp path to Save path
            File.Copy(ReportTempPath, ReportSavePath);

            // Save a new report to the database
            Data.PRDataDataContext db = new Data.PRDataDataContext();
            var pri = new Data.PubReportInstance();
            pri.created_by_user_name = DAgents.Common.Utilities.GetWindowsIdentityNameLower();
            pri.path_to_hard_copy = ReportSavePath;
            pri.report_content = ReportText;
            pri.vendor_id = Data.Vendor.GetOrCreate(_cur.publisher_name);
            pri.email_status_msg = "unsent";
            pri.saved = DateTime.Now;
            db.PubReportInstances.InsertOnSubmit(pri);
            db.SubmitChanges();

            string xmlPath = ReportSavePath + ".xml";
            System.Xml.Serialization.XmlSerializer serializer =
                new XmlSerializer(typeof(PublisherReportDataSet1.VerifiedLineItemsDataTable));
            Stream stream = new FileStream(xmlPath, FileMode.CreateNew);
            serializer.Serialize(stream, _lis);
            stream.Close();

            DoGenerateReport(false);

            return pri.id;
        }

        public void Print()
        {
            webBrowser1.Print();
        }

        void DoGenerateReport(bool payInterface)
        {
            // Get the path to the generated publiser report
            // reflecting the current selection
            ReportTempPath = Utils.Utility.GeneratePublisherReport(_lis, _cur, payInterface);

            // Save the text
            ReportText = File.ReadAllText(ReportTempPath);

            // Initialize the "save to path" (even though it's not there until Save is called)
            try
            {
                ReportSavePath = ApplicationDeployment.CurrentDeployment.DataDirectory
                    + @"\pub_report_" + _cur.publisher_name + "_" + DateTime.Now.Ticks;
            }
            catch
            {
                ReportSavePath = Application.UserAppDataPath
                    + @"\pub_report_" + _cur.publisher_name + "_" + DateTime.Now.Ticks;
            }
            finally
            {
                if (ReportSavePath.Length < 1)
                {
                    MessageBox.Show(Resources.Report_DoGenerateReport_Could_not_initialize_ReportSavePath);
                }
            }

            // Display the report in the WebBrowser control
            webBrowser1.DocumentStream = new MemoryStream(File.ReadAllBytes(ReportTempPath));

            // Fill the saved reports DGV
            pubReportInstanceTableAdapter.Fill(publisherReportDataSet1.PubReportInstance, _cur.publisher_name);

            // Set the email in the menu
            if (_lis.Count > 0)
            {
                toolStripTextBox1.Text = _lis.First().email;
            }

            string s = "unsent";
            if (dataGridView1.RowCount > 0)
            {
                s = (string)dataGridView1["colEmailStatusMsg", 0].Value;
            }

            // Set the email status label
            label1.Text = s;
            label1.Visible = true;
        }

        private void printToolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            Print();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClickedOnSave != null) ClickedOnSave(this, EventArgs.Empty);
        }

        private void toolStripLabel1_Click_1(object sender, EventArgs e)
        {
            bool b = splitContainer1.Panel2Collapsed;

            if (b)
            {
                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
            }
        }

        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClickedOnEmail != null)
            {
                ClickedOnEmail(this, EventArgs.Empty);
            }
        }

        private void toolStripTextBox1_Enter(object sender, EventArgs e)
        {
            this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DoGenerateReport(true);
        }
    }
}
