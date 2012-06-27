using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using DAgents.Common;

namespace DirectTrack.Rest.Schemas
{
    public class Advertiser
    {
        Models.Resource _resource { get; set; }
        advertiser _dtAdvertiser = new advertiser();
        private XDocument doc;

        /// <summary>
        /// 
        /// </summary>
        public advertiser Inner
        {
            get { return _dtAdvertiser; }
            set { _dtAdvertiser = value; }
        }

        /// <summary>
        /// Construct from database entity containing the xml content from direct track
        /// </summary>
        /// <param name="resource"></param>
        public Advertiser(Models.Resource resource)
        {
            _resource = resource;
            DAgents.Common.Utilities.CopyUtility.Copy(XDocument.Parse(resource.Content).Deserialize<advertiser>(), _dtAdvertiser);
            _dtAdvertiser.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_advertiser_PropertyChanged);
        }

        void _advertiser_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_resource != null) _resource.Content = new XDocument().Serialize(_dtAdvertiser);
        }

        /// <summary>
        /// Refresh from DirectTrack
        /// </summary>
        public void Refresh()
        {
            var dtAdvertiser = XmlGetter.GetDoc("advertiser/" + this.Id);
            DAgents.Common.Utilities.CopyUtility.Copy(dtAdvertiser.Deserialize<advertiser>(), _dtAdvertiser);
        }

        public int Id { get { return int.Parse(this.Inner.location.Split('/').Last()); } }
        public string Company { get { return Inner.company; } }
        public string FirstName { get { return Inner.firstName; } }
        public string LastName { get { return Inner.lastName; } }
    }
}

