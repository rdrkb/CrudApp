

using A.Contracts.Models;

namespace C.Business.TokenServices
{
    public interface ITokenService
    {
        string CreateToken(AccountModel accountModel);
        string GetUsernameFromToken(string token);
    }
}
