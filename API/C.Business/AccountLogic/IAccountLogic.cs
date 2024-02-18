

using A.Contracts.DTOs;

namespace C.Business.AccountLogic
{
    public interface IAccountLogic
    {
        Task<bool> UserExists(string username);
        Task<Token> Register(RegisterModel registerModel);
        Task<Token> Login(LoginModel loginModel);
        Task<bool> DeleteAccount(string username);
    }
}
