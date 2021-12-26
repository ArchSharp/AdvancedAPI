using AutoMapper;
using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Entities.Identities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ShareLoanApp.Application.DTOs;
using ShareLoanApp.Application.Helpers;
using ShareLoanApp.Application.Services.Interfaces;
using ShareLoanApp.Domain.Entities;

namespace ShareLoanApp.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserActivity> _userActivityRepository;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Tokens> _tokenRepository;
        private readonly IConfiguration _configuration;

        public UserService(
            IRepository<User> userRepository, 
            IRepository<UserActivity> userActivityRepository,
            IJwtAuthenticationManager jwtAuthenticationManager,
            UserManager<User> userManager,
            IMapper mapper,
            IRepository<Tokens> tokenRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userActivityRepository = userActivityRepository;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _mapper = mapper;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        public async Task<SuccessResponse<CreateUserResponse>> CreateUser(CreateUserDTO model, IEnumerable<string> roles = null)
        {
            // ReSharper disable once HeapView.ClosureAllocation
            var email = model.Email.Trim().ToLower();
            var isEmailExist = await _userRepository.ExistsAsync(x => x.Email == email);

             if (isEmailExist)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DuplicateEmail);

             if (string.IsNullOrEmpty(model.Password))
                 throw new RestException(HttpStatusCode.BadGateway, message: ResponseMessages.PasswordCannotBeEmpty);

            var user = _mapper.Map<User>(model);
            
            await _userManager.CreateAsync(user, model.Password);
            
            if (roles is not null)
                await AddUserToRoles(user, roles);

            var userActivity = new UserActivity
            {
                EventType = "User created",
                UserId = user.Id,
                ObjectClass = "USER",
                Details = "signed up",
                ObjectId = user.Id
            };

            await _userActivityRepository.AddAsync(userActivity);
            await _userActivityRepository.SaveChangesAsync();

            var userResponse = _mapper.Map<CreateUserResponse>(user);

            return new SuccessResponse<CreateUserResponse>
            {
                Message = ResponseMessages.CreationSuccessResponse,
                Data = userResponse
            };
        }
        public async Task<SuccessResponse<UserLoginResponse>> Login(UserLoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.WrongEmailOrPassword);

            // ReSharper disable once HeapView.BoxingAllocation
            if (!user.EmailConfirmed || user?.Status?.ToUpper() != EUserStatus.ACTIVE.ToString() || !user.Verified)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.WrongEmailOrPassword);

            var isUserValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isUserValid)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.WrongEmailOrPassword);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var userActivity = new UserActivity
            {
                EventType = "User Login",
                UserId = user.Id,
                ObjectClass = "USER",
                Details = "logged in",
                ObjectId = user.Id
            };

            var roles = await _userManager.GetRolesAsync(user);
            await _userActivityRepository.AddAsync(userActivity);
            await _userActivityRepository.SaveChangesAsync();

            var userViewModel = _mapper.Map<UserLoginResponse>(user);

            var tokenResponse = _jwtAuthenticationManager.Authenticate(user, roles);
            userViewModel.AccessToken = tokenResponse.AccessToken;
            userViewModel.ExpiresIn = tokenResponse.ExpiresIn;
            userViewModel.RefreshToken = _jwtAuthenticationManager.GenerateRefreshToken(user.Id);

            return new SuccessResponse<UserLoginResponse>
            {
                Message = ResponseMessages.LoginSuccessResponse,
                Data = userViewModel
            };
        }
        public async Task<SuccessResponse<UserByIdResponse>> GetUserById(Guid userId)
        {
            var user = await _userRepository.SingleOrDefaultNoTracking(x => x.Id == userId);

            if (user == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            var userResponse = _mapper.Map<UserByIdResponse>(user);

            return new SuccessResponse<UserByIdResponse>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
                Data    = userResponse
            };
        }
        public async Task<SuccessResponse<RefreshTokenResponse>> GetRefreshToken(RefreshTokenDTO model)
        {
            var userId = _jwtAuthenticationManager.GetUserIdFromAccessToken(model.AccessToken);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            var isRefreshTokenValid = _jwtAuthenticationManager.ValidateRefreshToken(model.RefreshToken);
            if (!isRefreshTokenValid)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.InvalidToken);

            var tokenResponse = _jwtAuthenticationManager.Authenticate(user);

            var newRefreshToken = _jwtAuthenticationManager.GenerateRefreshToken(user.Id);

            var tokenViewModel = new RefreshTokenResponse
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = tokenResponse.ExpiresIn
            };

            return  new SuccessResponse<RefreshTokenResponse>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
                Data = tokenViewModel
            };

        }
        public async Task<SuccessResponse<object>> ResetPassword(ResetPasswordDTO model)
        {
            var user = await _userRepository.FirstOrDefault(x => x.Email == model.Email);
            if (user == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            var token = new Tokens
            {
                UserId = user.Id,
                Token = CustomToken.GenerateToken()
            };
            await _tokenRepository.AddAsync(token);

            var userActivity = new UserActivity
            {
                EventType = "Password Reset Request",
                UserId = user.Id,
                ObjectClass = "USER",
                Details = "requested for password reset",
                ObjectId = user.Id
            };
            
            await _userActivityRepository.AddAsync(userActivity);

            await _tokenRepository.SaveChangesAsync();

            //Send email to queue
            return new SuccessResponse<object>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
            };
        }
        public async Task<SuccessResponse<object>> VerifyToken(VerifyTokenDTO model)
        {
            var token = await _tokenRepository.FirstOrDefault(x => x.Token == model.Token);
            if (token == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.InvalidToken);

            var tokenLifeSpan = double.Parse(_configuration["TOKEN_LIFESPAN"]);

            var isValid = CustomToken.IsTokenValid(token, tokenLifeSpan);
            if(!isValid)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.InvalidToken);

            return new SuccessResponse<object>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
            };
        }
        public async Task<SuccessResponse<object>> SetPassword(SetPasswordDTO model)
        {
            var token = await _tokenRepository.FirstOrDefault(x => x.Token == model.Token);
            if (token == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.InvalidToken);

            var tokenLifeSpan = double.Parse(_configuration["TOKEN_LIFESPAN"]);

            var isValid = CustomToken.IsTokenValid(token, tokenLifeSpan);
            if (!isValid)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.InvalidToken);

            _tokenRepository.Remove(token);

            var user = await _userRepository.GetByIdAsync(token.UserId);
            user.UpdatedAt = DateTime.UtcNow;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            user.Verified = true;
            user.EmailConfirmed = true;
            _userRepository.Update(user);

            var userActivity = new UserActivity
            {
                EventType = "Set Password",
                UserId = user.Id,
                Details = "set password",
                ObjectClass = "USER",
                ObjectId = user.Id
            };
            await _userActivityRepository.AddAsync(userActivity);           

            await _tokenRepository.SaveChangesAsync();

            return new SuccessResponse<object>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
            };
        }
        
        #region Private Functions
        private async Task AddUserToRoles(User user, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                if(!await _userManager.IsInRoleAsync(user, role))
                    await _userManager.AddToRoleAsync(user, role);
            }
        }

        #endregion
    }
}
