using Contracts.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
