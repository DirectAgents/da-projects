﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Roku
{
    /// <summary>
    /// Roku  summary metric entity.
    /// </summary>
    public class RokuSummary
    {

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        /// <value>
        ///  Id.
        /// </value>
        [Key]
        [Column(Order = 0)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the impressions metric value.
        /// The number of times your ads were on screen.
        /// </summary>
        /// <value>
        /// The impressions.
        /// </value>
        public string OrderName { get; set; }

        /// <summary>
        /// Gets or sets the clicks metric value.
        /// </summary>
        /// <value>
        /// The clicks.
        /// </value>
        public string Types { get; set; }

        /// <summary>
        /// Gets or sets Number Of lineitems value.
        /// </summary>
        /// <value>
        /// Number Of lineitems.
        /// </value>
        public int NumberOfLineItems { get; set; }

        /// <summary>
        /// Gets or sets the flight dates value.
        /// </summary>
        /// <value>
        /// flight dates.
        /// </value>
        public string FlightDates { get; set; }

        /// <summary>
        /// Gets or sets the Stats value.
        /// </summary>
        /// <value>
        /// The Stats.
        /// </value>
        public string Stats { get; set; }

        /// <summary>
        /// Gets or sets the Spend metric value.
        /// </summary>
        /// <value>
        /// The Spend.
        /// </value>
        public string Spend { get; set; }

        /// <summary>
        /// Gets or sets the Budget revenue metric value.
        /// </summary>
        /// <value>
        /// Budget.
        /// </value>
        public string Budget { get; set; }

        /// <summary>
        /// Gets or sets the Order Date metric value.
        /// </summary>
        /// <value>
        /// Date.
        /// </value>
        public string OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the extracting date.
        /// </summary>
        /// <value>
        /// Date.
        /// </value>
        [Key]
        [Column(Order = 1)]
        public DateTime ExtractingDate { get; set; }
    }
}