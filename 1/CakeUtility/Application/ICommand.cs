namespace DirectAgents.Common
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<TResult>
    {
        TResult Execute();
    }

    public interface ICommand<TInput, TResult>
    {
        TResult Execute(TInput input);
    }
}
