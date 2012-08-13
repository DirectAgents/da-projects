namespace EomApp1.Screens.Extra
{
    partial class ExtraItemsForm
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.accountManagerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accountManagersDataSet = new EomApp1.Screens.Extra.AccountManagersDataSet();
            this.extraItemsUserControl = new EomApp1.Screens.Extra.ExtraItemsUserControl();
            this.accountManagerTableAdapter = new EomApp1.Screens.Extra.AccountManagersDataSetTableAdapters.AccountManagerTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagersDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.accountManagerBindingSource;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(287, 1);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(89, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // accountManagerBindingSource
            // 
            this.accountManagerBindingSource.DataMember = "AccountManager";
            this.accountManagerBindingSource.DataSource = this.accountManagersDataSet;
            this.accountManagerBindingSource.Filter = "";
            // 
            // accountManagersDataSet
            // 
            this.accountManagersDataSet.DataSetName = "AccountManagersForExtraItemsDataSet";
            this.accountManagersDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // extraItemsUserControl
            // 
            this.extraItemsUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extraItemsUserControl.Location = new System.Drawing.Point(0, 0);
            this.extraItemsUserControl.Name = "extraItemsUserControl";
            this.extraItemsUserControl.Size = new System.Drawing.Size(920, 627);
            this.extraItemsUserControl.TabIndex = 0;
            // 
            // accountManagerTableAdapter
            // 
            this.accountManagerTableAdapter.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::EomApp1.Properties.Resources.Refresh1;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Location = new System.Drawing.Point(382, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 21);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.RefreshButtonClicked);
            // 
            // ExtraItemsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 627);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.extraItemsUserControl);
            this.Name = "ExtraItemsForm";
            this.ShowIcon = false;
            this.Text = "Extra Items";
            this.Load += new System.EventHandler(this.FormLoaded);
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagersDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtraItemsUserControl extraItemsUserControl;
        private System.Windows.Forms.ComboBox comboBox1;
        private AccountManagersDataSet accountManagersDataSet;
        private System.Windows.Forms.BindingSource accountManagerBindingSource;
        private AccountManagersDataSetTableAdapters.AccountManagerTableAdapter accountManagerTableAdapter;
        private System.Windows.Forms.Button button1;
    }
}