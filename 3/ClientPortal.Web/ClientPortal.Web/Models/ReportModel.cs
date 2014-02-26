using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class ReportModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string TodayString
        {
            get { return _userInfo.Dates.Today.ToString("d", _userInfo.CultureInfo); }
        }
        public string YesterdayString
        {
            get { return _userInfo.Dates.Yesterday.ToString("d", _userInfo.CultureInfo); }
        }
        public string OneYearAgoString
        {
            get { return _userInfo.Dates.OneYearAgo.ToString("d", _userInfo.CultureInfo); }
        }

        private UserInfo _userInfo;
        public bool ShowConVal
        {
            get { return _userInfo.ShowConversionData; }
        }
        public string ConValName
        {
            get { return _userInfo.ConversionValueName; }
        }
        public bool ConValIsNum
        {
            get { return _userInfo.ConversionValueIsNumber; }
        }
        public ReportModel(UserInfo userInfo, bool defaultSetupMTD = true)
        {
            _userInfo = userInfo;

            if (defaultSetupMTD)
            {
                StartDate = userInfo.Dates.FirstOfMonth.ToString("d", userInfo.CultureInfo);
                EndDate = userInfo.Dates.Latest.ToString("d", userInfo.CultureInfo);
            }
        }

        public ReportModel(string startDate, string endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public void SetStartEndYesterday()
        {
            StartDate = _userInfo.Dates.Yesterday.ToString("d", _userInfo.CultureInfo);
            EndDate = _userInfo.Dates.Yesterday.ToString("d", _userInfo.CultureInfo);
        }
    }
}