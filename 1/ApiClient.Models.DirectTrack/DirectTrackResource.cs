using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ApiClient.Models.DirectTrack
{
    public class DirectTrackResource
    {
        [Key]
        [System.ComponentModel.DataAnnotations.MaxLength(500)]
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
