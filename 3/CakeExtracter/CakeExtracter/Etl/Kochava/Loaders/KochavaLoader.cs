using CakeExtracter.Etl.Kochava.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Kochava;
using System;
using System.Collections.Generic;

namespace CakeExtracter.Etl.Kochava.Loaders
{
    /// <summary>
    /// Kochava Data Loader
    /// </summary>
    public class KochavaLoader
    {
        private readonly KochavaCleaner kochavaCleaner;
        
        public KochavaLoader(KochavaCleaner kochavaCleaner)
        {
            this.kochavaCleaner = kochavaCleaner;
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="accountsReportData">The accounts report data.</param>
        public void LoadData(List<KochavaReportItem> accountsReportData, ExtAccount account)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw ex;
            }
        }
    }
}
