using A.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.Business.Accounts.Commands
{
    public class UserRegisterCommandConsumer 
    {
        private readonly IAccountService _accountService;
        
        public UserRegisterCommandConsumer(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Token> ExecuteAsync(UserRegisterCommand command)
        {
            var registerModel = command.RegisterModel;

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


// mediator design pattern

// mediator accept class, resolve the class handler. and handler execute 

// class -> class handler