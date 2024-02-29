

using MediatR;

namespace Business.Accounts.Commands
{
    public class UserDeleteCommand : IRequest<bool>
    {
        public string Username { get; set; }

        public UserDeleteCommand(string username)
        {
            Username = username;
        }
    }
}
