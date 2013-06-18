namespace CakeExtracter
{
    class ProgramArgs
    {
        private readonly string[] _args;

        public ProgramArgs(string[] args)
        {
            _args = args;
        }

        public bool IsScheduler
        {
            get { return ((_args.Length == 1) && (_args[0] == "scheduler")); }
        }

        public bool ValidForSyncher
        {
            get { return (_args.Length == 3); }
        }
    }
}