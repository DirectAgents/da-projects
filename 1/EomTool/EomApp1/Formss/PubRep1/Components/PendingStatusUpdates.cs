using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace EomApp1.Formss.PubRep1.Components
{
    public partial class PendingStatusUpdates : Component, IListSource
    { 
        public PendingStatusUpdates()
        {
            InitializeComponent();
        }

        public PendingStatusUpdates(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        #region IListSource Members

        public bool ContainsListCollection
        {
            get { return ((IListSource)publisherReportDataSet11.StatusChangeAction).ContainsListCollection; }
        }

        public System.Collections.IList GetList()
        {
            return ((IListSource)publisherReportDataSet11.StatusChangeAction).GetList();
        }

        #endregion

        public void Add(string publisher, string from, string to, decimal amount)
        {
            // maps name of status in the app to name of status in the DB
            //6	Approved
            //4	Check Cut
            //5	Check Signed and Paid
            //1	default
            //3	Do Not Pay
            //7	Hold
            //2	Payment Due
            //8	Verified
            var map = new Dictionary<string, string>
            {
                {"Unverified", "default"},
                {"Verified", "Verified"},
                {"Approved", "Approved"},  
                {"Paid", "Check Signed and Paid"},              
            };

            var o = itemIDsForStatusChangeTableAdapter1.GetData(publisher, map[from], "Verified");

            if (o.Count != 1)
            {
                throw new Exception("expecting one row");
            }

            publisherReportDataSet11.StatusChangeAction.AddStatusChangeActionRow(
                publisher, 
                from, 
                to, 
                amount,
                o.First().ItemIDs,
                "-", // Saved
                "-" // By
                );

            DataGridView.ClearSelection();
        }

        public void Save(string excludedItemIDs)
        {
            //6	Approved
            //4	Check Cut
            //5	Check Signed and Paid
            //1	default
            //3	Do Not Pay
            //7	Hold
            //2	Payment Due
            //8	Verified
            var map = new Dictionary<string, int>
            {
                {"Unverified", 1},
                {"Verified", 8},
                {"Approved", 6},  
                {"Paid", 5},              
            };
            foreach (var item in publisherReportDataSet11.StatusChangeAction)
            {
                if (item.Saved == "-")
                {
                    int to = map[item.ToStatus];
                    
                    string query;

                    if(string.IsNullOrEmpty(excludedItemIDs))
                    {
                         query = 
                            @"update Item set item_accounting_status_id={0} where id in (" + item.ItemIDs + ")";
                    }
                    else
                    {
                        query = 
                            @"update Item set item_accounting_status_id={0} where id in (" + item.ItemIDs + ")" +
                            @" and not id in (" + excludedItemIDs + ")";
                    }

                    using (var cn = new SqlConnection(Properties.Settings.Default.DADatabaseR1ConnectionString))
                    using (var cm = new SqlCommand(string.Format(query, to), cn))
                    {
                        //MessageBox.Show("will execute sql: " + cm.CommandText);

                        cn.Open();
                        int n = cm.ExecuteNonQuery();
                        cn.Close();

                        //MessageBox.Show(n.ToString() + " rows affected");

                        item.BeginEdit();
                        item.Saved = string.Format("{0} by {1}", DateTime.Now, DAgents.Common.Utilities.GetWindowsIdentityNameLower());
                        item.EndEdit();
                    }
                }
            }
        }

        public DataGridView DataGridView { get; set; }
    }
}
