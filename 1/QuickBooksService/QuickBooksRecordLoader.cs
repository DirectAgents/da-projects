using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Xml.Linq;
using AccountingBackupWeb.Models.AccountingBackup;
using DAgents.Common;

namespace QuickBooksService
{
    public class Act
    {
        bool _doneOnce = false;
        int _calls = 0;
        public void Once(Action a)
        {
            if (!_doneOnce)
            {
                a();
                _doneOnce = true;
            }
        }
        public void Every(int every, Action<int> a)
        {
            if ((++_calls % every) == 0)
            {
                a(_calls);
            }
        }
    }

    public static class ObjectExtensions
    {
        public static object xGetProperty(this object source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName).GetValue(source, null);
        }

        public static void xSetProperty(this object source, string propertyName, object value)
        {
            source.GetType().GetProperty(propertyName).SetValue(source, value, null);
        }

        public static object xInvoke(this object source, string methodName, params object[] args)
        {
            return source.GetType().GetMethod(methodName).Invoke(source, args);
        }
    }

    public class QuickBooksRecordLoader : ProgramAction
    {
        string _containerElement = "DocumentElement";
        string _inputFile = @"\\ad1\Accounting$\Xml\qb_{0}.xml";
        IEnumerable<XElement> Containers
        {
            get
            {
                var doc = XDocument.Load(string.Format(_inputFile, _company.Name));
                var containers = doc.Root.Elements(_containerElement);
                return containers;
            }
        }
        string _namespace = "AccountingBackupWeb.Models.QuickBooks";
        string _assembly = "AccountingBackupWeb.Models";
        AccountingBackupWeb.Models.QuickBooks.AaronContainer _quickBooksModel;
        Company _company;

        /// <summary>
        /// 
        /// </summary>
        public QuickBooksRecordLoader(Company company)
        {
            _company = company;
        }

        /// <summary>
        /// Creates the database if it does not exist and processes all records.
        /// </summary>
        public override void Execute()
        {
            InitializeDatabase();

            try
            {
                ProcessAll();
            }
            finally
            {
                _quickBooksModel.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void InitializeDatabase()
        {
            _quickBooksModel = new AccountingBackupWeb.Models.QuickBooks.AaronContainer();

            if (!_quickBooksModel.DatabaseExists())
            {
                //Log("database does not exist, creating");

                _quickBooksModel.CreateDatabase();
            }
            else
            {
                //Log("database exists, records will be appended");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void ProcessAll()
        {
            foreach (var container in Containers)
            {
                var act = new Act();
                var elements = container.Elements();
                int elementCount = elements.Count();
                foreach (var element in elements)
                {
                    string name = element.Name.LocalName;
                    act.Once(() =>
                    {
                        //Log("Processing {0} {1}", elementCount, name);
                    });
                    act.Every(50, i =>
                    {
                        //Log("{0}/{1} {2} done, {3} to go", i, elementCount, name, elementCount - i);
                    });
                    ObjectHandle handle = Activator.CreateInstance(_assembly, _namespace + "." + name, null);
                    if (handle != null)
                    {
                        object target = handle.Unwrap();
                        target.xSetProperty("CompanyId", _company.Id);
                        Copy(element, target);
                        _quickBooksModel.xGetProperty(name).xInvoke("AddObject", target);
                    }
                }
                //Log("Saving...");
                _quickBooksModel.SaveChanges();
            }
        }

        /// <summary>
        /// Copies values from elements of source to same-named properties of target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void Copy(XElement source, object target)
        {
            var items = from c in source.Elements()
                        select new
                        {
                            Name = c.Name.LocalName,
                            Value = c.Value
                        };

            foreach (var item in items)
            {
                var property = target.GetType().GetProperty(item.Name);

                if (property != null)
                {
                    property.SetValue(
                        target,
                        Parse(item.Value, property.PropertyType),
                        null);
                }
            }
        }

        /// <summary>
        /// Converts a string to an instance of a type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object Parse(string value, Type type)
        {
            object result;
            try
            {
                if (type.FullName == typeof(Decimal).FullName || type.FullName == typeof(Nullable<Decimal>).FullName)
                {
                    result = Decimal.Parse(value.Trim());
                }
                else if (type.FullName == typeof(Double).FullName || type.FullName == typeof(Nullable<Double>).FullName)
                {
                    result = Double.Parse(value.Trim());
                }
                else if (type.FullName == typeof(DateTime).FullName || type.FullName == typeof(Nullable<DateTime>).FullName)
                {
                    result = DateTime.Parse(value.Trim());
                }
                else if (type.FullName == typeof(Int32).FullName || type.FullName == typeof(Nullable<Int32>).FullName)
                {
                    result = Int32.Parse(value.Trim());
                }
                else if (type.FullName == typeof(Boolean).FullName || type.FullName == typeof(Nullable<Boolean>).FullName)
                {
                    result = Boolean.Parse(value.Trim());
                }
                else
                {
                    result = value;
                }
            }
            catch (Exception e)
            {
                throw new Exception("error parsing " + value, e);
            }
            return result;
        }
    }
}
