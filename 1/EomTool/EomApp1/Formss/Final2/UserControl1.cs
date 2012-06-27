using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Final2
{
    public partial class UserControl1 : UserControl
    {
        public void Fill()
        {
            ta.Fill(ds.Campaign);
        }

        public UserControl1()
        {
            InitializeComponent();
            CreateObjects();
            BeginInit();
            Setup();
            EndInit();
        }

        private void CreateObjects()
        {
            bs = new BindingSource(components);
            ds = new Final.FinalizeDataSet1();
            dgv = new DataGridView();
            ta = new Final.FinalizeDataSet1TableAdapters.CampaignTableAdapter();
        }

        private void BeginInit()
        {
            ComponentBeginInit(dgv);
            ComponentBeginInit(bs);
            ComponentBeginInit(ds);
            SuspendLayout();
        }

        private void Setup()
        {
            SetupGrid();
            AddGridColumns();
            dgv.DataSource = bs;
            SetupData();
            Controls.Add(dgv);
        }

        private void SetupData()
        {
            bs.DataMember = "Campaign";
            bs.DataSource = ds;
            bs.Filter = @"Status='Active'";

            ds.DataSetName = "FinalizeDataSet1";
            ds.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
            
            ta.ClearBeforeFill = true;
        }

        private void SetupGrid()
        {
            dgv.Dock = DockStyle.Fill;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoGenerateColumns = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        private void AddGridColumns()
        {
            dgv.Columns.Add(new DataGridViewReadOnlyTextColumn("pid", "PID"));
            dgv.Columns.Add(new DataGridViewReadOnlyTextColumn("Advertiser", "Advertiser"));
            dgv.Columns.Add(new DataGridViewReadOnlyTextFillColumn("campaign_name", "Campaign"));
            dgv.Columns.Add(new DataGridViewReadOnlyCurrencyColumn("Curr", "Curr"));
            dgv.Columns.Add(new DataGridViewReadOnlyMoneyColumn("Revenue", "Revenue"));
            dgv.Columns.Add(new DataGridViewReadOnlyButtonColumn("Final"));
            dgv.Columns.Add(new DataGridViewReadOnlyTextColumn("AM", "AM"));
        }

        private void EndInit()
        {
            ComponentEndInit(dgv);
            ComponentEndInit(bs);
            ComponentEndInit(ds);
            ResumeLayout(false);
        }

        private void ComponentBeginInit(object o)
        {
            ((System.ComponentModel.ISupportInitialize)(o)).BeginInit();
        }

        private void ComponentEndInit(object o)
        {
            ((System.ComponentModel.ISupportInitialize)(o)).EndInit();
        }
        
        private EomApp1.Formss.Final.FinalizeDataSet1TableAdapters.CampaignTableAdapter ta;
        private BindingSource bs;
        private EomApp1.Formss.Final.FinalizeDataSet1 ds;
        private DataGridView dgv;
    }
}
