

using A.Contracts.DTOs;
using A.Contracts.Models;

namespace B.Database.MongoDB.AccountData
{
    public interface IAccountDataAccess
    {
        Task<bool> UserExists(string username);
        Task Register(AccountModel accountModel);
        Task<AccountModel> Login(LoginModel loginModel);
        Task<bool> DeleteAccount(string username);
    }
}
