using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectAgents.Domain.Entities.Cake
{
    public class MonthlySummary
    {
        private System.DateTime dateField;

        private int clicksField;

        private int conversionsField;

        private int paidField;

        private int sellableField;

        private decimal costField;

        private decimal revenueField;

        //-------------------------------------
        public int MonthlySummaryId { get; set; }

        public int pid { get; set; }
        //-------------------------------------

        /// <remarks/>
        public System.DateTime date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public int clicks
        {
            get
            {
                return this.clicksField;
            }
            set
            {
                this.clicksField = value;
            }
        }

        /// <remarks/>
        public int conversions
        {
            get
            {
                return this.conversionsField;
            }
            set
            {
                this.conversionsField = value;
            }
        }

        /// <remarks/>
        public int paid
        {
            get
            {
                return this.paidField;
            }
            set
            {
                this.paidField = value;
            }
        }

        /// <remarks/>
        public int sellable
        {
            get
            {
                return this.sellableField;
            }
            set
            {
                this.sellableField = value;
            }
        }

        /// <remarks/>
        public decimal cost
        {
            get
            {
                return this.costField;
            }
            set
            {
                this.costField = value;
            }
        }

        /// <remarks/>
        public decimal revenue
        {
            get
            {
                return this.revenueField;
            }
            set
            {
                this.revenueField = value;
            }
        }
    }
}
