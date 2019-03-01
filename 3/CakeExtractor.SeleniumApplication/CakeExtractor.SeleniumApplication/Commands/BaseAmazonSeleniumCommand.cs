namespace CakeExtractor.SeleniumApplication.Commands
{
    /// <summary>
    /// Base entity for selenium command. Contain base interface for commands interaction.
    /// </summary>
    public abstract class BaseAmazonSeleniumCommand
    {
        /// <summary>
        /// Do comamnds environment preparation steps.
        /// </summary>
        /// <param name="executionProfileNumber">The execution profile number.</param>
        public abstract void PrepareCommandEnvironment(int executionProfileNumber);

        /// <summary>
        /// Starts the command execution.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        public abstract string CommandName
        {
            get;
        }
    }
}
