using Business.Accounts.Services;
using MediatR;

namespace Business.Accounts.Commands
{
    public class UserDeleteCommandConsumer : IRequestHandler<UserDeleteCommand, bool>
    {
        private readonly IAccountService _accountService;
        public UserDeleteCommandConsumer(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<bool> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var username = request.Username;

            bool result = await _accountService.DeleteAccount(username);

            return result;
        }
    }
}
