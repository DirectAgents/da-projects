using System;
using System.Data;
using System.Data.Odbc;
using AccountingBackupWeb.Models.AccountingBackup;
using DAgents.Common;

namespace QuickBooksLibrary
{
    public class QuickBooksQuery : ProgramObject
    {
        public Company Company { get; set; }
        public string Error { get; set; }

        private string qodbcConnectionString = @"DSN=QuickBooks Data;SERVER=QODBC;OptimizerDBFolder=%UserProfile%\QODBC Driver for QuickBooks\Optimizer;OptimizerAllowDirtyReads=N;SyncFromOtherTables=Y;IAppReadOnly=Y";
        public string QODBCConnectionString
        {
            get { return qodbcConnectionString; }
            set { qodbcConnectionString = value; }
        }

        public QuickBooksQuery(Company company)
        {
            Company = company;
        }

        public DataTable GetData(string query)
        {
            return GetData("QuickBooksData", query);
        }

        public DataTable GetData(string tableName, string query)
        {
            Logger.Log("QuickBooks query: " + query);

            var dataTable = new DataTable(tableName);
            if (!this.TryFill(dataTable, query))
            {
                throw new Exception(this.Error);
            }

            return dataTable;
        }

        public void Fill(DataTable table, string query)
        {
            Logger.Log("QuickBooks query: " + query);

            if (!this.TryFill(table, query))
            {
                throw new Exception(this.Error);
            }
        }

        private bool TryFill(DataTable table, string query)
        {
            bool success = true;
            try
            {
                using (var adapter = GetAdapter(query))
                {
                    adapter.Fill(table);
                }
            }
            catch (Exception e)
            {
                Error = e.Message;
                success = false;
            }
            AddCompanyColumnToTable(table);
            AddTimeStampColumnToTable(table);
            return success;
        }

        protected OdbcDataAdapter GetAdapter(string qodbcQuery)
        {
            return new OdbcDataAdapter(qodbcQuery, this.QODBCConnectionString);
        }

        private void AddCompanyColumnToTable(DataTable table)
        {
            DataColumn companyColumn = table.Columns.Add("Company", typeof(string));
            foreach (var row in table.AsEnumerable())
            {
                row.SetField<string>(companyColumn, Company.Name);
            }
        }

        private void AddTimeStampColumnToTable(DataTable table)
        {
            var now = DateTime.Now;
            DataColumn col = table.Columns.Add("TimeStamp", typeof(DateTime));
            foreach (var row in table.AsEnumerable())
            {
                row.SetField<DateTime>(col, now);
            }
        }
    }
}
