﻿using LTWeb.DataAccess;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace LTWeb.Service
{
    public class LendingTreeService : ILendingTreeService
    {
        public ILendingTreeResult Send(ILendingTreeModel request)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.GetUrlForPost());
            webRequest.ContentType = "text/xml";
            webRequest.Method = "POST";

            string requestXML = request.GetXMLForPost();
            
            var lead = new Lead
            {
                RequestContent = requestXML,
                Timestamp = DateTime.UtcNow,
                AppId = request.AppID,
                AffiliateId = request.AffiliateSiteID,
                IPAddress = request.VisitorIPAddress
            };

            using (var writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(requestXML);
            }

            using (var response = (HttpWebResponse)webRequest.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var responseXML = reader.ReadToEnd();

                lead.ResponseContent = responseXML;
                lead.ResponseTimestamp = DateTime.UtcNow;
                using (var repo = new Repository(new LTWebDataContext(), false))
                {
                    repo.Add<Lead>(lead);
                }

                return new LendingTreeResult
                {
                    IsSuccess = true // hard coded for now
                };
            }
        }

        public void Resend(Lead lead)
        {
            string serviceConfigName = System.Web.Configuration.WebConfigurationManager.AppSettings["LendingTreeServiceConfig"];
            var ltm = new LendingTreeModel(serviceConfigName);
            string url = ltm.GetUrlForPost();

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "text/xml";
            webRequest.Method = "POST";

            string requestXML = lead.RequestContent;

            using(var context = new LTWebDataContext())
            using (var repo = new Repository(context))
            {
                Lead existingLead = repo.Single<Lead>(c => c.AppId == lead.AppId);
                existingLead.RequestContent = requestXML;

                using (var writer = new StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(requestXML);
                }

                using (var response = (HttpWebResponse)webRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var responseXML = reader.ReadToEnd();
                    lead.ResponseContent = responseXML;
                    existingLead.ResponseContent = responseXML;
                    context.SaveChanges();
                }
            }
        }

        public IQueryable<Lead> GetAllLeads(LTWebDataContext context)
        {
            return context.Leads;
        }
    }
}
