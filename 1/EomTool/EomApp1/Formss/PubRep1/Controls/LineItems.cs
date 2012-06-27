using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EomApp1.Formss.PubRep1.Controls
{
    public partial class LineItems : UserControl
    {
        public LineItems()
        {
            InitializeComponent();
        }

        internal void FillByName(string name)
        {
            var db = new Data.PRDataDataContext();

            verifiedLineItemsTableAdapter.ClearBeforeFill = true; // adding rows from multiple sources

            // Query against a LINQ to SQL data context
            // The UNION returns a set of affid found in the Affiliate table (the primary table)
            // and the Publisher table (where we map multiple Affilaites to the same logical Publisher)
            foreach (var affid in
                db.ExecuteQuery<int>(@"
                     SELECT     Item.affid           
                       FROM     dbo.Item INNER JOIN
                                dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid
                      WHERE     Affiliate.name={0}
                      UNION 
                     SELECT     affid
                       FROM     dbo.Publisher
                      WHERE     name={0}", name))
            {
                verifiedLineItemsTableAdapter.FillByAffId(
                    publisherReportDataSet1.VerifiedLineItems, affid);

                if (verifiedLineItemsTableAdapter.ClearBeforeFill)
                {
                    verifiedLineItemsTableAdapter.ClearBeforeFill = false;
                }
            }

            // force user to select a row
            dataGridView1.ClearSelection();
        }

        public Data.PublisherReportDataSet1.VerifiedLineItemsDataTable VerifiedLineItemsDataTable
        {
            get
            {
                return publisherReportDataSet1.VerifiedLineItems;
            }
        }

        public void PayItems()
        {
            // get all the line items that have
            //    - accounting status set to default
            //    - the associated campaign status set to Verified
            // aa: the complication of this logic comes from the publisher name mapping to multiple affiliate ids
            // a sql union might work better
            // the idea is that first the dataset is filtered
            // then change the accounting status to signed and paid on line items that
            //      have accounting status default
            //      have the same affiliate id
            //      have campaign status verified
            //      have the same campaign pid

            var lineItemsToPay = from c in publisherReportDataSet1.VerifiedLineItems // LINQ to DataSet
                                 where (c.item_accounting_status_id == 1) // accounting status is Default
                                    && (c.status == "Verified") // campaign status is Verified
                                 select c;

            var db = new Data.PRDataDataContext(); // Context for update

            Dictionary<int, int> campaignStatusMap = db.Campaigns.ToDictionary(c => c.pid, c => c.campaign_status_id); // Map of all campaign statuses

            // Loop over the items that need to be paid
            foreach (var itemToPay in lineItemsToPay)
            {
                if (itemToPay != null)
                {
                    var otherItemsToPay =
                        (
                            from c in db.Items
                            where (c.item_accounting_status_id == 1)  // accounting staus is default
                                  && (c.affid == itemToPay.affid) // matching affiliate id
                                  && (c.pid == itemToPay.pid) // matching campaign pid
                            select c
                        )
                            .ToList();

                    foreach (var otherItemToPay in otherItemsToPay.Where(c => campaignStatusMap[c.pid] == 4)) // Campaign status 4 is Verified
                    {
                        otherItemToPay.item_accounting_status_id = 5; // Accounting staus 5 is 'Check Signed and Paid'
                    }
                }

                itemToPay.item_accounting_status_id = 5;
            }

            //var changes = db.GetChangeSet();

            db.SubmitChanges();
        }

        private void payToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }
    }
}
