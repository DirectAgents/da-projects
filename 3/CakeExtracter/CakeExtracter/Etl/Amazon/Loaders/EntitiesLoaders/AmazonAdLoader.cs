using System.Collections.Generic;
using AutoMapper;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Amazon.Loaders.EntitiesLoaders
{
    public class AmazonAdLoader : AmazonBaseEntityLoader<TDad>
    {
        protected override string LevelName => AmazonJobLevels.Creative;

        protected override object LockerObject => SafeContextWrapper.AdLocker;

        public AmazonAdLoader(int accountId)
            : base(accountId)
        {
        }

        public void PrepareData(List<TDadSummary> items)
        {
            items.ForEach(x =>
            {
                if (x.TDad == null)
                {
                    x.TDad = new TDad();
                }
                Mapper.Map(x, x.TDad);
            });
        }
    }
}
