using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAgents.Common;

namespace EomAppControls.Logging
{
    public partial class LoggerDialog : Form, ILogger
    {
        public LoggerDialog()
        {
            InitializeComponent();
        }

        public void Log(string message)
        {
            loggerBox1.Log(message);
        }

        public void LogError(string message)
        {
            throw new NotImplementedException();
        }
    }
}
