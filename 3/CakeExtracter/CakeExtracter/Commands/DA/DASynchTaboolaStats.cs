using System.ComponentModel.Composition;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using Taboola.Utilities;

namespace CakeExtracter.Commands.DA
{
    [Export(typeof(ConsoleCommand))]
    class DASynchTaboolaStats : ConsoleCommand
    {
        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults
        /// </summary>
        public override void ResetProperties()
        {
            
        }

        /// <inheritdoc />
        /// <summary>
        /// The constructor sets a command name and command arguments names, provides a description for them.
        /// </summary>
        public DASynchTaboolaStats()
        {
            IsCommand("DASynchTaboolaStats", "Synch Taboola Stats");
        }

        /// <inheritdoc />
        /// <summary>
        /// The method runs the current command and extract and save statistics from the Taboola portal based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code</returns>
        public override int Execute(string[] remainingArguments)
        {
            TaboolaUtility.TokenSets = GetTokensFromDb();
            var utility = CreateUtility();

            utility.GetAccessToken();

            SaveTokens(TaboolaUtility.TokenSets);
            return 0;
        }

        private static TaboolaUtility CreateUtility()
        {
            return new TaboolaUtility();
        }

        private static string[] GetTokensFromDb()
        {
            return Platform.GetPlatformTokens(Platform.Code_Taboola);
        }

        private static void SaveTokens(string[] tokens)
        {
            Platform.SavePlatformTokens(Platform.Code_Taboola, tokens);
        }
    }
}
