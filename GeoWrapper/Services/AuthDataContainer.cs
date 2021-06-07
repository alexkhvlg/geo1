namespace GeoWrapper.Services
{
    public class AuthDataContainer
    {
        public string Server { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public AuthDataContainer(string server, string login, string password)
        {
            Server = server;
            Login = login;
            Password = password;
        }
    }
}
