namespace EomApp1.Formss.Final
{
    partial class StatusMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.campaignStatusAuditTrailViewUserControl1 = new EomApp1.Formss.Final.CampaignStatusAuditTrailViewUserControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // campaignStatusAuditTrailViewUserControl1
            // 
            this.campaignStatusAuditTrailViewUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.campaignStatusAuditTrailViewUserControl1.Location = new System.Drawing.Point(0, 0);
            this.campaignStatusAuditTrailViewUserControl1.Name = "campaignStatusAuditTrailViewUserControl1";
            this.campaignStatusAuditTrailViewUserControl1.Size = new System.Drawing.Size(784, 538);
            this.campaignStatusAuditTrailViewUserControl1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // StatusMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.campaignStatusAuditTrailViewUserControl1);
            this.Name = "StatusMonitor";
            this.Text = "StatusMonitor";
            this.Load += new System.EventHandler(this.StatusMonitor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CampaignStatusAuditTrailViewUserControl campaignStatusAuditTrailViewUserControl1;
        private System.Windows.Forms.Timer timer1;
    }
}