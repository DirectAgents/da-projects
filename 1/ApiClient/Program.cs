using ApiClient.Etl.Cake;

namespace ApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new ConversionsFromWebService();
            var dest = new ConversionsToStaging();
            var extract = source.Extract();
            var load = dest.Load(source);
            extract.Join();
            load.Join();
        }
    }
}
