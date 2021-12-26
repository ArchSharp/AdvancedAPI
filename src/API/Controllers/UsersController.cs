using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareLoanApp.Application.Helpers;
using ShareLoanApp.Application.Services.Interfaces;
using System.Threading.Tasks;
using ShareLoanApp.Application.DTOs;
using ShareLoanApp.Domain.DTOs;

namespace ShareLoanApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Endpoint to register a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(SuccessResponse<CreateUserResponse>), 201)]
        public async Task<IActionResult> RegisterUser(CreateUserDTO model)
        {
            var response = await _userService.CreateUser(model);

            return CreatedAtAction(nameof(GetUserById), new {id = response.Data.Id}, response);
        }

        /// <summary>
        /// Endpoint to login a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(SuccessResponse<UserLoginResponse>), 200)]
        public async Task<IActionResult> LoginUser(UserLoginDTO model)
        {
            var response = await _userService.Login(model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetUserById))]
        [ProducesResponseType(typeof(SuccessResponse<UserLoginResponse>), 200)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = await _userService.GetUserById(id);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to generate a new access and refresh token
        /// </summary>
        /// <param name="mdoel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(typeof(SuccessResponse<RefreshTokenResponse>), 200)]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO mdoel)
        {
            var response = await _userService.GetRefreshToken(mdoel);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to initializes password reset
        /// </summary>
        /// <param name="mdoel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> ForgotPassword(ResetPasswordDTO mdoel)
        {
            var response = await _userService.ResetPassword(mdoel);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to verify token
        /// </summary>
        /// <param name="mdoel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("verify-token")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> VerifyToken(VerifyTokenDTO mdoel)
        {
            var response = await _userService.VerifyToken(mdoel);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to set password
        /// </summary>
        /// <param name="mdoel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("set-password")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> SetPassword(SetPasswordDTO mdoel)
        {
            var response = await _userService.SetPassword(mdoel);

            return Ok(response);
        }
    }
}
