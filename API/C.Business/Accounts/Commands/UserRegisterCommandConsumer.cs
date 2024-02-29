using Business.Accounts.Services;
using Contracts.DTOs;
using MediatR;

namespace Business.Accounts.Commands
{
    public class UserRegisterCommandConsumer : IRequestHandler<UserRegisterCommand, Token>
    {
        private readonly IAccountService _accountService;

        public UserRegisterCommandConsumer(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Token> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var registerModel = request.RegisterModel;

            if (await _accountService.UserExists(registerModel.Username))
            {
                throw new Exception("Username is taken");
            }

            var token = await _accountService.Register(registerModel);

            if (token is null)
            {
                throw new Exception("Register failed");
            }

            return token;
        }
    }
}