using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Concrete
{
    public class SecurityRepository : ISecurityRepository
    {
        EomToolSecurityEntities db;
        public SecurityRepository()
        {
            db = new EomToolSecurityEntities();
        }

        public IEnumerable<Entities.Group> GroupsByWindowsIdentity(string windowsIdentity)
        {
            var groups = db.Groups.ToList();
            groups.Where(c => c.WindowsIdentity.Split(',').Any(d => d.ToUpper() == windowsIdentity.ToUpper()));
            return groups;
        }
    }
}
