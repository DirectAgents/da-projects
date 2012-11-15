using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTWeb
{
    public class AdminSetting : Entity<long>
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
