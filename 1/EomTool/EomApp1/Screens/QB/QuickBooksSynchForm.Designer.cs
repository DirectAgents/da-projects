namespace EomApp1.Screens.QB
{
    partial class QuickBooksSynchForm
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
            this.quickBooksSynchControl1 = new QuickBooks.UI.Controls.QuickBooksSynchControl();
            this.SuspendLayout();
            // 
            // quickBooksSynchControl1
            // 
            this.quickBooksSynchControl1.Location = new System.Drawing.Point(12, 12);
            this.quickBooksSynchControl1.Name = "quickBooksSynchControl1";
            this.quickBooksSynchControl1.Size = new System.Drawing.Size(183, 136);
            this.quickBooksSynchControl1.TabIndex = 0;
            // 
            // QuickBooksSynchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 156);
            this.Controls.Add(this.quickBooksSynchControl1);
            this.Name = "QuickBooksSynchForm";
            this.ShowIcon = false;
            this.Text = "QB Synch";
            this.ResumeLayout(false);

        }

        #endregion

        private QuickBooks.UI.Controls.QuickBooksSynchControl quickBooksSynchControl1;
    }
}