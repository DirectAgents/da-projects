using System.Reflection;
using LTWeb.DataAccess;
using LTWeb.Service;
using System;

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
                service.Send(lendingTreeModel);
                context.SaveChanges();

                return leadPost.AppID;
            }
        }

        private static void CopyProperties<TDest, TSource>(TDest destObj, TSource sourceObj)
        {
            foreach (PropertyInfo sourceProp in typeof(TSource).GetProperties())
            {
                PropertyInfo targetProp = typeof(TDest).GetProperty(sourceProp.Name);
                if (targetProp != null)
                    targetProp.SetValue(destObj, sourceProp.GetValue(sourceObj));
            }
        }
    }
}
