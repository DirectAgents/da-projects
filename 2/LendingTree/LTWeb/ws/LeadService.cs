using System;
using LTWeb.DataAccess;
using LTWeb.Service;

namespace LTWeb.ws
{
    public class LeadService : ILeadService
    {
        public string RefiLead(LeadPost leadPost, string password)
        {
            if (password != "h5b5creP") // TODO: unhardcode
                throw new Exception("Invalid Password");

            using (var context = new LTWebDataContext())
            {
                context.LeadPosts.Add(leadPost);
                context.SaveChanges();

                var lendingTreeModel = new LendingTreeModel((leadPost.Test ?? false) ? "Test" : "PaperLeaf");
                leadPost.AppID = lendingTreeModel.AppID;
                CopyProperties(lendingTreeModel, leadPost);

                var service = new LendingTreeService();
                string errorResponse = service.SendAndReturnAnyErrorResponse(lendingTreeModel);
                context.SaveChanges();

                if (errorResponse != null)
                    throw new Exception(errorResponse);

                return leadPost.AppID;
            }
        }

        private static void CopyProperties<TDest, TSource>(TDest destObj, TSource sourceObj)
        {
            foreach (var sourceProp in typeof(TSource).GetProperties())
            {
                var targetProp = typeof(TDest).GetProperty(sourceProp.Name);
                if (targetProp != null)
                {
                    var valueToSet = sourceProp.GetValue(sourceObj);

                    if (valueToSet == null)
                        continue;

                    if (valueToSet is decimal && ((decimal)valueToSet) == 0)
                        continue;

                    targetProp.SetValue(destObj, valueToSet);
                }
            }
        }
    }
}
