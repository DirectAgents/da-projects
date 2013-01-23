namespace QuickBooks.UI
{
    public class PresenterBase<T> where T : IView
    {
        public PresenterBase(T view)
        {
            this.View = view;
        }

        protected T View { get; private set; }
    }
}
