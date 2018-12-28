using System;

namespace CakeExtractor.SeleniumApplication.Models.CommonHelperModels
{
    internal class JobScheduleModel
    {
        public DateTime StartExtractionTime { get; set; }

        public int DaysInterval { get; set; }
    }
}
