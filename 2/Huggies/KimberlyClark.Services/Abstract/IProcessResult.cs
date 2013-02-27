namespace KimberlyClark.Services.Abstract
{
    public interface IProcessResult
    {
        string Error { get; set; }
        bool IsSuccess { get; set; }
        bool IsSuccessSpecified { get; set; }
        string ProcessOutPut { get; set; }
    }
}
