namespace EomApp1
{
    partial class CurrencyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurrencyForm));
            System.Windows.Forms.Label currencyLabel;
            System.Windows.Forms.Label mult_to_USDLabel;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataSetForCurrency = new EomApp1.data.DataSetForCurrency();
            this.currencyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.currencyTableAdapter = new EomApp1.data.DataSetForCurrencyTableAdapters.CurrencyTableAdapter();
            this.tableAdapterManager = new EomApp1.data.DataSetForCurrencyTableAdapters.TableAdapterManager();
            this.currencyBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.currencyBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.currencyDataGridView = new System.Windows.Forms.DataGridView();
            this.currencyTextBox = new System.Windows.Forms.TextBox();
            this.mult_to_USDTextBox = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            currencyLabel = new System.Windows.Forms.Label();
            mult_to_USDLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetForCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingNavigator)).BeginInit();
            this.currencyBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currencyDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataSetForCurrency
            // 
            this.dataSetForCurrency.DataSetName = "DataSetForCurrency";
            this.dataSetForCurrency.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // currencyBindingSource
            // 
            this.currencyBindingSource.DataMember = "Currency";
            this.currencyBindingSource.DataSource = this.dataSetForCurrency;
            // 
            // currencyTableAdapter
            // 
            this.currencyTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CurrencyTableAdapter = this.currencyTableAdapter;
            this.tableAdapterManager.UpdateOrder = EomApp1.data.DataSetForCurrencyTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // currencyBindingNavigator
            // 
            this.currencyBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.currencyBindingNavigator.BindingSource = this.currencyBindingSource;
            this.currencyBindingNavigator.CountItem = null;
            this.currencyBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.currencyBindingNavigator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.currencyBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.currencyBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.currencyBindingNavigatorSaveItem});
            this.currencyBindingNavigator.Location = new System.Drawing.Point(0, 205);
            this.currencyBindingNavigator.MoveFirstItem = null;
            this.currencyBindingNavigator.MoveLastItem = null;
            this.currencyBindingNavigator.MoveNextItem = null;
            this.currencyBindingNavigator.MovePreviousItem = null;
            this.currencyBindingNavigator.Name = "currencyBindingNavigator";
            this.currencyBindingNavigator.PositionItem = null;
            this.currencyBindingNavigator.Size = new System.Drawing.Size(285, 25);
            this.currencyBindingNavigator.TabIndex = 0;
            this.currencyBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // currencyBindingNavigatorSaveItem
            // 
            this.currencyBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.currencyBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("currencyBindingNavigatorSaveItem.Image")));
            this.currencyBindingNavigatorSaveItem.Name = "currencyBindingNavigatorSaveItem";
            this.currencyBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.currencyBindingNavigatorSaveItem.Text = "Save Data";
            this.currencyBindingNavigatorSaveItem.Click += new System.EventHandler(this.currencyBindingNavigatorSaveItem_Click);
            // 
            // currencyDataGridView
            // 
            this.currencyDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.currencyDataGridView.AutoGenerateColumns = false;
            this.currencyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.currencyDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.currencyDataGridView.DataSource = this.currencyBindingSource;
            this.currencyDataGridView.Location = new System.Drawing.Point(0, 0);
            this.currencyDataGridView.Name = "currencyDataGridView";
            this.currencyDataGridView.Size = new System.Drawing.Size(285, 123);
            this.currencyDataGridView.TabIndex = 1;
            // 
            // currencyLabel
            // 
            currencyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            currencyLabel.AutoSize = true;
            currencyLabel.Location = new System.Drawing.Point(12, 142);
            currencyLabel.Name = "currencyLabel";
            currencyLabel.Size = new System.Drawing.Size(52, 13);
            currencyLabel.TabIndex = 4;
            currencyLabel.Text = "Currency:";
            // 
            // currencyTextBox
            // 
            this.currencyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currencyTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.currencyBindingSource, "Currency", true));
            this.currencyTextBox.Location = new System.Drawing.Point(86, 139);
            this.currencyTextBox.Name = "currencyTextBox";
            this.currencyTextBox.Size = new System.Drawing.Size(100, 20);
            this.currencyTextBox.TabIndex = 5;
            // 
            // mult_to_USDLabel
            // 
            mult_to_USDLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            mult_to_USDLabel.AutoSize = true;
            mult_to_USDLabel.Location = new System.Drawing.Point(12, 168);
            mult_to_USDLabel.Name = "mult_to_USDLabel";
            mult_to_USDLabel.Size = new System.Drawing.Size(68, 13);
            mult_to_USDLabel.TabIndex = 6;
            mult_to_USDLabel.Text = "Mult to USD:";
            // 
            // mult_to_USDTextBox
            // 
            this.mult_to_USDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mult_to_USDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.currencyBindingSource, "Mult to USD", true));
            this.mult_to_USDTextBox.Location = new System.Drawing.Point(86, 165);
            this.mult_to_USDTextBox.Name = "mult_to_USDTextBox";
            this.mult_to_USDTextBox.Size = new System.Drawing.Size(100, 20);
            this.mult_to_USDTextBox.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Currency";
            this.dataGridViewTextBoxColumn2.HeaderText = "Currency";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 65;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Mult to USD";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn3.HeaderText = "Mult to USD";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // CurrencyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 230);
            this.Controls.Add(currencyLabel);
            this.Controls.Add(this.currencyTextBox);
            this.Controls.Add(mult_to_USDLabel);
            this.Controls.Add(this.mult_to_USDTextBox);
            this.Controls.Add(this.currencyDataGridView);
            this.Controls.Add(this.currencyBindingNavigator);
            this.Name = "CurrencyForm";
            this.ShowIcon = false;
            this.Text = "Currencies";
            this.Load += new System.EventHandler(this.CurrencyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataSetForCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingNavigator)).EndInit();
            this.currencyBindingNavigator.ResumeLayout(false);
            this.currencyBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currencyDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private data.DataSetForCurrency dataSetForCurrency;
        private System.Windows.Forms.BindingSource currencyBindingSource;
        private data.DataSetForCurrencyTableAdapters.CurrencyTableAdapter currencyTableAdapter;
        private data.DataSetForCurrencyTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator currencyBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton currencyBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView currencyDataGridView;
        private System.Windows.Forms.TextBox currencyTextBox;
        private System.Windows.Forms.TextBox mult_to_USDTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}