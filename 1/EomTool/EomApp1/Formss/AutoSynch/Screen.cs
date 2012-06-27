using System;
using System.Linq;
using System.Windows.Forms;

namespace EomApp1.Formss.AutoSynch
{
    public interface IView
    {
        Items.ItemDataTable CampaignSynchItems { set; }
    }
    public partial class Screen : Form, IView
    {       
        #region Primary
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            presenter.Load();
        }
        Items.ItemDataTable IView.CampaignSynchItems
        {
            set
            {
                dataGridView1.AutoGenerateColumns = false;
                gridSource.DataSource = value;
                dataGridView1.DataSource = gridSource;
            }
        }
        #endregion
        #region Secondary
        IPresenter presenter;
        BindingSource gridSource = new BindingSource();
        public Screen()
        {
            InitializeComponent();
        }
        public Screen(Presenter presenter)
            : this()
        {
            this.presenter = presenter;
            this.presenter.View = this;
        } 
        // Filtering Column Headers
        private void dataGridView1_BindingContextChanged(object sender, EventArgs e)
        {
            CampaignNameCol.HeaderCell = new DataGridViewAutoFilter.DataGridViewAutoFilterColumnHeaderCell(CampaignNameCol.HeaderCell);
        }
        #endregion
    }
    public interface IPresenter
    {
        void Load();
        IView View { set; }
    }
    public class Presenter : IPresenter
    {
        #region Primary
        public void Load()
        {
            var data = model.GetItems();
            data.ItemRowChanged += (sender, eventArgs) => model.UpdateItem(eventArgs.Row);
            view.CampaignSynchItems = data;
        }
        #endregion
        #region Secondary
        public Presenter(IModel m)
        {
            model = m;
        }
        public IView View { set { view = value; } } 
        IView view;
        IModel model; 
        #endregion
    }
    public interface IModel
    {
        Items.ItemDataTable GetItems();
        void UpdateItem(Items.ItemRow itemRow);
    }
    public class Model : IModel
    {
        #region Primary
        public Items.ItemDataTable GetItems()
        {
            var table = new Items.ItemDataTable();
            using (var db = DB())
            {
                foreach (var item in
                    from c in db.CampaignSynches
                    select new
                    {
                        id = c.Id,
                        campaignName = c.Campaign.campaign_name,
                        active = c.Active
                    })
                    table.AddItemRow(
                        item.campaignName,
                        item.active,
                        item.id);
            }
            return table;
        }
        public void UpdateItem(Items.ItemRow itemRow)
        {
            using (var db = DB())
            {
                var updateRow = db.CampaignSynches.First(c => c.Id == itemRow.Id);
                updateRow.Active = itemRow.Active;
                db.SaveChanges();
            }
        }
        #endregion
        #region Secondary
        static string cs = @"data source=biz2\da; initial catalog=DADatabaseAug11; integrated security=True; multipleactiveresultsets=True; App=EntityFramework";
        DADatabaseEntities1 DB() { return new DADatabaseEntities1(string.Format(@"metadata=res://*/Formss.AutoSynch.Model1.csdl|res://*/Formss.AutoSynch.Model1.ssdl|res://*/Formss.AutoSynch.Model1.msl;provider=System.Data.SqlClient; provider connection string=""{0}""", cs)); }
        #endregion
    }
}
