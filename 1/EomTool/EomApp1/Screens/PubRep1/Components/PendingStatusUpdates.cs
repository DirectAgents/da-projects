using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomAppCommon;

namespace EomApp1.Screens.PubRep1.Components
{
    public enum PaymentBatchApprovalStateId
    {
        Default = 1,
        Queued = 2,
        Sent = 3,
        Approved = 4,
        Held = 5
    }

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

        public void Add(string publisher, string fromStatus, string toStatus, decimal amount)
        {
            var existing = publisherReportDataSet11.StatusChangeAction.Where(c => c.Publisher == publisher && c.Amount == amount);
            foreach (var item in existing.ToList())
                item.Delete();

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

            // This query ensures that only verified item IDs are changed
            var o = itemIDsForStatusChangeTableAdapter1.GetData(publisher, map[fromStatus], "Verified");

            if (o.Count != 1)
            {
                throw new Exception("expecting one row");
            }

            if (!publisherReportDataSet11.StatusChangeAction.Any(c => c.Publisher == publisher && c.FromStatus == fromStatus && c.ToStatus == toStatus && c.Amount == amount))
            {
                publisherReportDataSet11.StatusChangeAction.AddStatusChangeActionRow(publisher, fromStatus, toStatus, amount, o.First().ItemIDs, "-", "-");
                DataGridView.ClearSelection();
            }
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
            foreach (var row in publisherReportDataSet11.StatusChangeAction)
            {
                if (row.Saved == "-")
                {
                    int to = map[row.ToStatus];

                    var itemIDs = GetItemIds(row, excludedItemIDs);
                    //var batch = GetBatchForItems(itemIDs);
                    Data.PaymentBatch batch = null;

                    // update item_accounting_status_id and set payment_batch_id
                    StringBuilder query = new StringBuilder("update Item set item_accounting_status_id={0}");
                    if (batch != null)
                        query.Append(", payment_batch_id={1}");

                    query.Append(" where id in (" + row.ItemIDs + ")");

                    if (!string.IsNullOrEmpty(excludedItemIDs))
                        query.Append(" and not id in (" + excludedItemIDs + ")");

                    var args = new List<object>() { to };
                    if (batch != null) args.Add(batch.id);

                    using (var cn = new SqlConnection(Properties.Settings.Default.DADatabaseR1ConnectionString))
                    using (var cm = new SqlCommand(string.Format(query.ToString(), args.ToArray()), cn))
                    {
                        //MessageBox.Show("will execute sql: " + cm.CommandText);

                        cn.Open();
                        int n = cm.ExecuteNonQuery();
                        cn.Close();

                        //MessageBox.Show(n.ToString() + " rows affected");

                        row.BeginEdit();
                        row.Saved = string.Format("{0} by {1}", DateTime.Now, DAgents.Common.WindowsIdentityHelper.GetWindowsIdentityName());
                        row.EndEdit();
                    }
                }
            }
        }

        private int[] GetItemIds(Eom.Common.PublisherReportDataSet1.StatusChangeActionRow row, string excludedItemIDs)
        {
            if (excludedItemIDs == null) excludedItemIDs = "";

            var allIDs = row.ItemIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => int.Parse(id));
            var excludedIDs = excludedItemIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => int.Parse(id));

            return allIDs.Except(excludedIDs).ToArray();
        }

        private Data.PaymentBatch GetBatchForItems(int[] itemIDs)
        {
            // check/create batch
            Data.PRDataDataContext db = new Data.PRDataDataContext(EomAppCommon.EomAppSettings.ConnStr);
            var batch = (from b in db.PaymentBatches
                         where b.is_current
                         orderby b.id descending
                         select b).FirstOrDefault();
            if (batch == null)
            {
                batch = new Data.PaymentBatch()
                {
                    is_current = true,
                    payment_threshold = EomAppSettings.Settings.EomAppSettings_PaymentWorkflow_First_Batch_Threshold
                };
                db.PaymentBatches.InsertOnSubmit(batch);
                db.SubmitChanges();
            }
            // check if will exceed the threshold
            // TODO: handle different currencies!
            if (batch.payment_threshold != null)
            {
                var batchItems = (from i in db.Items
                                  where i.payment_batch_id == batch.id
                                  select i);
                var batchTotal = batchItems.Sum(i => i.total_cost);

                var itemsToUpdate = (from i in db.Items
                                     where itemIDs.Contains(i.id)
                                     select i);
                var totalToUpdate = itemsToUpdate.Sum(i => i.total_cost);

                if (batchTotal + totalToUpdate > batch.payment_threshold.Value)
                {
                    batch = new Data.PaymentBatch()
                    {
                        is_current = true
                    };
                    db.PaymentBatches.InsertOnSubmit(batch);
                    db.SubmitChanges();
                }
            }
            return batch;
        }

        static private LinkedList<string> _StatusList = null;
        static private LinkedList<string> StatusList
        {
            get
            {
                if (_StatusList == null)
                {
                    _StatusList = new LinkedList<string>();
                    _StatusList.AddLast("Unverified");
                    _StatusList.AddLast("Verified");
                    _StatusList.AddLast("Approved");
                    _StatusList.AddLast("Paid");
                }
                return _StatusList;
            }
        }

        public DataGridView DataGridView { get; set; }
    }
}
