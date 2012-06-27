namespace Mainn.Controls.Logging
{
    partial class LoggerBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.primaryRichTextBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.secondaryRichTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // primaryRichTextBox
            // 
            this.primaryRichTextBox.BackColor = System.Drawing.Color.Black;
            this.primaryRichTextBox.ContextMenuStrip = this.contextMenuStrip2;
            this.primaryRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.primaryRichTextBox.ForeColor = System.Drawing.Color.Lime;
            this.primaryRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.primaryRichTextBox.Name = "primaryRichTextBox";
            this.primaryRichTextBox.Size = new System.Drawing.Size(602, 285);
            this.primaryRichTextBox.TabIndex = 1;
            this.primaryRichTextBox.Text = "";
            this.primaryRichTextBox.WordWrap = false;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(102, 26);
            // 
            // clearToolStripMenuItem1
            // 
            this.clearToolStripMenuItem1.Name = "clearToolStripMenuItem1";
            this.clearToolStripMenuItem1.Size = new System.Drawing.Size(101, 22);
            this.clearToolStripMenuItem1.Text = "Clear";
            this.clearToolStripMenuItem1.Click += new System.EventHandler(this.clearToolStripMenuItem1_Click);
            // 
            // secondaryRichTextBox
            // 
            this.secondaryRichTextBox.BackColor = System.Drawing.Color.Black;
            this.secondaryRichTextBox.ContextMenuStrip = this.contextMenuStrip2;
            this.secondaryRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.secondaryRichTextBox.ForeColor = System.Drawing.Color.Lime;
            this.secondaryRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.secondaryRichTextBox.Name = "secondaryRichTextBox";
            this.secondaryRichTextBox.Size = new System.Drawing.Size(602, 288);
            this.secondaryRichTextBox.TabIndex = 2;
            this.secondaryRichTextBox.Text = "";
            this.secondaryRichTextBox.WordWrap = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.primaryRichTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.secondaryRichTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(602, 577);
            this.splitContainer1.SplitterDistance = 285;
            this.splitContainer1.TabIndex = 3;
            // 
            // LogBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "LogBox";
            this.Size = new System.Drawing.Size(602, 577);
            this.Load += new System.EventHandler(this.LogBox_Load);
            this.contextMenuStrip2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox primaryRichTextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem1;
        private System.Windows.Forms.RichTextBox secondaryRichTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
