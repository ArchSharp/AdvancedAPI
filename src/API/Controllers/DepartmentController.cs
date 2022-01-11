using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareLoanApp.Application.Helpers;
using ShareLoanApp.Application.Services.Interfaces;
using System.Threading.Tasks;
using ShareLoanApp.Application.DTOs;

namespace ShareLoanApp.API.Controllers
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/department")]
    public class DepartmentController : ControllerBase
    {
        private readonly IEmployeeService _departmentService;

        public DepartmentController(IEmployeeService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Endpoint to create a department
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(SuccessResponse<CreateDepartmentDtoResponse>), 200)]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto model)
        {
            var response = await _departmentService.CreateDepartment(model);

            return Ok(response);
        }
    }
}