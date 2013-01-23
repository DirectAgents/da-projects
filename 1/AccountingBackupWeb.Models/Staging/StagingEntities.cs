using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingBackupWeb.Models.Staging
{
    public partial class StagingEntities
    {
        public interface IFactory
        {
            StagingEntities Create();
        }
    }
}
