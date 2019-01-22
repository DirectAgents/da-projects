namespace CakeExtractor.SeleniumApplication.Commands
{
    public abstract class BaseAmazonSeleniumCommand
    {
        public abstract void PrepareCommandEnvironment();

        public abstract void Run();

        public abstract string CommandName
        {
            get;
        }
    }
}
