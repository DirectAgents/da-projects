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
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
