using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Web.Areas.TD.Models;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;
using System.Net;
using System.Web.UI.WebControls;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class ReportsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ReportsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Home(int campId)
        {
            var campaign = tdRepo.Campaign(campId);
            if (campaign == null)
                return HttpNotFound();
            return View(campaign);
        }

        public ActionResult Test()
        {
            ReportViewer rv = new ReportViewer()
            {
                ProcessingMode = ProcessingMode.Remote,
                Width = 1000,
                Height = 600
            };

            rv.ServerReport.ReportPath = "/DA - Trading Desk Report";
            //rv.ServerReport.ReportServerUrl = new Uri("http://biz/ReportServer_SQLEXPRESS");
            rv.ServerReport.ReportServerUrl = new Uri("http://173.204.123.91/ReportServer_SQL3");
            //rv.ServerReport.ReportServerCredentials = new ReportServerCredentials("administrator", "xxx", "");
            rv.ServerReport.ReportServerCredentials = new ReportServerCredentials("reportviewertest", "xxx", "");

            rv.SizeToReportContent = true;
            rv.Width = Unit.Percentage(100);
            rv.Height = Unit.Percentage(100);

            ViewBag.ReportViewer = rv;
            return View();
        }
	}
    public class ReportServerCredentials : IReportServerCredentials
    {
        private string _userName;
        private string _password;
        private string _domain;

        public ReportServerCredentials(string userName, string password, string domain)
        {
            _userName = userName;
            _password = password;
            _domain = domain;
        }

        public WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }
        public ICredentials NetworkCredentials
        {
            get { return new NetworkCredential(_userName, _password, _domain); }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            // Do not use forms credentials to authenticate.
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}