using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Screens.Final
{
    public static class CampaignNoteUtility
    {
        public static void AddCampaignNote(int campaignId, string note)
        {
            if (!string.IsNullOrWhiteSpace(note))
            {
                var db = new FinalizeDataDataContext(true);
                var campaignNote = new CampaignNote {
                    campaign_id = campaignId,
                    note = note
                };
                db.CampaignNotes.InsertOnSubmit(campaignNote);
                db.SubmitChanges();
            }
        }
    }
}
