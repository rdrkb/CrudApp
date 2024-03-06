using Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Business.Students.Commands
{
    public class PartiallyUpdateStudentCommand : IRequest<bool>
    {
        public string Username { get; }
        public JsonPatchDocument<StudentModel> PatchDocument { get; }
        public PartiallyUpdateStudentCommand(string username, JsonPatchDocument<StudentModel> patchDocument)
        {
            Username = username;
            PatchDocument = patchDocument;
        }
        
    }
}
