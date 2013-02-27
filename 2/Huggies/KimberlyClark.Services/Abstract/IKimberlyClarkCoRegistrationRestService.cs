namespace KimberlyClark.Services.Abstract
{
    public interface IKimberlyClarkCoRegistrationRestService
    {
        bool CheckIfConsumerExists(string email);
        IProcessResult ProcessConsumerInformation(IConsumer consumer);
    }
}
