namespace Iris.ServiceLayer.Contracts
{
    public class SmtpSettings
    {
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
    }
}
