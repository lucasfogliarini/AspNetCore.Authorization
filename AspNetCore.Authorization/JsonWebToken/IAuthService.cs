namespace AspNetCore.Authorization.JsonWebToken
{
    public interface IAuthService
    {
        public Jwt CreateJwt(AuthenticationInput authenticationInput);
    }
}
