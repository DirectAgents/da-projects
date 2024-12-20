﻿using System;
using System.Collections.Generic;

namespace FacebookAPI.Entities
{
    /// <summary>
    /// Facebook summary row entity.
    /// </summary>
    public class FBSummary
    {
        public DateTime Date { get; set; }

        public decimal Spend { get; set; }

        public int Impressions { get; set; }

        public int LinkClicks { get; set; }

        public int AllClicks { get; set; }

        public int Conversions_click { get; set; }

        public int Conversions_view { get; set; }

        public decimal ConVal_click { get; set; }

        public decimal ConVal_view { get; set; }

        public string CampaignId { get; set; }

        public string CampaignName { get; set; }

        public string AdSetId { get; set; }

        public string AdSetName { get; set; }

        public string AdId { get; set; }

        public string AdName { get; set; }

        public string AdStatus { get; set; }

        public bool AllZeros()
        {
            return Spend == 0 && Impressions == 0 && LinkClicks == 0 && AllClicks == 0 &&
                   Conversions_click == 0 && Conversions_view == 0 && ConVal_click == 0 && ConVal_view == 0;
        }

        public Dictionary<string, FBAction> Actions { get; set; }

        public void SetNumConvsFromAction(FBAction action)
        {
            if (action.Num_click.HasValue)
            {
                this.Conversions_click = action.Num_click.Value;
            }
            if (action.Num_view.HasValue)
            {
                this.Conversions_view = action.Num_view.Value;
            }
        }

        public void SetConValsFromAction(FBAction action)
        {
            if (action.Val_click.HasValue)
            {
                this.ConVal_click = action.Val_click.Value;
            }
            if (action.Val_view.HasValue)
            {
                this.ConVal_view = action.Val_view.Value;
            }
        }
    }
}
