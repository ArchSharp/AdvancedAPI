using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShareLoanApp.Application.DTOs;
using ShareLoanApp.Application.Helpers;

namespace ShareLoanApp.Application.Services.Interfaces
{
    public interface IDepartmentService : IAutoDependencyService
    {
        Task<SuccessResponse<CreateDepartmentDto>> CreateDepartment(CreateDepartmentDto model);
        Task<SuccessResponse<GetDepartmentDto>> GetDepartmentById(Guid id);
        Task<SuccessResponse<UpdateDepartmentDto>> UpdateDepartmentById(Guid id, UpdateDepartmentDto model);
        Task<SuccessResponse<DeleteDepartmentDto>> DeleteDepartmentById(Guid id);

        Task<SuccessResponse<GetDepartmentEmployeesDto>> GetDepartmentEmployees(Guid id);
    }
}