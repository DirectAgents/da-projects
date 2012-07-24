namespace EomApp1.Screens.Final.Forms
{
    partial class PublishersForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.finalizePublishersView = new EomApp1.Screens.Final.Controls.PublishersView();
            this.verifyPublishersView = new EomApp1.Screens.Final.Controls.PublishersView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(5, 5);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(481, 460);
            this.splitContainer1.SplitterDistance = 215;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.finalizePublishersView);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 215);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "To be Finalized";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.verifyPublishersView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 241);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "To be Verified";
            // 
            // finalizePublishersView
            // 
            this.finalizePublishersView.ActionButtonText = "Finalize";
            this.finalizePublishersView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.finalizePublishersView.Location = new System.Drawing.Point(3, 16);
            this.finalizePublishersView.Name = "finalizePublishersView";
            this.finalizePublishersView.Padding = new System.Windows.Forms.Padding(5);
            this.finalizePublishersView.Size = new System.Drawing.Size(475, 196);
            this.finalizePublishersView.TabIndex = 0;
            // 
            // verifyPublishersView
            // 
            this.verifyPublishersView.ActionButtonText = "Verify";
            this.verifyPublishersView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.verifyPublishersView.Location = new System.Drawing.Point(3, 16);
            this.verifyPublishersView.Name = "verifyPublishersView";
            this.verifyPublishersView.Padding = new System.Windows.Forms.Padding(5);
            this.verifyPublishersView.Size = new System.Drawing.Size(475, 222);
            this.verifyPublishersView.TabIndex = 1;
            // 
            // PublishersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 470);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PublishersForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.Text = "Publishers";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.PublishersView finalizePublishersView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controls.PublishersView verifyPublishersView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}