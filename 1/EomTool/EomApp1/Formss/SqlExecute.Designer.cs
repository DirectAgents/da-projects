namespace EomApp1.Formss
{
    partial class SqlExecute
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
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.queriesForSqlExecuteDialogBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dAMain1DataSetForSqlExecute = new EomApp1.DAMain1DataSetForSqlExecute();
            this.queriesForSqlExecuteDialogTableAdapter = new EomApp1.DAMain1DataSetForSqlExecuteTableAdapters.QueriesForSqlExecuteDialogTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.queriesForSqlExecuteDialogBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dAMain1DataSetForSqlExecute)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(240, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(104, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(181, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Password";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Font = new System.Drawing.Font("Lucida Console", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Ivory;
            this.richTextBox1.Location = new System.Drawing.Point(0, 46);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(984, 356);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(356, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.queriesForSqlExecuteDialogBindingSource;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(163, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.ValueMember = "query_text";
            // 
            // queriesForSqlExecuteDialogBindingSource
            // 
            this.queriesForSqlExecuteDialogBindingSource.DataMember = "QueriesForSqlExecuteDialog";
            this.queriesForSqlExecuteDialogBindingSource.DataSource = this.dAMain1DataSetForSqlExecute;
            this.queriesForSqlExecuteDialogBindingSource.CurrentChanged += new System.EventHandler(this.queriesForSqlExecuteDialogBindingSource_CurrentChanged);
            // 
            // dAMain1DataSetForSqlExecute
            // 
            this.dAMain1DataSetForSqlExecute.DataSetName = "DAMain1DataSetForSqlExecute";
            this.dAMain1DataSetForSqlExecute.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // queriesForSqlExecuteDialogTableAdapter
            // 
            this.queriesForSqlExecuteDialogTableAdapter.ClearBeforeFill = true;
            // 
            // SqlExecute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 402);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "SqlExecute";
            this.Text = "SqlExecute";
            this.Load += new System.EventHandler(this.SqlExecute_Load);
            ((System.ComponentModel.ISupportInitialize)(this.queriesForSqlExecuteDialogBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dAMain1DataSetForSqlExecute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private DAMain1DataSetForSqlExecute dAMain1DataSetForSqlExecute;
        private System.Windows.Forms.BindingSource queriesForSqlExecuteDialogBindingSource;
        private DAMain1DataSetForSqlExecuteTableAdapters.QueriesForSqlExecuteDialogTableAdapter queriesForSqlExecuteDialogTableAdapter;
    }
}