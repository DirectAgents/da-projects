﻿using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class CreativeStatVM
    {
        private CreativeStat Model { get; set; }

        public CreativeStatVM(CreativeStat creativeStat)
        {
            this.Model = creativeStat;
        }

        public int CreativeId
        {
            get { return Model.CreativeId; }
        }
        public string CreativeName
        {
            get { return Model.Creative.CreativeName; }
        }
        public string Clicks
        {
            get { return (Model.Clicks.HasValue ? Model.Clicks.Value.ToString("N0") : ""); }
        }
        public string Leads
        {
            get { return (Model.Leads.HasValue ? Model.Leads.Value.ToString("N0") : ""); }
        }
        public string ClickThroughRate
        {
            get { return (Model.ClickThroughRate.HasValue ? String.Format("{0:0.00%}", Model.ClickThroughRate.Value) : ""); }
        }
        public string ConversionRate
        {
            get { return (Model.ConversionRate.HasValue ? String.Format("{0:0.00%}", Model.ConversionRate.Value) : ""); }
        }

        public byte[] Thumbnail
        {
            get { return Model.Creative.Thumbnail; }
        }

        public string Link
        {
            get
            {
                if (Model.Creative.CreativeFiles.Count > 0)
                {
                    var fileWithPreview = Model.Creative.CreativeFiles.FirstOrDefault(f => f.Preview);
                    if (fileWithPreview != null)
                        return fileWithPreview.CreativeFileLink;
                }
                return null;
            }
        }
    }
}