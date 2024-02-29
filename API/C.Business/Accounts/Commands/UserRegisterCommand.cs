using Amazon.Runtime.Internal;
using Contracts.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
