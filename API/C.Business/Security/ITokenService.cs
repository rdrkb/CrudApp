

using A.Contracts.Models;

namespace C.Business.Security
{
    public interface ITokenService
    {
        string CreateToken(AccountModel accountModel);
        string GetUsernameFromToken(string token);
    }
}
