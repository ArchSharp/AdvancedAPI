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
        Task<SuccessResponse<CreateEmployeeDto>> CreateEmployee(CreateEmployeeDto model);
        Task<SuccessResponse<UpdateEmployeeDto>> UpdateEmployeeById(Guid id, UpdateEmployeeDto model);
        Task<SuccessResponse<GetEmployeeDto>> GetEmployeeById(Guid id);
        Task<SuccessResponse<DeleteEmployeeDto>> DeleteEmployeeById(Guid id);
        Task<SuccessResponse<IEnumerable<SearchEmployeeDto>>> GetEmployeeBySearch(string search);
        Task<SuccessResponse<GetEmployeeDepartmentsDto>> GetEmployeeDepartments(Guid id);
    }
}