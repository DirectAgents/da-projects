namespace DAgents.Common
{
    public abstract class ProgramAction : ProgramObject, IProgramAction
    {
        public abstract void Execute();
    }
}
