using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Campaign2
{
    public partial class Campaigns2UserControl : UserControl
    {
        private Campaigns2Entities context;

        public Campaigns2UserControl()
        {
            InitializeComponent();

            if (!this.DesignMode)
            {
                dataGridView1.AutoGenerateColumns = false;

                PopulateCampaigns();
                ChangeColumns();

                this.Disposed += new EventHandler(Campaigns2UserControl_Disposed);
            }
        }

        void Campaigns2UserControl_Disposed(object sender, EventArgs e)
        {
            if (this.context != null)
            {
                context.Dispose();
            }
        }

        private void ChangeColumns()
        {
            dataGridView1.Columns.Add(new DataGridViewComboBoxColumn {
                DataPropertyName = "account_manager_id",
                DataSource = AccountManagersDataSource(),
                DisplayMember = "name",
                HeaderText = "account_manager_id",
                Name = "accountmanageridDataGridViewTextBoxColumn",
                Resizable = System.Windows.Forms.DataGridViewTriState.True,
                SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic,
                ValueMember = "id",
            });
        }

        private object AccountManagersDataSource()
        {
            if (context != null)
            {
                return context.AccountManager.ToList();
            }
            else
            {
                return null;
            }
        }

        private void PopulateCampaigns()
        {
            context = new Campaigns2Entities();
            var query = context.Campaign;
            var campaigns = query.ToList();
            dataGridView1.DataSource = campaigns;
        }
    }
}
