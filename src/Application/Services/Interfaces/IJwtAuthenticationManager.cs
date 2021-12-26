using System;
using System.Collections.Generic;
using Domain.Entities.Identities;
using ShareLoanApp.Application.Helpers;
using ShareLoanApp.Application.Services.Implementations;
using ShareLoanApp.Domain.Entities;

namespace ShareLoanApp.Application.Services.Interfaces
{
    public interface IJwtAuthenticationManager: IAutoDependencyService
    {
        TokenReturnHelper Authenticate(User user, IList<string> roles =null);
        Guid GetUserIdFromAccessToken(string accessToken);
        string GenerateRefreshToken(Guid userId);
        bool ValidateRefreshToken(string refreshToken);
    }
}
