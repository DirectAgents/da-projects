using System;
using System.Data;
using System.Linq;
using AccountingBackupWeb.Models.AccountingBackup;

namespace QuickBooksLibrary
{
    public class QuickBooksCompanyFileQuery : QuickBooksQuery
    {
        public QuickBooksCompanyFileQuery(Company company)
            : base(company)
        {
            this.Company = company;
            InitCompanyFileMustContain();
        }

        void InitCompanyFileMustContain()
        {
            if (Company.Name == "us")
            {
                CompanyFileMustContain = "Direct Agents Inc";
            }
            else if (Company.Name == "intl")
            {
                CompanyFileMustContain = "Direct Agents Interactive Limited";
            }
            else if (Company.Name == "test")
            {
                CompanyFileMustContain = "Aaron";
            }
        }

        public bool CanRun()
        {
            bool canRun = false;
            try
            {
                using (var companyFileNameAdapter = this.GetAdapter("SP_QBFILENAME"))
                {
                    var companyFileNameTable = new DataTable();
                    companyFileNameAdapter.Fill(companyFileNameTable);

                    var companyFileNameRow = companyFileNameTable.AsEnumerable().FirstOrDefault();
                    if (companyFileNameRow != null)
                    {
                        CompanyFile = companyFileNameRow.Field<string>("QBFileName");
                        canRun = ValidateCompanyFile();
                    }
                    else
                    {
                        Error = "cannot determine company file";
                    }
                }
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
            return canRun;
        }

        bool ValidateCompanyFile()
        {
            bool valid = false;
            if (CompanyFile.Contains(CompanyFileMustContain))
            {
                valid = true;
            }
            else
            {
                Error = "wrong company file, expecting to find '"
                    + CompanyFileMustContain + "' in '" + CompanyFile + "'";
            }
            return valid;
        }

        public string GetBestRowId(string tableName)
        {
            string result;

            var bestRowIDs = GetData("Tables", string.Format("SP_SPECIALCOLUMNS {0} BEST_ROWID", tableName)).AsEnumerable();
            if (bestRowIDs.Count() != 1)
            {
                throw new Exception("expecting 1 best row id, got " + bestRowIDs.Count());
            }

            var bestRowID = bestRowIDs.First();
            result = bestRowID.Field<string>("COLUMNNAME");

            return result;
        }

        public DataTable GetAllTables()
        {
            string tablesQuery = "SP_TABLES";

            return GetData("Tables", tablesQuery);
        }

        public DataTable GetColumns(string tableName)
        {
            string tableColumnsQuery = "SP_COLUMNS " + tableName;

            return GetData(tableName, tableColumnsQuery);
        }

        public DataTable GetTable(string tableName, DateTime since)
        {
            string tableQueryFormat = @"SELECT * FROM {0} A WHERE (A.TimeCreated>={ts '{1}'}) ORDER BY A.TimeCreated ";

            string tableQuery = tableQueryFormat
                                    .Replace("{0}", tableName)
                                    .Replace("{1}", since.ToString("yyyy-MM-dd HH:mm:ss"));

            return GetData(tableName, tableQuery);
        }

        public DataTable GetTable(string tableName)
        {
            return GetData(tableName, "SELECT * FROM " + tableName);
        }

        public string CompanyFile { get; set; }

        public string CompanyFileMustContain { get; set; }
    }
}
