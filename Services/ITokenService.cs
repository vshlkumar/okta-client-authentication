using DemoApplication.Model;

namespace DemoApplication.Services
{
    public interface ITokenService
    {
        Task<OktaToken> GetToken(string username, string password);
    }
}
