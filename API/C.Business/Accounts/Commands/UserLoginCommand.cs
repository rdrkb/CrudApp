using Contracts.DTOs;
using MediatR;

namespace Business.Accounts.Commands
{
    public class UserLoginCommand : IRequest<Token>
    {
        public LoginModel LoginModel { get; set; }

        public UserLoginCommand(LoginModel loginModel)
        {
            LoginModel = loginModel;
        }
    }
}
