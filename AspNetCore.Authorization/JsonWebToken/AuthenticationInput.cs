namespace AspNetCore.Authorization.JsonWebToken
{
    public class AuthenticationInput(string user, string password)
    {
        public string User { get; set; } = user;
        public string Password { get; set; } = password;
    }
}
