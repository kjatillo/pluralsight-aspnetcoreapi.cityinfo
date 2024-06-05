namespace Pluralsight.AspNetCoreWebApi.CityInfo.Services
{
    public class LocalMainService
    {
        private string _mailTo = "admin@mycompany.com";
        private string _mailFrom = "noreply@mycompany.com";

        public void Send(string subject, string message)
        {
            // Send Mail :) - output to console
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(LocalMainService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
