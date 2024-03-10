

using Business.Accounts.Services;
using Contracts.DTOs;
using MassTransit;
using MediatR;
using SchoolManagement.Shared.CQRS;

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

    public class UserLoginDynamicCommandConsumer : ADynamicCommandConsumer
    {
        private readonly IAccountService _accountService;

        public UserLoginDynamicCommandConsumer(IAccountService accountService)
        {
            _accountService = accountService;
        }

        protected override async Task<DynamicCommandResponse> ExecuteAsync(DynamicCommand command, ConsumeContext<DynamicCommand> context = null)
        {
            var loginModel = command.GetValue<LoginModel>("LoginModel");

            var token = await _accountService.Login(loginModel);

            if (token is null)
            {
                throw new Exception("Login failed");
            }

            var response = command.CreateResponse();

            response
                .SetValue("Token", token.token)
                .SetValue("Message", "Token Created Successfully");

            return response;
        }
    }
}
