using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.GoogleAdWords.Extracters;
using CakeExtracter.Etl.GoogleAdWords.Loaders;

namespace CakeExtracter.ConsoleCommands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesXmlCommand : ConsoleCommand
    {
        public SynchSearchDailySummariesXmlCommand()
        {
            IsCommand("synchSearchDailySummariesXml",
                      "synch SearchDailySummaries from Google AdWords Xml Report");
            HasRequiredOption("f|xmlFile=", "XML File", c => XmlFile = c);
            HasRequiredOption("a|account=", "Account Name", c => AccountName = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new AdWordsXmlReportExtracter(XmlFile, AccountName);
            var loader = new AdWordsXmlReportLoader();
            var extracterThread = extracter.BeginExtracting();
            var loaderThread = loader.BeginLoading(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        public string XmlFile { get; set; }

        public string AccountName { get; set; }
    }
}
