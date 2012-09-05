﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomTool.Domain.Entities;
using System.Security.Principal;

namespace EomTool.Domain.Abstract
{
    public interface ISecurityRepository
    {
        Group WindowsIdentityGroup(string windowsIdentity);
        IEnumerable<Group> GroupsForUser(IPrincipal user);
    }
}
