namespace EomAppControls.Logging
{
    partial class LoggerDialog
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
            this.SuspendLayout();
            // 
            // loggerBox1
            // 
            this.loggerBox1.Location = new System.Drawing.Point(12, 12);
            this.loggerBox1.Name = "loggerBox1";
            this.loggerBox1.ShowErrorMessages = false;
            this.loggerBox1.ShowLogMessages = true;
            this.loggerBox1.Size = new System.Drawing.Size(602, 626);
            this.loggerBox1.TabIndex = 0;
            // 
            // LoggerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 339);
            this.Controls.Add(this.loggerBox1);
            this.Name = "LoggerDialog";
            this.ShowIcon = false;
            this.Text = "Log";
            this.ResumeLayout(false);

        }

        #endregion

        private Mainn.Controls.Logging.LoggerBox loggerBox1;
    }
}