using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using DAgents.Common;

namespace EomApp1.Formss.AB2.Model.Adapters
{
    /// <summary>
    /// This class is used to ....
    /// </summary>
    public class DirectTrackResourceAdapter : IAdapter<XDocument>
    {
        public DirectTrackResourceAdapter(XDocument directTrackResourceXDoc)
        {
            // the source object that is being "adapted"
            this.Source = directTrackResourceXDoc;

            // get the name of root element of the direct track XML
            string rootElement = directTrackResourceXDoc.Root.Name.LocalName;

            // now create an instance of the nested type (inner class) that has the same name
            var adapter = this.GetType().GetNestedTypes().Where(c => c.Name == rootElement);
            this.Target = Activator.CreateInstance(adapter.First(), directTrackResourceXDoc);
        }

        #region IAdaptable

        public XDocument Source { get; set; }
        public object Target { get; set; } 

        // Assigns the created adapter (inner class instance) to the single assignable property in mapTarget
        public void MapTo<TTo>(TTo mapTarget)
        {
            var prop = from c in mapTarget.GetType().GetProperties()
                       where c.PropertyType.IsAssignableFrom(Target.GetType())
                       select c;

            if (prop.Count() != 1) throw new Exception("ambiguous");

            prop.First().SetValue(mapTarget, Target, null);
        }

        #endregion

        // An adapter class with the same name as the XDoc root element name that 
        // inherits from the target entity type
        //
        // public class <root-element-name-of-directtrack-xml> : <model-type-being-adapted-to>
        // {
        //    public <root-element-name-of-directtrack-xml>(XDocument xDoc)
        //    {
        //       // TODO: logic to assign fields from XML document to Entity
        //    }
        // }
        #region Adapters

        public class campaign : Wrap<DirectTrackCampaign>
        {
            public campaign(XDocument xDoc)
            {
                var campaign = new DAgents.Synch.CampaignDetail(xDoc.ToString());

                Inner.Campaign = new Campaign 
                {
                    Name = campaign.CampaignName
                };
            }
        }

        public class campaignGroup : Wrap<DirectTrackCampaignGroup>
        {
            public campaignGroup(XDocument campaignGroupResouceXDoc)
            {
                var campaignGroup = new DAgents.Synch.CampaignGroup(campaignGroupResouceXDoc.ToString());

                Inner.Name = campaignGroup.Name;

                foreach (var pid in campaignGroup.CampaignPIDs)
                {
                    using (var model = new DirectAgentsEntities())
                    {
                        // see if direct track campaign already exists
                        var directTrackCampaign = model.DirectTrackCampaigns.Where(c => c.CampaignNumber == pid).FirstOrDefault();

                        // if not, then create it
                        if (directTrackCampaign == null)
                        {
                            directTrackCampaign = new campaign(XDocument.Parse(DirectTrack.Rest.XmlGetter.ViewCampaign(pid))).Inner;
                        }
                        else
                        {
                            // found, need to detach from the querying context
                            model.Detach(directTrackCampaign);
                        }

                        Inner.DirectTrackCampaigns.Add(directTrackCampaign);
                    }
                }
            }
        }

        #endregion
    }
}
