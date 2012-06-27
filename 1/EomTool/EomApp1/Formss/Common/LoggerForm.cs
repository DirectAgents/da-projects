using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAgents.Common;

namespace EomApp1.Formss.Common
{
    public partial class LoggerForm : Form, ILogger
    {
        public LoggerForm()
        {
            InitializeComponent();
        }

        #region ILogger Members

        public void Log(string message)
        {
            logBox01.Log(message);
        }

        public void LogError(string message)
        {
            logBox01.LogError(message);
        }

        //Signlal _synch = new object();

        public void DoIt(IWin32Window w, Action f)
        {
            Show(w);
            var e = new DoWorkEventArgs(f);
            backgroundWorker1.RunWorkerAsync(e);
        }
        
        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ((Action)(e.Argument))();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
    }
}
