using Business.Accounts.Repositories;
using Business.Security;
using Contracts.DTOs;
using Contracts.Models;
using System.Security.Cryptography;
using System.Text;

namespace Business.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountDataAccess;
        private readonly ITokenService _tokenService;

        public AccountService(IAccountRepository accountDataAccess, ITokenService tokenService)
        {
            _accountDataAccess = accountDataAccess;
            _tokenService = tokenService;
        }


        public async Task<bool> UserExists(string username)
        {
            return await _accountDataAccess.UserExists(username);
        }

        public async Task<Token> Register(RegisterModel registerModel)
        {
            try
            {
                using var hmac = new HMACSHA512();
                var user = new AccountModel
                {
                    Username = registerModel.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerModel.Password)),
                    PasswordSalt = hmac.Key,
                    Role = registerModel.Role
                };

                await _accountDataAccess.Register(user);

                return new Token
                {
                    token = _tokenService.CreateToken(user)
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<Token> Login(LoginModel loginModel)
        {
            var user = await _accountDataAccess.Login(loginModel);
            if (user == null)
            {
                throw new Exception("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginModel.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    throw new Exception("Invalid password");
                }
            }

            return new Token
            {
                token = _tokenService.CreateToken(user)
            };
        }

        public async Task<bool> DeleteAccount(string username)
        {
            return await _accountDataAccess.DeleteAccount(username);
        }
    }
}
