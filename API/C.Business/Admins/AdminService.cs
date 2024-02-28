

using A.Contracts.Models;
using A.Contracts.Update_Models;
using Microsoft.AspNetCore.JsonPatch;

namespace C.Business.Admins
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminDataAccess;

        public AdminService(IAdminRepository adminDataAccess)
        {
            _adminDataAccess = adminDataAccess;
        }

        public async Task CreateNewAdmin(AdminModel admin)
        {
            await _adminDataAccess.CreateNewAdmin(admin);
            return;
        }

        public async Task<List<AdminModel>> GetAdmins(int pageNumber, int itemPerPage)
        {
            List<AdminModel> adminModels = await _adminDataAccess.GetAdmins(pageNumber, itemPerPage);
            return adminModels;
        }

        public async Task<AdminModel> GetAdmin(string username)
        {
            AdminModel adminModel = await _adminDataAccess.GetAdmin(username);
            return adminModel;
        }

        public async Task<long> TotalNumberOfAdmins(int pageNumber, int itemPerPage)
        {
            return await _adminDataAccess.TotalNumberOfAdmins(pageNumber, itemPerPage);
        }

        public async Task<bool> UpdateAdmin(string username, UpdateAdminModel admin)
        {
            return await _adminDataAccess.UpdateAdmin(username, admin);
        }

        public async Task<bool> DeleteAdmin(string username)
        {
            return await _adminDataAccess.DeleteAdmin(username);
        }

        public async Task<bool> DeleteAllAdmin()
        {
            return await _adminDataAccess.DeleteAllAdmin();
        }

        public async Task<bool> PartiallyUpdateAdmin(string username, JsonPatchDocument<AdminModel> patchDocument)
        {
            return await _adminDataAccess.PartiallyUpdateAdmin(username, patchDocument);
        }
    }
}
