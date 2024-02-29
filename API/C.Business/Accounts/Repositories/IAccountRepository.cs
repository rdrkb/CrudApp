using Contracts.DTOs;
using Contracts.Models;

namespace Business.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> UserExists(string username);
        Task Register(AccountModel accountModel);
        Task<AccountModel> Login(LoginModel loginModel);
        Task<bool> DeleteAccount(string username);
    }
}
