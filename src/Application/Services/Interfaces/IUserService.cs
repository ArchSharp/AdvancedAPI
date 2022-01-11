using ShareLoanApp.Application.DTOs;
using ShareLoanApp.Application.Helpers;
using ShareLoanApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareLoanApp.Application.Services.Interfaces
{
    public interface IUserService: IAutoDependencyService
    {
        Task<SuccessResponse<CreateUserResponse>> CreateUser(CreateUserDTO model, IEnumerable<string> roles = null);
        Task<SuccessResponse<UserLoginResponse>> Login(UserLoginDTO model);
        Task<SuccessResponse<UserByIdResponse>> GetUserById(Guid userId);
        Task<SuccessResponse<RefreshTokenResponse>> GetRefreshToken(RefreshTokenDTO model);
        Task<SuccessResponse<object>> ResetPassword(ResetPasswordDTO model);
        Task<SuccessResponse<object>> VerifyToken(VerifyTokenDTO model);
        Task<SuccessResponse<object>> SetPassword(SetPasswordDTO model);
    }
}
