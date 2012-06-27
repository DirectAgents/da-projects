using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using EomApp1.Events;
using EomApp1.Formss.PubRep1.Data;

namespace EomApp1.Formss.PubRep1.Controls
{
    public partial class Publishers : UserControl
    {
        public Publishers()
        {
            InitializeComponent();
        }

        // When the control loads
        private void PublisherListUserControl_Load(object sender, EventArgs e)
        {
            // net terms filter
            publisherReportDataSet1.NetTermType.AddNetTermTypeRow("(Net Terms)", "%");
            netTermTypeTableAdapter.ClearBeforeFill = false;
            netTermTypeTableAdapter.Fill(publisherReportDataSet1.NetTermType);
            netTermTypeBindingSource.Sort = "name ASC";
            netTermTypeBindingSource.PositionChanged += (se1, ev1) => SetPublisherFilter();

            // currency filter
            publisherReportDataSet1.Currency.AddCurrencyRow("(Currency)", "%");
            currencyTableAdapter.ClearBeforeFill = false;
            currencyTableAdapter.Fill(publisherReportDataSet1.Currency);
            currencyBindingSource.Sort = "name ASC";
            currencyBindingSource.PositionChanged += (se2, ev2) => SetPublisherFilter();

            Fill();

            this.trackTypedTextComponent1.BindControl(dataGridView1);
        }

        private void SetPublisherFilter()
        {
            var drv1 = (DataRowView)netTermTypeBindingSource.Current;
            var row1 = (PublisherReportDataSet1.NetTermTypeRow)drv1.Row;
            var drv2 = (DataRowView)currencyBindingSource.Current;
            var row2 = (PublisherReportDataSet1.CurrencyRow)drv2.Row;
            string filterFormat = "{0} LIKE '{1}' AND {2} LIKE '{3}'";
            string filter = string.Format(filterFormat, "NetTerms", row1.filter_string, "CurrName", row2.filter_string);
            affiliatesHavingReportsBindingSource.Filter = filter;
        }

        // Refresh button
        private void button2_Click(object sender, EventArgs e)
        {
            netTermTypeBindingSource.MoveFirst();
            currencyBindingSource.MoveFirst();
            Fill();
        }

        public void Fill()
        {
            affiliatesHavingReportsTableAdapter.Fill(publisherReportDataSet1.AffiliatesHavingReports);
            publisherReportDataSet1.AffiliatesHavingReports.ToList().ForEach(c => c.text_for_col_pay_button = "Pay");
        }

        private void trackTypedTextComponent1_Typing(object sender, EventArgs e)
        {
            affiliatesHavingReportsBindingSource.Filter = "publisher_name Like '%" +
                ((TextEventArgs)e).Text + "%'";
        }

        public event EventHandler PickedARow;
        public event EventHandler ClickedPayOnARow;

        // Give back the name of the publisher that is currently selected in the control
        public string NameOfPickedRow
        {
            get
            {
                var row = dataGridView1.CurrentRow;

                if (row != null)
                {
                    return (string)dataGridView1.CurrentRow.Cells["colName"].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        // Control's internal response to being clicked on
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Send event
            if (PickedARow != null)
            {
                PickedARow(this, EventArgs.Empty);
            }

            if (e.ColumnIndex == dataGridView1.Columns["colTextForPayButton"].Index)
            {
                // Send event
                if (ClickedPayOnARow != null)
                {
                    ClickedPayOnARow(this, EventArgs.Empty);
                }
            }
        }

        public Data.PublisherReportDataSet1.AffiliatesHavingReportsRow CurrentPublisherRow
        {
            get
            {
                return publisherReportDataSet1.AffiliatesHavingReports.First(
                    c => c["publisher_name"].ToString() == this.NameOfPickedRow);
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.affiliatesHavingReportsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.publisherReportDataSet1 = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1();
            this.affiliatesHavingReportsTableAdapter = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1TableAdapters.AffiliatesHavingReportsTableAdapter();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trackTypedTextComponent1 = new EomApp1.Components.TypingTracker(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.netTermTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.currencyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.netTermTypeTableAdapter = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1TableAdapters.NetTermTypeTableAdapter();
            this.currencyTableAdapter = new EomApp1.Formss.PubRep1.Data.PublisherReportDataSet1TableAdapters.CurrencyTableAdapter();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.CurrName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetTerms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTextForPayButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.affiliatesHavingReportsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherReportDataSet1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.netTermTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // affiliatesHavingReportsBindingSource
            // 
            this.affiliatesHavingReportsBindingSource.DataMember = "AffiliatesHavingReports";
            this.affiliatesHavingReportsBindingSource.DataSource = this.publisherReportDataSet1;
            // 
            // publisherReportDataSet1
            // 
            this.publisherReportDataSet1.DataSetName = "PublisherReportDataSet1";
            this.publisherReportDataSet1.EnforceConstraints = false;
            this.publisherReportDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // affiliatesHavingReportsTableAdapter
            // 
            this.affiliatesHavingReportsTableAdapter.ClearBeforeFill = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "publisher_name";
            this.dataGridViewTextBoxColumn1.HeaderText = "publisher_name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // trackTypedTextComponent1
            // 
            this.trackTypedTextComponent1.Tracking += new System.EventHandler(this.trackTypedTextComponent1_Typing);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.comboBox1);
            this.flowLayoutPanel1.Controls.Add(this.comboBox2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(841, 27);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = global::EomApp1.Properties.Resources.button_refresh_01_25by25;
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 27);
            this.button2.TabIndex = 3;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolTip1.SetToolTip(this.button2, "Refresh Data");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboBox1.DataSource = this.netTermTypeBindingSource;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(30, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.ValueMember = "filter_string";
            // 
            // netTermTypeBindingSource
            // 
            this.netTermTypeBindingSource.DataMember = "NetTermType";
            this.netTermTypeBindingSource.DataSource = this.publisherReportDataSet1;
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboBox2.DataSource = this.currencyBindingSource;
            this.comboBox2.DisplayMember = "name";
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(157, 3);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 1;
            this.comboBox2.ValueMember = "filter_string";
            // 
            // currencyBindingSource
            // 
            this.currencyBindingSource.DataMember = "Currency";
            this.currencyBindingSource.DataSource = this.publisherReportDataSet1;
            // 
            // netTermTypeTableAdapter
            // 
            this.netTermTypeTableAdapter.ClearBeforeFill = true;
            // 
            // currencyTableAdapter
            // 
            this.currencyTableAdapter.ClearBeforeFill = true;
            // 
            // CurrName
            // 
            this.CurrName.DataPropertyName = "CurrName";
            this.CurrName.HeaderText = "Curr";
            this.CurrName.Name = "CurrName";
            this.CurrName.ReadOnly = true;
            // 
            // NetTerms
            // 
            this.NetTerms.DataPropertyName = "NetTerms";
            this.NetTerms.HeaderText = "Net Terms";
            this.NetTerms.Name = "NetTerms";
            this.NetTerms.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "publisher_name";
            this.colName.HeaderText = "publisher_name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colTextForPayButton
            // 
            this.colTextForPayButton.DataPropertyName = "text_for_col_pay_button";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.colTextForPayButton.DefaultCellStyle = dataGridViewCellStyle1;
            this.colTextForPayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colTextForPayButton.HeaderText = "Pay";
            this.colTextForPayButton.Name = "colTextForPayButton";
            this.colTextForPayButton.ReadOnly = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Black;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTextForPayButton,
            this.colName,
            this.NetTerms,
            this.CurrName});
            this.dataGridView1.DataSource = this.affiliatesHavingReportsBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(0, 30);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(841, 412);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // Publishers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Publishers";
            this.Size = new System.Drawing.Size(841, 442);
            this.Load += new System.EventHandler(this.PublisherListUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.affiliatesHavingReportsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.publisherReportDataSet1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.netTermTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
        //    base.OnPaint(e);
        //    var gp = new GraphicsPath();
        //    Rectangle cr = flowLayoutPanel1.ClientRectangle;
        //    e.Graphics.AddRectangle(Pens.Red, 0, 0, 20, 20);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
