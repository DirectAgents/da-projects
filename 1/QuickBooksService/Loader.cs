using System;
using System.Xml.Linq;
using AccountingBackupWeb.Models.AccountingBackup;
using DAgents.Common;
using Microsoft.Practices.Unity;

namespace QuickBooksService
{
    public abstract class Loader<T> : ProgramObject, ILoader where T : new()
    {
        [Dependency]
        public AccountingBackupEntities.IFactory ModelFactory { get; set; }

        protected AccountingBackupEntities Model { get; set; }
        protected Company Company { get; set; }
        protected abstract string TargetElementName { get; }
        protected abstract T GetOrCreate(dynamic source);
        protected abstract void CopyFields(dynamic source, T target);

        public Loader(Company company)
        {
            Company = company;
        }

        public void Load(XElement element)
        {
            if (TargetElementName == default(string))
            {
                throw new Exception("TargetElementName is not set");
            }

            string elementName = element.Name.LocalName;
            if (elementName == TargetElementName)
            {
                //Logger.Log("Processing " + Company.Name + " " + elementName);
                using (Model = ModelFactory.Create())
                {
                    dynamic source = new DynamicXML(element);
                    Company = Company.ByName(Company.Name, Model);
                    T target = GetOrCreate(source);
                    CopyFields(source, target);

                    //Logger.Log("Saving");
                    Model.SaveChanges();
                }
            }

            OnLoadComplete();
        }

        void OnLoadComplete() { if (LoadComplete != null) { LoadComplete(this, EventArgs.Empty); } }
        public event EventHandler LoadComplete;
    }
}
