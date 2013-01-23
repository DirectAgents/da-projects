using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using DAgents.Common;

namespace AccountingBackupWeb.Models.DirectTrack
{
    public partial class DirectTrackSynchContainer
    {
        partial void OnContextCreated()
        {
            CreateEntityTypes();
            CreateDirectTracks();
            CreateSynchTaskTypes();
        }

        private void CreateSynchTaskTypes()
        {
            if (SynchTaskTypes.Count() == 0)
            {
                SynchTaskTypes.AddObject(new SynchTaskType {
                    Name = "CampaignList",
                    Description = "Get a list of all campaigns.",
                });
                SynchTaskTypes.AddObject(new SynchTaskType {
                    Name = "ExtractCampaigns",
                    Description = "Pull campaigns.",
                });
                SynchTaskTypes.AddObject(new SynchTaskType {
                    Name = "ExtractCampaign",
                    Description = "Pull campaign.",
                });
                SaveChanges();
            }
        }

        private void CreateDirectTracks()
        {
        }

        private void CreateEntityTypes()
        {
            int entityTypesCount = EntityTypes.Count();

            if (entityTypesCount == 0)
            {
                EntityTypes.AddObject(
                    new EntityType {
                        Name = "Campaign"
                    }
                );

                EntityTypes.AddObject(
                    new EntityType {
                        Name = "CampaignList"
                    }
                );
                SaveChanges();
            }
        }

        public IQueryable<SynchTask> UnstartedSynchTasks
        {
            get
            {
                return
                    from c in SynchTasks
                    where c.Started == null
                    select c;
            }
        }

        public SynchTask CampaignList
        {
            get
            {
                return (from c in SynchTasks
                        where
                             c.SynchTaskType.Name == "CampaignList" &&
                             c.Error == null &&
                             c.Started != null &&
                             c.Stopped != null &&
                             c.Entity != null &&
                             c.Entity.EntityType.Name == "CampaignList"
                        orderby c.Stopped descending
                        select c).FirstOrDefault() ?? EmptyCampaignList;
            }
        }

        public SynchTask EmptyCampaignList
        {
            get
            {
                return new SynchTask {
                    Entity = new Entity {
                        Content = new XDocument().ToString()
                    }
                };
            }
        }

        public AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.Campaign.campaign GetDirectTrackCampaign(int programID)
        {
            string programIdAsString = programID.ToString();

            var campaignEntity = (from c in Entities
                                  where c.EntityType.Name == "Campaign" && c.Key.EndsWith(programIdAsString)
                                  select c).FirstOrDefault();

            AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.Campaign.campaign result;

            if(campaignEntity == null)
            {
                result = null;
            }
            else
            {
                result = 
                    XDocument.Parse(campaignEntity.Content)
                        .Deserialize<AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.Campaign.campaign>();
            }

            return result;
        }
    }
}
