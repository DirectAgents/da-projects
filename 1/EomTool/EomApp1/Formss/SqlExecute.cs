using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EomApp1.Formss
{
    public partial class SqlExecute : Form
    {
        public SqlExecute()
        {
            InitializeComponent();
        }

        private void SqlExecute_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dAMain1DataSetForSqlExecute.QueriesForSqlExecuteDialog' table. You can move, or remove it, as needed.
            this.queriesForSqlExecuteDialogTableAdapter.Fill(this.dAMain1DataSetForSqlExecute.QueriesForSqlExecuteDialog);

        }

        private void queriesForSqlExecuteDialogBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            string queryText =               
                ((DAMain1DataSetForSqlExecute.QueriesForSqlExecuteDialogRow)
                (((DataRowView)(queriesForSqlExecuteDialogBindingSource.Current)).Row)).query_text;

            // If the clipboard contains a valid PID of a campaign, automatically insert it into the query.
            string token = "pid=?";
            if (queryText.Contains(token))
            {
                string clipboardText = (string)Clipboard.GetData("Text");
                int pid;
                if (int.TryParse(clipboardText.Trim(), out pid))
                {
                    if (IsValidPid(pid))
                    {
                        queryText = queryText.Replace(token, token.Replace("?", pid.ToString()));
                    }
                }
            }

            richTextBox1.Text = queryText;
        }

        private bool IsValidPid(int pid)
        {
            bool result = false;
            using (var sqlConnection = new SqlConnection(EomAppCommon.Settings.ConnStr))
            using (var sqlCommand = new SqlCommand(string.Format("select count(pid) from Campaign where pid={0}", pid), sqlConnection))
            {
                sqlConnection.Open();
                try
                {
                    int? count = sqlCommand.ExecuteScalar() as int?;
                    if (count != null && count > 0)
                    {
                        result = true;
                    }
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var sqlConnection = new SqlConnection(EomAppCommon.Settings.ConnStr))
            using (var sqlCommand = new SqlCommand(richTextBox1.Text, sqlConnection))
            {
                if (textBox1.Text == "da123")
                {
                    sqlConnection.Open();
                    int i = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    MessageBox.Show(string.Format("{0} rows affected", i));
                }
                else
                {
                    MessageBox.Show("Enter the correct password");
                }
            }
        }
    }
}
