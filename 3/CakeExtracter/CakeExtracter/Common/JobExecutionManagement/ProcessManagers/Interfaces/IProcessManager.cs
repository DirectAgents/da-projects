namespace CakeExtracter.Common.JobExecutionManagement.ProcessManagers.Interfaces
{
    /// <summary>
    /// The utility that helps to manage processes.
    /// </summary>
    public interface IProcessManager
    {
        /// <summary>
        /// Starts a new process of the current application with certain parameters.
        /// </summary>
        /// <param name="arguments">Command-line arguments to use when starting the application.</param>
        void RestartApplicationInNewProcess(string arguments);

        /// <summary>
        /// Terminates the current process.
        /// </summary>
        void EndCurrentProcess();
    }
}
