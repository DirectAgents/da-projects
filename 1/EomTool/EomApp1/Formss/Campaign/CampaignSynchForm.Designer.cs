namespace EomApp1.Formss.Campaign
{
    partial class CampaignSynchForm
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
            this.campaignSynchView1 = new EomApp1.Formss.Campaign.CampaignSynchView();
            this.SuspendLayout();
            // 
            // campaignSynchView1
            // 
            this.campaignSynchView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.campaignSynchView1.Location = new System.Drawing.Point(0, 0);
            this.campaignSynchView1.Name = "campaignSynchView1";
            this.campaignSynchView1.Size = new System.Drawing.Size(874, 553);
            this.campaignSynchView1.TabIndex = 0;
            // 
            // CampaignSynchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 553);
            this.Controls.Add(this.campaignSynchView1);
            this.Name = "CampaignSynchForm";
            this.ShowIcon = false;
            this.Text = "Auto Synch";
            this.Load += new System.EventHandler(this.CampaignSynchForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CampaignSynchView campaignSynchView1;
    }
}