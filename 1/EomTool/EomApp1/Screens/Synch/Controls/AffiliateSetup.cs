using System;
using System.Collections.ObjectModel;
using System.Linq;
using EomApp1.Screens.Synch.Models.Eom;
using EomAppControls;

namespace EomApp1.Screens.Synch.Controls
{
    public partial class AffiliateSetup : UserControlBase
    {
        private AffiliateItems items = new AffiliateItems();

        public AffiliateSetup()
            : base(() => EomDatabaseEntities.Create())
        {
            InitializeComponent();
            SetupReadOnlyColumns();
            if (base.Running)
            {
                InitializeData();
            }
        }

        #region Initialization
        // --------------------------------------------------------------------

        private void InitializeData()
        {
            this.bindingSource.DataSource = this.items;
            Fill();
        }

        private void SetupReadOnlyColumns()
        {
            this.affiliateDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
        }

        #endregion

        #region Data Operations
        // --------------------------------------------------------------------

        private void Fill()
        {
            UsingDB<EomDatabaseEntities>(db => {
                AffiliateItemsQuery(db).ToList().ForEach(c => {
                    this.bindingSource.Add(c);
                });
            },
            saveChanges: false);
        }

        private void Add(int cakeAffilaiteID, int affiliateID)
        {
            UsingDB<EomDatabaseEntities>(db => {
                var affiliate = db.Affiliates.First(c => c.affid == affiliateID);
                affiliate.external_id = cakeAffilaiteID;
                affiliate.TrackingSystem = db.TrackingSystems.First(c => c.name == "Cake Marketing");
            },
            saveChanges: true);
        }

        private void Save()
        {
            UsingDB<EomDatabaseEntities>(db => {
                (from aff in AffiliatesQuery(db).ToList()
                 from containerItem in this.items.Items
                 where aff.affid == containerItem.Id
                 select new {
                     Affiliate = aff,
                     Item = containerItem
                 }).ToList().ForEach(item => {
                     item.Affiliate.external_id = item.Item.ExternalId;
                 });
            },
            saveChanges: true);
        }

        #endregion

        #region UI Event Handlers
        // --------------------------------------------------------------------

        private void SaveClicked(object sender, EventArgs e)
        {
            Validate();
            this.bindingSource.EndEdit();
            Save();
        }

        private void AddClicked(object sender, EventArgs e)
        {
            var form = new AddAffiliateSetupForm();
            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (form.CreateAffiliateWithSafeId)
                {
                    int newAffiliateID = CreateAffiliateFromCakeWithSafeId(form.CakeAffiliateId);
                    Add(form.CakeAffiliateId, newAffiliateID);
                }
                else
                {
                    Add(form.CakeAffiliateId, form.AffiliateId);
                }
                this.bindingSource.Clear();
                Fill();
            }
        }

        private int CreateAffiliateFromCakeWithSafeId(int cakeAffiliateID)
        {
            int affid = 0;

            UsingDB<EomDatabaseEntities>(db => {
                var affiliate = Models.Eom.Affiliate.CreateFromCakeWithSafeId(db, cakeAffiliateID);
                affid = affiliate.affid;
            }, saveChanges: false);

            return affid;
        }

        #endregion

        #region Queries
        // --------------------------------------------------------------------

        private static IQueryable<AffiliateItem> AffiliateItemsQuery(Models.Eom.EomDatabaseEntities db)
        {
            return from c in AffiliatesQuery(db)
                   select new AffiliateItem {
                       Affiliate = c.name2,
                       Id = c.affid,
                       ExternalId = c.external_id.Value
                   };
        }

        private static IQueryable<Models.Eom.Affiliate> AffiliatesQuery(Models.Eom.EomDatabaseEntities db)
        {
            return from c in db.Affiliates
                   where c.tracking_system_id != null && c.TrackingSystem.name == "Cake Marketing"
                   select c;
        }

        #endregion
    }

    internal class AffiliateItems
    {
        private ObservableCollection<AffiliateItem> items = new ObservableCollection<AffiliateItem>();
        public ObservableCollection<AffiliateItem> Items
        {
            get { return items; }
            set { items = value; }
        }
    }

    internal class AffiliateItem
    {
        public int Id { get; set; }
        public string Affiliate { get; set; }
        public int ExternalId { get; set; }
    }
}
