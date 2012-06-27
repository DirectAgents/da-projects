using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAgents.Common;

namespace EomApp1.Formss.Campaign
{
    public partial class LoggerForm : Form
    {
        private Action<ILogger> _action = null;
        public LoggerForm()
        {
            InitializeComponent();
        }
        public LoggerForm(string formTitle, Action<ILogger> action) : this()
        {
            Text = formTitle;
            _action = action;
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            backgroundWorker1.RunWorkerAsync();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_action != null)
            {
                _action(loggerBox1);
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}
