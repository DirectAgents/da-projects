using System.ComponentModel.Composition;
using System.Threading;
using CakeExtracter.Common;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class PrintIt : ConsoleCommand
    {
        public PrintIt()
        {
            IsCommand("printIt", "fake it");
            HasOption("t|Pause", "ms to pause between prints", c => this.Pause = int.Parse(c));
            HasOption("i|Iterations", "number times to execute", c => this.Iterations = int.Parse(c));
            HasOption("m|Message", "message to print", c => this.Message = c);

        }

        public override int Execute(string[] remainingArguments)
        {
            for (int i = 0; i < this.Iterations; i++)
            {
                System.Console.WriteLine(this.Message);
                Thread.Sleep(this.Pause);
            }
            return 0;
        }

        public int Pause { get; set; }

        public int Iterations { get; set; }

        public string Message { get; set; }
    }

    [Export(typeof(ConsoleCommand))]
    public class PrintIt2 : ConsoleCommand
    {
        public PrintIt2()
        {
            IsCommand("printIt2", "fake it");
            HasOption("t|Pause", "ms to pause between prints", c => this.Pause = int.Parse(c));
            HasOption("i|Iterations", "number times to execute", c => this.Iterations = int.Parse(c));
            HasOption("m|Message", "message to print", c => this.Message = c);

        }

        public override int Execute(string[] remainingArguments)
        {
            for (int i = 0; i < this.Iterations; i++)
            {
                System.Console.WriteLine(this.Message);
                Thread.Sleep(this.Pause);
            }
            return 0;
        }

        public int Pause { get; set; }

        public int Iterations { get; set; }

        public string Message { get; set; }
    }
}
