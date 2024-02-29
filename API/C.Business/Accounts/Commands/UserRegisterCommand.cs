using A.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.Business.Accounts.Commands
{
    public class UserRegisterCommand
    {
        public RegisterModel RegisterModel { get; set; }

        public UserRegisterCommand(RegisterModel registerModel)
        {
            RegisterModel = registerModel;
        }
    }
}
