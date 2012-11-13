namespace LTWeb.Service
{
    public interface ILendingTreeService
    {
        ILendingTreeResult Send(ILendingTreeModel request);
    }
}
