using System;
using CakeExtracter.Commands;

namespace CakeExtracter
{
    class SyncherRun
    {
        private readonly Syncher _syncher;
        private readonly int _intervalMinutes;
        private readonly int _advertiserId;
        private readonly int _offsetDays;
        private DateTime _last;

        public SyncherRun(Syncher syncher, int advertiserId, int intervalMinutes, int offsetDays)
        {
            _syncher = syncher;
            _advertiserId = advertiserId;
            _intervalMinutes = intervalMinutes;
            _offsetDays = offsetDays;
            _last = DateTime.MinValue;
        }

        public void Run()
        {
            var now = DateTime.Now;
            var today = now.Date;
            var tommorow = today.AddDays(1);

            Console.WriteLine("Running synch for advertiser {0} at {1}..", _advertiserId, now);

            _last = now;

            _syncher.Run(new SyncherCommand {
                AdvertiserId = _advertiserId.ToString(),
                StartDate = today.AddDays(_offsetDays),
                EndDate = tommorow.AddDays(_offsetDays)
            });
        }

        public DateTime Next { get { return _last.AddMinutes(_intervalMinutes); } }
    }
}