namespace EomApp1.Formss.Final
{
    partial class ConfirmationBox
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
            this.confirmActionUserControl1 = new EomApp1.Formss.Final.ConfirmActionUserControl();
            this.SuspendLayout();
            // 
            // confirmActionUserControl1
            // 
            this.confirmActionUserControl1.Location = new System.Drawing.Point(12, 12);
            this.confirmActionUserControl1.Name = "confirmActionUserControl1";
            this.confirmActionUserControl1.NotesText = "";
            this.confirmActionUserControl1.Size = new System.Drawing.Size(224, 132);
            this.confirmActionUserControl1.TabIndex = 0;
            // 
            // ConfirmationBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 152);
            this.Controls.Add(this.confirmActionUserControl1);
            this.Name = "ConfirmationBox";
            this.Text = "ConfirmationBox";
            this.Load += new System.EventHandler(this.ConfirmActionDialogLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private ConfirmActionUserControl confirmActionUserControl1;
    }
}