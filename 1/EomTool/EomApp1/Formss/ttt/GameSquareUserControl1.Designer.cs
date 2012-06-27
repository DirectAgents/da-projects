namespace EomApp1.Formss.ttt
{
    partial class GameSquareUserControl1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonX = new System.Windows.Forms.Button();
            this.buttonO = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.ContextMenuStrip = this.contextMenuStrip1;
            this.panel1.Controls.Add(this.buttonO);
            this.panel1.Controls.Add(this.buttonX);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 100);
            this.panel1.TabIndex = 0;
            this.panel1.MouseEnter += new System.EventHandler(this.panel1_MouseEnter);
            this.panel1.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            // 
            // buttonX
            // 
            this.buttonX.BackgroundImage = global::EomApp1.Properties.Resources.TttX;
            this.buttonX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonX.Location = new System.Drawing.Point(3, 3);
            this.buttonX.Name = "buttonX";
            this.buttonX.Size = new System.Drawing.Size(45, 45);
            this.buttonX.TabIndex = 0;
            this.buttonX.UseVisualStyleBackColor = true;
            this.buttonX.Visible = false;
            // 
            // buttonO
            // 
            this.buttonO.BackgroundImage = global::EomApp1.Properties.Resources.TttO;
            this.buttonO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonO.Location = new System.Drawing.Point(52, 52);
            this.buttonO.Name = "buttonO";
            this.buttonO.Size = new System.Drawing.Size(45, 45);
            this.buttonO.TabIndex = 1;
            this.buttonO.UseVisualStyleBackColor = true;
            this.buttonO.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(128, 70);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.BackgroundImage = global::EomApp1.Properties.Resources.TttX;
            this.toolStripMenuItem1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Tag = "X";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.BackgroundImage = global::EomApp1.Properties.Resources.TttO;
            this.toolStripMenuItem2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Tag = "O";
            // 
            // GameSquareUserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "GameSquareUserControl1";
            this.Size = new System.Drawing.Size(100, 100);
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonO;
        private System.Windows.Forms.Button buttonX;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}
