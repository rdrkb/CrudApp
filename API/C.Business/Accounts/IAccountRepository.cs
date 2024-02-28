

using A.Contracts.DTOs;
using A.Contracts.Models;

namespace C.Business.Accounts
{
    public interface IAccountRepository
    {
        Task<bool> UserExists(string username);
        Task Register(AccountModel accountModel);
        Task<AccountModel> Login(LoginModel loginModel);
        Task<bool> DeleteAccount(string username);
    }
}
