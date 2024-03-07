using Contracts.Models;

namespace Business.Security
{
    public interface ITokenService
    {
        string CreateToken(AccountModel accountModel);
        string GetUsernameFromToken(string token);
        string GetRoleFromToken(string token); 
    }
}
