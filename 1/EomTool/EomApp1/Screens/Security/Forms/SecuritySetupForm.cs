using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace EomApp1.Screens.Security.Forms
{
    public partial class SecuritySetupForm : Form
    {
        public SecuritySetupForm()
        {
            InitializeComponent();
        }

        private void SecuritySetupForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.RoleGroup' table. You can move, or remove it, as needed.
            this.roleGroupTableAdapter.Fill(this.dataSet1.RoleGroup);
            // TODO: This line of code loads data into the 'dataSet1.Roles' table. You can move, or remove it, as needed.
            this.rolesTableAdapter.Fill(this.dataSet1.Roles);
            // TODO: This line of code loads data into the 'dataSet1.RolePermission' table. You can move, or remove it, as needed.
            this.rolePermissionTableAdapter.Fill(this.dataSet1.RolePermission);
            // TODO: This line of code loads data into the 'dataSet1.Permissions' table. You can move, or remove it, as needed.
            this.permissionsTableAdapter.Fill(this.dataSet1.Permissions);
        }

        private void permissionsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.permissionsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.permissionsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);
        }
    }
}
