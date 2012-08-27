using System;
using System.Windows.Forms;
using DAgents.Common;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public partial class SendMailDialog : Form
    {
        private System.Windows.Forms.BindingSource settingsBindingSource;

        public SendMailDialog(string to, ITransformText bodyTemplate)
        {
            InitializeComponent();
            this.settingsBindingSource = new System.Windows.Forms.BindingSource()
            {
                DataSource = EomAppCommon.EomAppSettings.Settings.GetType()
            };
            this.fromTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsBindingSource, "EomAppSettings_MediaBuyerWorkflow_Email_From", true));
            this.subjectTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsBindingSource, "EomAppSettings_MediaBuyerWorkflow_Email_Subject", true));
            this.settingsBindingSource.AddNew();
            this.To = to;
            this.Body = bodyTemplate.TransformText();
        }

        public string From
        {
            get { return this.fromTextBox.Text; }
            set { this.fromTextBox.Text = value; }
        }

        public string To
        {
            get { return this.toTextBox.Text; }
            set { this.toTextBox.Text = value; }
        }

        public string Subject
        {
            get { return this.subjectTextBox.Text; }
            set { this.subjectTextBox.Text = value; }
        }

        public string Body
        {
            get { return this.bodyContent.InnerHtml; }
            set { this.bodyContent.InnerHtml = value; }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                EmailUtility.SendEmail(this.From, new string[] { this.To }, new string[] { }, this.Subject, this.Body, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending mail: " + ex.Message);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
