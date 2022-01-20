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
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Endpoint to create a department
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(SuccessResponse<CreateDepartmentDto>), 200)]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto model)
        {
            var response = await _departmentService.CreateDepartment(model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get a department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<GetDepartmentDto>), 200)]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            var response = await _departmentService.GetDepartmentById(id);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to update a department
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<UpdateDepartmentDto>), 200)]
        public async Task<IActionResult> UpdateDepartmentById(Guid id, UpdateDepartmentDto model)
        {
            var response = await _departmentService.UpdateDepartmentById(id, model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to delete a department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<DeleteDepartmentDto>), 200)]
        public async Task<IActionResult> DeleteDepartmentById(Guid id)
        {
            var response = await _departmentService.DeleteDepartmentById(id);

            return Ok(response);
        }
        /// <summary>
        /// Endpoint to get all the employees under a departments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/employees")]
        [ProducesResponseType(typeof(SuccessResponse<GetDepartmentEmployeesDto>), 200)]
        public async Task<IActionResult> GetDepartmentEmployees(Guid id)
        {
            var response = await _departmentService.GetDepartmentEmployees(id);

            return Ok(response);
        }
    }
}