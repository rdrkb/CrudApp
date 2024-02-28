

using A.Contracts.DTOs;

namespace C.Business.Accounts
{
    public interface IAccountService
    {
        Task<bool> UserExists(string username);
        Task<Token> Register(RegisterModel registerModel);
        Task<Token> Login(LoginModel loginModel);
        Task<bool> DeleteAccount(string username);
    }
}
