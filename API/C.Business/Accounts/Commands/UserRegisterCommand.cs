using Contracts.DTOs;
using MediatR;

namespace Business.Accounts.Commands
{
    public class UserRegisterCommand : IRequest<Token>
    {
        public RegisterModel RegisterModel { get; set; }

        public UserRegisterCommand(RegisterModel registerModel)
        {
            RegisterModel = registerModel;
        }
    }
}
