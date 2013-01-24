using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickBooks.Metadata;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Collections.Generic;

namespace QuickBooks.Test
{
    [TestClass]
    public class Generate
    {
        QBMetaData qb;
        string[] tables;
        StringBuilder sb;
        int indent = 0;

        [TestMethod]
        public void TestMethod1()
        {
            qb = QBMetaData.Create();
            tables = qb.Tables.Select(c => c.TABLENAME).ToArray();
            sb = new StringBuilder();
            Write("#region QB", true);
            Write("public partial class AccountingContext : DbContext", true);
            Write("{", true);
            Indent();
            foreach (var table in qb.Tables.Where(c => tables.Contains(c.TABLENAME)))
            {
                Write("public DbSet<" + table.TABLENAME + "> " + DbSetName(table.TABLENAME) + " { get; set; }", true);
            }
            Outdent();
            Write("}", true);
            foreach (var table in qb.Tables.Where(c => tables.Contains(c.TABLENAME)))
            {
                var columnNameSeenCount = new Dictionary<string, int>();
                int pkCount = 0;
                Write("public partial class ");
                Write(table.TABLENAME, true);
                Write("{", true);
                Indent();
                Write("[Key, Column(Order = " + pkCount + ")]", true);
                pkCount++;
                Write("public int CompanyId { get; set; }", true);
                foreach (var column in qb.Columns.Where(c => c.TABLENAME == table.TABLENAME))
                {
                    Write(AttributesForColumn(column, ref pkCount), true);
                    Write("public ");
                    Write(TypeName(column.TYPENAME));
                    Write(" ");
                    Write(column.COLUMNNAME.Trim());
                    if (columnNameSeenCount.ContainsKey(column.COLUMNNAME))
                    {
                        columnNameSeenCount[column.COLUMNNAME]++;
                        Write(columnNameSeenCount[column.COLUMNNAME].ToString(), false, false);
                    }
                    else
                    {
                        columnNameSeenCount[column.COLUMNNAME] = 0;
                    }
                    Write(" { get; set; }" + RelatesTo(column));
                }
                Outdent();
                Write("}", true);
            }
            Write("#endregion", true);
            string result = sb.ToString();
            System.Windows.Forms.Clipboard.SetText(result);
            Console.WriteLine(result);
        }

        PluralizationService pluralizationService = PluralizationService.CreateService(CultureInfo.CurrentCulture);

        string DbSetName(string name)
        {
            if (pluralizationService.IsSingular(name))
            {
                return pluralizationService.Pluralize(name);
            }
            return name;
        }

        string RelatesTo(QBMetaDataColumn column)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(column.RELATES_TO))
            {
                result += " // Relates To: " + column.RELATES_TO + "\r\n";
                var pair = column.RELATES_TO.Split('.');
                if (pair.Length == 2)
                {
                    if (pair[1] == "ListID")
                    {
                        if (tables.Contains(pair[0]))
                        {
                            result += "public virtual " + pair[0] + " " + 
                                      column.COLUMNNAME.Substring(0, column.COLUMNNAME.IndexOf("ListID")) 
                                      + pair[0] + "{ get; set; }";
                            result += " // Navigation: " + column.TABLENAME + " -> " + pair[0] + "\r\n";
                        }
                    }
                }
            }
            else
            {
                result += "\r\n";
            }
            return result;
        }

        string AttributesForColumn(QBMetaDataColumn column, ref int pkCount)
        {
            string result = string.Empty;
            int numAttributes = 0;

            bool hasLength = column.TYPENAME == "VARCHAR";
            int lenth = -1;
            if (hasLength)
            {
                numAttributes++;
                lenth = column.LENGTH;
            }

            bool isRequired = column.NULLABLE == 0;
            if (isRequired)
                numAttributes++;

            bool isKey = column.COLUMNNAME == "ListID";
            if (isKey)
                numAttributes++;

            if (numAttributes > 0)
            {
                result = "[";
                if (isRequired)
                {
                    result += "Required, ";
                }
                if (hasLength)
                {
                    result += "MaxLength(" + lenth + "), ";
                }
                if (isKey)
                {
                    result += "Key, Column(Order = " + pkCount + "), ";
                    pkCount++;
                }
                result = result.Substring(0, result.Length - 2);
                result += "]";
            }

            return result;
        }

        string TypeName(string typeName)
        {
            switch (typeName)
            {
                case "VARCHAR":
                    return "string";
                case "TIMESTAMP":
                    return "DateTime";
                case "BIT":
                    return "bool";
                case "INTEGER":
                    return "int";
                case "DATE":
                    return "DateTime";
                case "DECIMAL":
                    return "decimal";
                case "DOUBLE":
                    return "double";
                default:
                    return typeName;
            }
        }

        void Indent()
        {
            indent += 2;
        }

        void Outdent()
        {
            indent -= 2;
        }

        void Write(string s, bool cr = false, bool doIndent = true)
        {
            if (string.IsNullOrWhiteSpace(s))
                return;

            if (doIndent)
            {
                for (int i = 0; i < indent; i++)
                {
                    sb.Append(" ");
                }
            }
            sb.Append(s);
            if (cr)
            {
                sb.AppendLine("");
            }
        }
    }
}
