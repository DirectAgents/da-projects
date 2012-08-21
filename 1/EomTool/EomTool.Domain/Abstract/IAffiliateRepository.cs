﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Abstract
{
    public interface IAffiliateRepository
    {
        IQueryable<Affiliate> Affiliates { get; }
    }
}
