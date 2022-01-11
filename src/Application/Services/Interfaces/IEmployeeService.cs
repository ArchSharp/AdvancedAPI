using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShareLoanApp.Application.DTOs;
using ShareLoanApp.Application.Helpers;

namespace ShareLoanApp.Application.Services.Interfaces
{
    public interface IEmployeeService: IAutoDependencyService
    {
        Task<SuccessResponse<CreateEmployeeDtoResponse>> CreateEmployee(CreateEmployeeDto model);
        Task<SuccessResponse<UpdateEmployeeDtoResponse>> UpdateEmployeeById(Guid id, UpdateEmployeeDto model);
        Task<SuccessResponse<GetEmployeeDtoResponse>> GetEmployeeById(Guid id);
        Task<SuccessResponse<DeleteEmployeeDtoResponse>> DeleteEmployeeById(Guid id);
        Task<SuccessResponse<IEnumerable<SearchEmployeeDtoResponse>>> GetEmployeeBySearch(string search);
        Task<SuccessResponse<CreateDepartmentDtoResponse>> CreateDepartment(CreateDepartmentDto model);
    }
}