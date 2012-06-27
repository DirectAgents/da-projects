using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Objects;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data.Objects.DataClasses;
using EomApp1.Formss.AB2.Model.Adapters;

namespace EomApp1.Formss.AB2.Model
{
    public partial class DirectAgentsModel : DirectAgentsEntities
    {
        public DirectAgentsModel()
            : base()
        {
        }
    }

    public partial class SqlServerDatabase
    {
        public EomApp1.Formss.AB2.ExternalDatabase.ExternalDatabaseModel CreateExternalDatabaseModel()
        {
            return EomApp1.Formss.AB2.ExternalDatabase.ExternalDatabaseModel.CreateExternalDatabaseModel(this.ConnectionString);
        }
    }

    public partial class DirectTrackResource
    {
        public static DirectTrackResource Create(XDocument xDoc)
        {
            DirectTrackResource res = new DirectTrackResource();

            res.XmlDoc = xDoc.ToString();
            res.Updated = DateTime.Now;
            res.ResourceName = xDoc.Root.Name.LocalName;

            var adapter = new DirectTrackResourceAdapter(xDoc);
            adapter.MapTo(res);

            return res;
        }
    }

    public partial class Client
    {
        public Client() { }
    }

    public partial class Unit
    {
        //static public void operator << (Unit unit, CreditLimit creditLimit)
        //{
        //    unit.Id = creditLimit.Unit.Id;
        //    unit.Name = creditLimit.Unit.Name;
        //}
        static public explicit operator Unit(Client client)
        {
            return client.CreditLimit.Unit;
        }
    }

    public partial class CreditLimit
    {
        public CreditLimit() { }
        public CreditLimit(decimal quantity, Unit unit)
        {
            this.Quantity = quantity;
            this.Unit = unit;
        }
    }

    public partial class Advertiser
    {
        public Advertiser() { }
        public Advertiser(string name, Client client)
        {
            this.Name = name;
            this.Client = client;
        }
    }

    public partial class StartingBalance
    {
        public StartingBalance() { }
        public StartingBalance(Unit unit, Client client)
        {
            Item = new Item {
                Amount = new Amount(0, unit),
                DateSpan = new DateTime(2011, 1, 1),
                Client = client
            };
        }
    }

    public partial class DateSpan
    {
        public DateSpan() { }
        public DateSpan(DateTime dt)
        {
            this.From = dt;
        }
        static public implicit operator DateSpan(DateTime dateTime)
        {
            return new DateSpan(dateTime);
        }
    }

    public partial class Amount
    {
        public Amount() { }
        public Amount(decimal quantity, Unit unit)
        {
            this.Quantity = quantity;
            this.Unit = unit;
        }
    }
}
