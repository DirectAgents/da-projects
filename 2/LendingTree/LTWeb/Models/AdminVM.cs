﻿using System;
using System.ComponentModel.DataAnnotations;
namespace LTWeb.Models
{
    public class AdminVM
    {
        public string Rate1 { get; set; }
        public string Rate2 { get; set; }
        [DataType(DataType.MultilineText)]
        public string Pixel { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}