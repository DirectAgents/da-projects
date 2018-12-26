using System;

namespace CakeExtractor.SeleniumApplication.Models
{
    class JobScheduleModel
    {
        public DateTime StartExtractionTime { get; set; }

        public int DaysInterval { get; set; }
    }
}
