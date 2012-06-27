using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.DBView.Forms
{
    public partial class DBViewForm1 : Form
    {
        public DBViewForm1()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                _launchMenuItem.Visible = false;
            }
        }

        private void DBViewForm1_Load(object sender, EventArgs e)
        {
            //Data.DBViewDataSet.TABLESDataTable dataTable = _tablesAdapter.GetData();
            
            _tablesAdapter.Fill(_dataSet.TABLES);

            var ds = _bindingSource.DataSource as Data.DBViewDataSet;

            if (ds != null)
            {
                string rootNodeName = "Database";
                string tableTypeForView = "VIEW";
                string viewsNodeName = "Views";
                string tableTypeForTable = "BASE TABLE";
                string tablesNodeName = "Tables";

                Dictionary<string, string> dic = new Dictionary<string, string>
                {
                    {tableTypeForView, viewsNodeName},
                    {tableTypeForTable, tablesNodeName}
                };

                _tree.Nodes.Add(rootNodeName);

                var tbl = ds.TABLES;

                TreeNode cursor = _tree.TopNode;
                
                (from c in tbl
                 select c.TABLE_TYPE).Distinct().ToList().ForEach(c => {
                    string name = dic[c];
                    TreeNode node = new TreeNode(name);
                    node.Name = name;
                    cursor.Nodes.Add(node);
                 });

                cursor = cursor.Nodes.Find(viewsNodeName, false)[0];

                (from c in tbl
                 where c.TABLE_TYPE == tableTypeForView
                 orderby c.TABLE_NAME
                 select c.TABLE_NAME).ToList().ForEach(c =>
                 {
                     TreeNode node = new TreeNode(c);
                     node.Name = c;
                     cursor.Nodes.Add(node);
                 });

                cursor = cursor.Parent;
                cursor = cursor.Nodes.Find(tablesNodeName, false)[0];

                (from c in tbl
                 where c.TABLE_TYPE == tableTypeForTable
                 orderby c.TABLE_NAME
                 select c.TABLE_NAME).ToList().ForEach(c =>{
                     TreeNode node = new TreeNode(c);
                     node.Name = c;
                     cursor.Nodes.Add(node);
                 });

                _tree.TopNode.Expand();
            }
            else
            {
                MessageBox.Show("DataSet is null");
            }

            //_bindingSource.
        }

        private void _tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode sn = _tree.SelectedNode;
            
            if (sn.Level >= 2)
            {
                if (sn.FullPath.StartsWith("Database\\Views"))
                {
                    _launchMenuItem.Text = string.Format("Launch {0} View", sn.Name);
                }
                if (sn.FullPath.StartsWith("Database\\Tables"))
                {
                    _launchMenuItem.Text = string.Format("Launch {0} Table", sn.Name);
                }
            }
            
            _launchMenuItem.Visible = (sn.Level >= 2);
        }

        private void launchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode sn = _tree.SelectedNode;

            if (sn.Level >= 2)
            {
                string targetName = sn.Name;

                TableType tt = default(TableType);

                if (sn.FullPath.StartsWith("Database\\Views"))
                {
                    tt = TableType.View;
                }
                if (sn.FullPath.StartsWith("Database\\Tables"))
                {
                    tt = TableType.Table;
                }

                string cs = DAgents.Common.Properties.Settings.Default.ConnStr;

                var a = new DBTableViewerForm1(cs, targetName, tt);

                a.Show();
            }
        }
    }
}
