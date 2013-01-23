using AccountingBackupWeb.Models.AccountingBackup;
using DAgents.Common;
using QuickBooksLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace QuickBooksService
{
    public class QuickBooksSchemaLoader : ProgramAction
    {
        readonly QuickBooksCompanyFileQuery _quickBooksQuery;

        public QuickBooksSchemaLoader(
            QuickBooksCompanyFileQuery quickBooksQuery
        )
        {
            _quickBooksQuery = quickBooksQuery;
        }

        public override void Execute()
        {
            LoadTables();
            LoadColumns();
        }

        void LoadTables()
        {
            XDocument tablesXDocument = QuickBooksTablesAsXDocument();
            if (tablesXDocument != null)
            {
                using (var accountingBackupModel = new AccountingBackupEntities())
                {
                    foreach (var tableXElement in tablesXDocument.Root.Elements("Tables"))
                    {
                        Upsert(accountingBackupModel, tableXElement);
                    }
                    accountingBackupModel.SaveChanges();
                }
            }
        }

        private void Upsert(AccountingBackupEntities accountingBackupModel, XElement tableXElement)
        {
            // key fields
            string qualifierName = tableXElement.Element("QUALIFIERNAME").Value;
            string tableName = tableXElement.Element("TABLENAME").Value;

            // query existing
            var quickBooksTableQuery = from c in accountingBackupModel.QuickBooksTables
                                       where
                                          c.QUALIFIERNAME == qualifierName &&
                                          c.TABLENAME == tableName
                                       select c;

            // conditionally create new
            var quickBooksTable = quickBooksTableQuery.FirstOrDefault();
            if (quickBooksTable == null)
            {
                quickBooksTable = new QuickBooksTable {
                    QUALIFIERNAME = qualifierName,
                    TABLENAME = tableName
                };
                accountingBackupModel.QuickBooksTables.AddObject(quickBooksTable);
            }

            // copy fields
            quickBooksTable.TYPENAME = tableXElement.Element("TYPENAME").Value;
            quickBooksTable.REMARKS = tableXElement.Element("QUALIFIERNAME").Value;
            quickBooksTable.DELETEABLE = int.Parse(tableXElement.Element("DELETEABLE").Value);
            quickBooksTable.VOIDABLE = int.Parse(tableXElement.Element("VOIDABLE").Value);
            quickBooksTable.INSERT_ONLY = int.Parse(tableXElement.Element("INSERT_ONLY").Value);
            quickBooksTable.BestRowID = _quickBooksQuery.GetBestRowId(tableName);
        }

        XDocument QuickBooksTablesAsXDocument()
        {
            XDocument tablesXDoc = new XDocument();
            var tables = _quickBooksQuery.GetAllTables();
            using (var writer = tablesXDoc.CreateWriter())
            {
                tables.WriteXml(writer);
            }
            return tablesXDoc;
        }

        void LoadColumns()
        {
            List<QuickBooksTable> quickBooksTables;
            using (var model = new AccountingBackupEntities())
            {
                quickBooksTables = model.QuickBooksTables.ToList();
            }

            foreach (var quickBooksTable in quickBooksTables)
            {
                using (var model = new AccountingBackupEntities())
                {
                    string quickBooksTableName = quickBooksTable.TABLENAME;
                    var quickBooksColumns = _quickBooksQuery.GetColumns(quickBooksTableName);
                    var quickBooksColumnsXDoc = new XDocument();
                    using (var writer = quickBooksColumnsXDoc.CreateWriter())
                    {
                        quickBooksColumns.WriteXml(writer);
                    }

                    foreach (var column in quickBooksColumnsXDoc.Root.Elements(quickBooksTableName))
                    {
                        string QUALIFIERNAME = column.Element("QUALIFIERNAME").Value;
                        string TABLENAME = column.Element("TABLENAME").Value;
                        string COLUMNNAME = column.Element("COLUMNNAME").Value;
                        var q = from c in model.QuickBooksColumns
                                where
                                   c.QUALIFIERNAME == QUALIFIERNAME &&
                                   c.TABLENAME == TABLENAME &&
                                   c.COLUMNNAME == COLUMNNAME
                                select c;
                        var t = q.FirstOrDefault();
                        if (t == null)
                        {
                            t = new QuickBooksColumn {
                                QUALIFIERNAME = QUALIFIERNAME,
                                TABLENAME = TABLENAME,
                                COLUMNNAME = COLUMNNAME
                            };
                            model.QuickBooksColumns.AddObject(t);
                        }
                        t.TYPE = column.Element("COLUMNNAME").Value;
                        t.TYPENAME = column.Element("TYPENAME").Value;
                        t.PRECISION = column.Element("PRECISION").Value;
                        t.LENGTH = column.Element("LENGTH").Value;
                        t.SCALE = column.Element("SCALE").Value;
                        t.NULLABLE = column.Element("NULLABLE").Value;
                        t.DEFAULT = column.Element("DEFAULT").Value;
                        t.DATATYPE = column.Element("DATATYPE").Value;
                        t.ORDINAL_POSITION = column.Element("ORDINAL_POSITION").Value;
                        t.QUERYABLE = column.Element("QUERYABLE").Value;
                        t.UPDATEABLE = column.Element("UPDATEABLE").Value;
                        t.INSERTABLE = column.Element("INSERTABLE").Value;
                        t.REQUIRED_ON_INSERT = column.Element("REQUIRED_ON_INSERT").Value;
                        t.FORMAT = column.Element("FORMAT").Value;
                        t.RELATES_TO = column.Element("RELATES_TO").Value;
                        t.JUMPIN_TYPE = column.Element("JUMPIN_TYPE").Value;
                        t.FORCE_UNOPTIMIZED = column.Element("FORCE_UNOPTIMIZED").Value;
                        t.ADVANCED = column.Element("ADVANCED").Value;
                        t.SDK_QB_NAME = column.Element("SDK_QB_NAME") != null ? column.Element("SDK_QB_NAME").Value : "unspecified";
                        t.COLUMNFULLNAME = column.Element("COLUMNFULLNAME").Value;
                        t.COLUMNSHORTNAME = column.Element("COLUMNSHORTNAME").Value;
                        t.QuickBooksTable = model.QuickBooksTables.First(c => c.Id == quickBooksTable.Id);
                    }
                    model.SaveChanges();
                }
            }
        }
    }
}
