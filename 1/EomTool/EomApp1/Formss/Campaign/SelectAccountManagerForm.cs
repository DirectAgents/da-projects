using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EomApp1.Formss.Campaign
{
    public partial class SelectAccountManagerForm : Form
    {
        private bool _initialized;
        public SelectAccountManagerForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Initialize();
        }

        public void Initialize()
        {
            listBox1.ValueMember = "Key";
            listBox1.DisplayMember = "Value";
            GetAccountManagers().ToList().ForEach(accountManager =>
            {
                listBox1.Items.Add(accountManager);
            });
            _initialized = true;
        }

        private IEnumerable<KeyValuePair<int, string>> GetAccountManagers()
        {
            using (var connection = new SqlConnection(global::DAgents.Common.Properties.Settings.Default.ConnStr))
            {
                connection.Open();
                using (var command = new SqlCommand("select * from AccountManager", connection))
                using (var row = command.ExecuteReader())
                {
                    while (row.Read())
                    {
                        yield return new KeyValuePair<int, string>((int)row["id"], (string)row["name"]);
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initialized)
            {
                SelectedAccountManagerId = ((KeyValuePair<int, string>)listBox1.SelectedItem).Key;
                SelectedAccountManagerName = ((KeyValuePair<int, string>)listBox1.SelectedItem).Value;
                Close();
            }
        }

        public int SelectedAccountManagerId { get; set; }

        public string SelectedAccountManagerName { get; set; }
    }
}
