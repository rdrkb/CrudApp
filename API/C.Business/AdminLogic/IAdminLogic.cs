

using A.Contracts.Models;
using A.Contracts.Update_Models;
using Microsoft.AspNetCore.JsonPatch;

namespace C.Business.AdminLogic
{
    public interface IAdminLogic
    {
        Task CreateNewAdmin(AdminModel admin);
        Task<List<AdminModel>> GetAdmins(int pageNumber, int itemPerPage);
        Task<AdminModel> GetAdmin(string username);
        Task<long> TotalNumberOfAdmins(int pageNumber, int itemPerPage);
        Task<bool> UpdateAdmin(string username, UpdateAdminModel admin);
        Task<bool> DeleteAdmin(string username);
        Task<bool> DeleteAllAdmin();
        Task<bool> PartiallyUpdateAdmin(string username, JsonPatchDocument<AdminModel> patchDocument);
    }
}
