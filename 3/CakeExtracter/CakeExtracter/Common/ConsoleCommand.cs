namespace CakeExtracter.Common
{
    public abstract class ConsoleCommand : ManyConsole.ConsoleCommand
    {
        public override int Run(string[] remainingArguments)
        {
            using (new LogElapsedTime())
            {
                var retCode = Execute(remainingArguments);
                return retCode;
            }
        }

        public abstract int Execute(string[] remainingArguments);
    }
}
