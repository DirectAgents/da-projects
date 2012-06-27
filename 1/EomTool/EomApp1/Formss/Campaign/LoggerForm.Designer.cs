namespace EomApp1.Formss.Campaign
{
    partial class LoggerForm
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
            this.loggerBox1 = new Mainn.Controls.Logging.LoggerBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // loggerBox1
            // 
            this.loggerBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loggerBox1.Location = new System.Drawing.Point(0, 0);
            this.loggerBox1.Name = "loggerBox1";
            this.loggerBox1.ShowLogMessages = true;
            this.loggerBox1.ShowErrorMessages = true;
            this.loggerBox1.Size = new System.Drawing.Size(496, 436);
            this.loggerBox1.TabIndex = 0;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // LoggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 436);
            this.Controls.Add(this.loggerBox1);
            this.Name = "LoggerForm";
            this.Text = "LoggerForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Mainn.Controls.Logging.LoggerBox loggerBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}