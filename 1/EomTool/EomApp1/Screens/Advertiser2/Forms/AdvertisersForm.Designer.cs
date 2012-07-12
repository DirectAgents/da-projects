namespace EomApp1.Screens.Advertiser2.Forms
{
    partial class AdvertisersForm
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
            this.advertisers2Control1 = new EomApp1.Screens.Advertiser2.Controls.Advertisers2Control();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // advertisers2Control1
            // 
            this.advertisers2Control1.Dock = System.Windows.Forms.DockStyle.Top;
            this.advertisers2Control1.Location = new System.Drawing.Point(0, 0);
            this.advertisers2Control1.Name = "advertisers2Control1";
            this.advertisers2Control1.Size = new System.Drawing.Size(474, 620);
            this.advertisers2Control1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 627);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Pull from DirectTrack";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AdvertisersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 662);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.advertisers2Control1);
            this.Name = "AdvertisersForm";
            this.ShowIcon = false;
            this.Text = "Advertisers";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Advertisers2Control advertisers2Control1;
        private System.Windows.Forms.Button button1;
    }
}