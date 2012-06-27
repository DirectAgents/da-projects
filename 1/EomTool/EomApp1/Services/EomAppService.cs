using System;
using EomApp1.Formss.Final;
using System.Collections.Generic;
using System.Linq;

namespace EomApp1.Services
{
    public class EomAppService
    {
        public EomAppService(IDataService dataService)
        {
            Data = dataService;
        }
        public IDataService Data { get; set; }
    }
    public class DataService : IDataService
    {
        public DataService(ITxns transactionScripts)
        {
            Txns = transactionScripts;
        }
        public ITxns Txns { get; set; }
    }
    public interface IDataService
    {
        ITxns Txns { get; set; }
    }
    public class Txns : ITxns
    {
        public void AddCampaignNote(int campaignId, string note)
        {
            var db = new FinalizeDataDataContext(true);
            var campaignNote = new CampaignNote
                                   {
                                       campaign_id = campaignId,
                                       note = note
                                   };
            db.CampaignNotes.InsertOnSubmit(campaignNote);
            db.SubmitChanges();
        }
        public IEnumerable<CampaignNote> GetCampaignNotes(int campaignId)
        {
            return from c in new FinalizeDataDataContext(true).CampaignNotes
                   where c.campaign_id == campaignId
                   select c;
        }
    }
    public interface ITxns
    {
        void AddCampaignNote(int campaignId, string note);
        IEnumerable<CampaignNote> GetCampaignNotes(int campaignId);
    }
}