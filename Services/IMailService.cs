namespace Pluralsight.AspNetCoreWebApi.CityInfo.Services
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}
