using AutoMapper;
using QuizeMC.Domain.Services.Abstractions;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Application.Services.Abstractions;
using QuizeMC.Application.Models.Admin;
using QuizeMC.Application.Models.Common;
using QuizeMC.Application.Services.Common;

namespace QuizeMC.Application.Services
{
    public class AdminApplicationService : IAdminApplicationService
    {
        private readonly IAdminService _adminService;
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AdminApplicationService(
            IAdminService adminService,
            IAdminRepository adminRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _adminService = adminService;
            _adminRepository = adminRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<AdminModel>> RegisterAsync(AdminCreateModel createModel)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Валидация
                if (createModel.Password != createModel.ConfirmPassword)
                {
                    return ApiResponse<AdminModel>.Fail("Password and confirmation do not match");
                }

                var email = new Email(createModel.Email);
                var admin = await _adminService.CreateAdminAsync(email, createModel.Password);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var adminModel = _mapper.Map<AdminModel>(admin);
                return ApiResponse<AdminModel>.Ok(adminModel, "Admin registered successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<AdminModel>.Fail($"Registration failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> LoginAsync(AdminLoginModel loginModel)
        {
            try
            {
                var email = new Email(loginModel.Email);
                var isValid = await _adminService.ValidateAdminCredentialsAsync(email, loginModel.Password);

                if (!isValid)
                {
                    return ApiResponse<string>.Fail("Invalid email or password");
                }

                var admin = await _adminRepository.GetByEmailAsync(loginModel.Email);
                admin.UpdateLoginTime();
                _adminRepository.Update(admin);
                await _unitOfWork.SaveChangesAsync();

                // В реальном приложении здесь генерировался бы JWT токен
                var token = $"mock-jwt-token-for-{admin.Id}";
                return ApiResponse<string>.Ok(token, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail($"Login failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminModel>> GetAdminAsync(Guid adminId)
        {
            try
            {
                var admin = await _adminRepository.GetWithQuizzesAsync(adminId);
                if (admin == null)
                {
                    return ApiResponse<AdminModel>.Fail("Admin not found");
                }

                var adminModel = _mapper.Map<AdminModel>(admin);
                return ApiResponse<AdminModel>.Ok(adminModel);
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminModel>.Fail($"Failed to get admin: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateAdminAsync(Guid adminId, AdminUpdateModel updateModel)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var admin = await _adminRepository.GetByIdAsync(adminId);
                if (admin == null)
                {
                    return ApiResponse.Fail("Admin not found");
                }

                if (!string.IsNullOrEmpty(updateModel.Email))
                {
                    var newEmail = new Email(updateModel.Email);
                    admin.UpdateEmail(newEmail);
                }

                _adminRepository.Update(admin);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse.Ok("Admin updated successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse.Fail($"Failed to update admin: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ChangePasswordAsync(Guid adminId, string currentPassword, string newPassword)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var admin = await _adminRepository.GetByIdAsync(adminId);
                if (admin == null)
                {
                    return ApiResponse.Fail("Admin not found");
                }

                await _adminService.ChangeAdminPasswordAsync(admin, currentPassword, newPassword);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse.Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse.Fail($"Failed to change password: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeactivateAdminAsync(Guid adminId)
        {
            try
            {
                var admin = await _adminRepository.GetByIdAsync(adminId);
                if (admin == null)
                {
                    return ApiResponse.Fail("Admin not found");
                }

                admin.Deactivate();
                _adminRepository.Update(admin);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse.Ok("Admin deactivated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail($"Failed to deactivate admin: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ActivateAdminAsync(Guid adminId)
        {
            try
            {
                var admin = await _adminRepository.GetByIdAsync(adminId);
                if (admin == null)
                {
                    return ApiResponse.Fail("Admin not found");
                }

                admin.Activate();
                _adminRepository.Update(admin);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse.Ok("Admin activated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail($"Failed to activate admin: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResult<AdminModel>>> GetAdminsPagedAsync(PagedRequest request)
        {
            try
            {
                var admins = await _adminRepository.GetAllAsync();
                var adminsList = admins.ToList();

                // Применяем пагинацию
                var pagedAdmins = adminsList
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var adminModels = _mapper.Map<List<AdminModel>>(pagedAdmins);
                var result = new PagedResult<AdminModel>(
                    adminModels,
                    adminsList.Count,
                    request.PageNumber,
                    request.PageSize
                );

                return ApiResponse<PagedResult<AdminModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<AdminModel>>.Fail($"Failed to get admins: {ex.Message}");
            }
        }
    }
}