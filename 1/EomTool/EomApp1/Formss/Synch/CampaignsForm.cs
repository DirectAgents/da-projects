using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace EomApp1.Formss.Synch
{
    public partial class CampaignsForm : Form
    {
        private CampaignSetupItemContainer container = new CampaignSetupItemContainer();

        public CampaignsForm()
        {
            InitializeComponent();
            this.bindingSource1.DataSource = this.container;
            this.Fill();
        }

        private void Fill()
        {
            using (var db = Models.Eom.EomDatabaseEntities.Create())
            {
                GetCampaignSetupItems(db).ToList().ForEach(c =>
                {
                    this.bindingSource1.Add(c);
                });
            }
        }

        private static IQueryable<Models.Eom.Campaign> GetCampaigns(Models.Eom.EomDatabaseEntities db)
        {
            return from c in db.Campaigns
                   where c.TrackingSystem.name == "Cake Marketing" && c.tracking_system_id != null
                   select c;
        }

        private static IQueryable<CampaignSetupItem> GetCampaignSetupItems(Models.Eom.EomDatabaseEntities db)
        {
            return from c in GetCampaigns(db)
                   select new CampaignSetupItem {
                       PID = c.pid,
                       Campaign = c.campaign_name,
                       ExternalId = c.external_id.Value
                   };
        }

        private void saveToolStripButtonClick(object sender, EventArgs e)
        {
            this.Validate();
            this.bindingSource1.EndEdit();
            this.Save();
        }

        private void Save()
        {
            using (var db = Models.Eom.EomDatabaseEntities.Create())
            {
                foreach (var item in from c in GetCampaigns(db).ToList()
                                     from d in this.container.Items
                                     where c.pid == d.PID
                                     select new
                                     {
                                         Campaign = c,
                                         Item = d
                                     })
                {
                    item.Campaign.external_id = item.Item.ExternalId;
                }
                db.SaveChanges();
            }
        }

        private void addItemToolStripClick(object sender, EventArgs e)
        {
            var form = new AddNewCampaignSetupForm();
            form.ShowDialog();
            this.Add(form.OfferId, form.PID);
            this.Clear();
            this.Fill();
        }

        private void Clear()
        {
            this.bindingSource1.Clear();
        }

        private void Add(int offerID, int pid)
        {
            using (var db = Models.Eom.EomDatabaseEntities.Create())
            {
                var campaign = db.Campaigns.First(c => c.pid == pid);
                campaign.external_id = offerID;
                campaign.TrackingSystem = db.TrackingSystems.First(c => c.name == "Cake Marketing");
                db.SaveChanges();
            }
        }
    }

    public class CampaignSetupItemContainer
    {
        private ObservableCollection<CampaignSetupItem> items = new ObservableCollection<CampaignSetupItem>();
        public ObservableCollection<CampaignSetupItem> Items
        {
            get { return items; }
            set { items = value; }
        }
    }

    public class CampaignSetupItem
    {
        public int PID { get; set; }
        public string Campaign { get; set; }
        public int ExternalId { get; set; }
    }
}
