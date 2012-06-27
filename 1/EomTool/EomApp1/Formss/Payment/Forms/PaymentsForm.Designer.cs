namespace EomApp1.Formss.Payment.Forms
{
    partial class PaymentsForm
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
            this.collapsibleSplitter1 = new EomApp1.UI.CollapsibleSplitter();
            this.advertiserListUC1 = new EomApp1.Formss.Payment.Controls.AdvertiserListUC();
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitter1)).BeginInit();
            this.collapsibleSplitter1.Panel1.SuspendLayout();
            this.collapsibleSplitter1.SuspendLayout();
            this.SuspendLayout();
            // 
            // collapsibleSplitter1
            // 
            this.collapsibleSplitter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapsibleSplitter1.IsVertical = true;
            this.collapsibleSplitter1.Location = new System.Drawing.Point(0, 0);
            this.collapsibleSplitter1.Name = "collapsibleSplitter1";
            // 
            // collapsibleSplitter1.Panel1
            // 
            this.collapsibleSplitter1.Panel1.Controls.Add(this.advertiserListUC1);
            this.collapsibleSplitter1.Size = new System.Drawing.Size(879, 608);
            this.collapsibleSplitter1.SplitterDistance = 293;
            this.collapsibleSplitter1.TabIndex = 0;
            // 
            // advertiserListUC1
            // 
            this.advertiserListUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advertiserListUC1.Location = new System.Drawing.Point(0, 0);
            this.advertiserListUC1.Name = "advertiserListUC1";
            this.advertiserListUC1.Size = new System.Drawing.Size(293, 608);
            this.advertiserListUC1.TabIndex = 2;
            // 
            // PaymentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 608);
            this.Controls.Add(this.collapsibleSplitter1);
            this.Name = "PaymentsForm";
            this.Text = "PaymentsForm1";
            this.Load += new System.EventHandler(this.PaymentsForm_Load);
            this.collapsibleSplitter1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitter1)).EndInit();
            this.collapsibleSplitter1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.CollapsibleSplitter collapsibleSplitter1;
        private Controls.AdvertiserListUC advertiserListUC1;



    }
}