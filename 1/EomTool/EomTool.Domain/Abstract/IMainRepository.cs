﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Abstract
{
    public interface IMainRepository
    {
        IQueryable<PublisherPayout> PublisherPayouts { get; }

        void Media_ApproveItems(int[] itemIds);
        void Media_HoldItems(int[] itemIds);
    }
}
