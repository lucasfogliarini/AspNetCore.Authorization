namespace AspNetCore.Authorization.JsonWebToken
{
    public class Jwt
    {
        public required string User { get; set; }
        public required string JwToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
