using System.Linq;

namespace CakeExtracter.Etl
{
    public class Transformer<TIn, TOut> : Extracter<TOut>
        where TIn : class
        where TOut : class
    {
        private readonly Extracter<TIn> extracter;
        private readonly Loader<TOut> loader;

        protected Transformer(Extracter<TIn> extracter, Loader<TOut> loader)
        {
            this.extracter = extracter;
            this.loader = loader;
        }

        public void Run()
        {
            var loaderThread = loader.Start(this);
            var extracterThread = Start();
            loaderThread.Join();
            extracterThread.Join();
        }

        protected override void Extract()
        {
            Add(extracter.EnumerateAll().Select(c => Transform(c)));
        }

        protected virtual TOut Transform(TIn item)
        {
            return item as TOut;
        }
    }
}