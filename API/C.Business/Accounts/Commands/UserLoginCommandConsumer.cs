

using Business.Accounts.Services;
using Contracts.DTOs;
using MediatR;

namespace Business.Accounts.Commands
{
    public class UserLoginCommandConsumer : IRequestHandler<UserLoginCommand, Token>
    {
        private readonly IAccountService _accountService;

        public UserLoginCommandConsumer(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Token> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var loginModel = request.LoginModel;

            var token = await _accountService.Login(loginModel);

            if (token is null)
            {
                throw new Exception("Login failed");
            }

            return token;
        }
    }
}
