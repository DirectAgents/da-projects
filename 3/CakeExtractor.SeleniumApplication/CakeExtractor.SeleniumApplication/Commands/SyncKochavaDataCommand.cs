namespace CakeExtractor.SeleniumApplication.Commands
{
    /// <summary>
    /// Command for cochava job.
    /// </summary>
    /// <seealso cref="CakeExtractor.SeleniumApplication.Commands.BaseSeleniumCommand" />
    internal class SyncKochavaDataCommand : BaseSeleniumCommand
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        public override string CommandName => "SyncCochavaDataCommand";

        /// <summary>
        /// Do comamnds environment preparation steps.
        /// </summary>
        /// <param name="executionProfileNumber">The execution profile number.</param>
        public override void PrepareCommandEnvironment(int executionProfileNumber)
        {
        }

        /// <summary>
        /// Starts the command execution.
        /// </summary>
        public override void Run()
        {
        }
    }
}
