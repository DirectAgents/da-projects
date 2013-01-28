namespace EomApp1.Screens.PubRep1.Forms
{
    partial class PubRep
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.publisherListView1 = new EomApp1.Screens.PubRep1.Controls.PublisherListView();
            this.publisherDetails1 = new EomApp1.Screens.PubRep1.Controls.PublisherDetails.PublisherDetailsUC();
            this.pubReport1 = new EomApp1.Screens.PubRep1.Controls.Report2();
            this.accountingView1 = new EomApp1.Screens.PubRep1.Controls.AccountingView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.accountingView1);
            this.splitContainer1.Size = new System.Drawing.Size(1099, 687);
            this.splitContainer1.SplitterDistance = 527;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pubReport1);
            this.splitContainer2.Size = new System.Drawing.Size(1099, 527);
            this.splitContainer2.SplitterDistance = 603;
            this.splitContainer2.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.publisherListView1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.publisherDetails1);
            this.splitContainer3.Size = new System.Drawing.Size(603, 502);
            this.splitContainer3.SplitterDistance = 408;
            this.splitContainer3.SplitterWidth = 2;
            this.splitContainer3.TabIndex = 4;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 502);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(603, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::EomApp1.Properties.Resources.uparr;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // publisherListView1
            // 
            this.publisherListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.publisherListView1.Location = new System.Drawing.Point(0, 0);
            this.publisherListView1.Margin = new System.Windows.Forms.Padding(0);
            this.publisherListView1.Name = "publisherListView1";
            this.publisherListView1.Size = new System.Drawing.Size(603, 408);
            this.publisherListView1.TabIndex = 1;
            // 
            // publisherDetails1
            // 
            this.publisherDetails1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.publisherDetails1.Location = new System.Drawing.Point(0, 0);
            this.publisherDetails1.Name = "publisherDetails1";
            this.publisherDetails1.Size = new System.Drawing.Size(603, 92);
            this.publisherDetails1.TabIndex = 3;
            // 
            // pubReport1
            // 
            this.pubReport1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pubReport1.Location = new System.Drawing.Point(0, 0);
            this.pubReport1.Name = "pubReport1";
            this.pubReport1.PayMode = false;
            this.pubReport1.ReportText = null;
            this.pubReport1.SendToEmail = "";
            this.pubReport1.SentStatus = "unsent";
            this.pubReport1.Size = new System.Drawing.Size(492, 527);
            this.pubReport1.TabIndex = 0;
            // 
            // accountingView1
            // 
            this.accountingView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountingView1.Location = new System.Drawing.Point(0, 0);
            this.accountingView1.Name = "accountingView1";
            this.accountingView1.Size = new System.Drawing.Size(1099, 156);
            this.accountingView1.TabIndex = 2;
            // 
            // PubRep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 687);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PubRep";
            this.Text = "PubRepForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PubRep_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Controls.PublisherListView publisherListView1;
        private Controls.AccountingView accountingView1;
        private Controls.Report2 pubReport1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Controls.PublisherDetails.PublisherDetailsUC publisherDetails1;
    }
}