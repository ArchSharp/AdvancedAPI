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
    [Route("api/v{version:apiVersion}/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        
        /// <summary>
        /// Endpoint to create an employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(SuccessResponse<CreateEmployeeDtoResponse>), 201)]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto model)
        {
            var response = await _employeeService.CreateEmployee(model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetEmployeeById))]
        [ProducesResponseType(typeof(SuccessResponse<GetEmployeeDtoResponse>), 200)]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var response = await _employeeService.GetEmployeeById(id);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to update an employee
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<UpdateEmployeeDtoResponse>), 200)]
        public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployeeDto model)
        {
            var response = await _employeeService.UpdateEmployeeById(id, model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to delete an employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = nameof(DeleteEmployeeById))]
        [ProducesResponseType(typeof(SuccessResponse<DeleteEmployeeDtoResponse>), 201)]
        public async Task<IActionResult> DeleteEmployeeById(Guid id)
        {
            var response = await _employeeService.DeleteEmployeeById(id);

            return Ok(response);
        }

        // To get all employee by search
        /// <summary>
        /// Endpoint to search an employee
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("search")]
        [ProducesResponseType(typeof(SuccessResponse<SearchEmployeeDtoResponse>), 200)]
        public async Task<IActionResult> GetEmployeeBySearch(string search)
        {
            var response = await _employeeService.GetEmployeeBySearch(search);

            return Ok(response);
        }
    }
}