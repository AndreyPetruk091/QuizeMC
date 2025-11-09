using QuizeMC.Application.Models.Admin;
using QuizeMC.Application.Models.Common;

namespace QuizeMC.Application.Services.Abstractions
{
    public interface IAdminApplicationService
    {
        Task<ApiResponse<AdminModel>> RegisterAsync(AdminCreateModel createModel);
        Task<ApiResponse<string>> LoginAsync(AdminLoginModel loginModel);
        Task<ApiResponse<AdminModel>> GetAdminAsync(Guid adminId);
        Task<ApiResponse> UpdateAdminAsync(Guid adminId, AdminUpdateModel updateModel);
        Task<ApiResponse> ChangePasswordAsync(Guid adminId, string currentPassword, string newPassword);
        Task<ApiResponse> DeactivateAdminAsync(Guid adminId);
        Task<ApiResponse> ActivateAdminAsync(Guid adminId);
        Task<ApiResponse<PagedResult<AdminModel>>> GetAdminsPagedAsync(PagedRequest request);
    }
}