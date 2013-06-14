using System;
using LTWeb.DataAccess;
using LTWeb.Service;

namespace LTWeb.ws
{
    public class LeadService : ILeadService
    {
        public string RefiLead(LeadPost leadPost, string password)
        {
            using (var context = new LTWebDataContext())
            {
                // Save new lead post
                context.LeadPosts.Add(leadPost);
                context.SaveChanges();

                // Create lending tree model
                var lendingTreeModel = new LendingTreeModel((leadPost.Test ?? false) ? "Test" : PartnerNameFromPassword(password));

                // Copy app id from lending tree model to lead post
                // NOTE: this will be saved below after the data has been posted to the lending tree server
                leadPost.AppID = lendingTreeModel.AppID;

                // Copy properties of lead post to lending tree model
                CopyProperties(lendingTreeModel, leadPost);

                // Create lending tree service
                var service = new LendingTreeService();

                // Post to lending tree server
                string errorResponse = service.SendAndReturnAnyErrorResponse(lendingTreeModel);

                // Save the app id
                // NOTE: by saving the app id separately, any rows without an app id represent failed attempts to send to tree server
                context.SaveChanges();

                if (errorResponse != null)
                    throw new Exception(errorResponse);

                return leadPost.AppID;
            }
        }

        // TODO: put in database
        private static string PartnerNameFromPassword(string password)
        {
            string partnerName;
            switch (password)
            {
                case "h5b5creP":
                    partnerName = "PaperLeaf";
                    break;
                case "u2s9ruvQ":
                    partnerName = "Acquisition Technologies";
                    break;
                default:
                    throw new Exception("Invalid Password");
            }
            return partnerName;
        }

        private static void CopyProperties<TDest, TSource>(TDest dest, TSource source)
        {
            foreach (var p in typeof(TSource).GetProperties())
            {
                var target = typeof(TDest).GetProperty(p.Name);
                if (target != null)
                {
                    var value = p.GetValue(source);

                    if (value == null)
                        continue;
                    if (value is decimal && ((decimal)value) == 0)
                        continue;

                    target.SetValue(dest, value);
                }
            }
        }
    }
}
