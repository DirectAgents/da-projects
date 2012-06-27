using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomApp1.Formss.Synch.Models.Eom;

namespace EomApp1.Formss.Synch
{
    public partial class DisplayAndSaveChanges : Form
    {
        private EomDatabaseEntities eomEntities;

        public DisplayAndSaveChanges(EomDatabaseEntities eomEntities)
            : this()
        {
            this.eomEntities = eomEntities;
            var objectStateManager = eomEntities.ObjectStateManager;
            var addedObjects = objectStateManager.GetObjectStateEntries(System.Data.EntityState.Added);
            var addedEntites = addedObjects.Select(c => c.Entity).Cast<Item>().ToList();
            this.dataGridView1.DataSource = addedEntites;
        }

        public DisplayAndSaveChanges()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = true;
        }
    }
}
