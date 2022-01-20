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
using Domain.Entities;
using System.Collections;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ShareLoanApp.Application.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(
                IRepository<Employee> employeeRepository,
                IRepository<Department> departmentRepository,
                IMapper mapper
        )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }



        // service for Department CRUD
        public async Task<SuccessResponse<CreateDepartmentDto>> CreateDepartment(CreateDepartmentDto model)
        {
            // ReSharper disable once HeapView.ClosureAllocation
            var department_name = model.Name;
            var isDepartmentExist = await _departmentRepository.ExistsAsync(x => x.Name == department_name);

            if (isDepartmentExist)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DepartmentExist);

            var department = _mapper.Map<Department>(model);

            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveChangesAsync();

            var departmentResponse = _mapper.Map<CreateDepartmentDto>(department);

            return new SuccessResponse<CreateDepartmentDto>
            {
                Message = department_name + ResponseMessages.DepartmentCreationSuccessResponse,
                Data = departmentResponse
            };
        }

        //service to get department
        public async Task<SuccessResponse<GetDepartmentDto>> GetDepartmentById(Guid id)
        {
            var check_department_id = await _departmentRepository.SingleOrDefaultNoTracking(x => x.Id == id);

            if (check_department_id == null)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DepartmentNotExist);

            var departmentResponse = _mapper.Map<GetDepartmentDto>(check_department_id);

            return new SuccessResponse<GetDepartmentDto>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
                Data = departmentResponse
            };
        }

        //service to update department
        public async Task<SuccessResponse<UpdateDepartmentDto>> UpdateDepartmentById(Guid id, UpdateDepartmentDto model)
        {
            var department = await _departmentRepository.SingleOrDefault(x => x.Id == id);
            var department_name = department.Name;

            if (department == null)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DepartmentNotExist);

            _mapper.Map(model, department);
            var departmentResponse = _mapper.Map<UpdateDepartmentDto>(department);
            await _departmentRepository.SaveChangesAsync();

            return new SuccessResponse<UpdateDepartmentDto>
            {
                Message = department_name + ResponseMessages.UpdateEmployeeByIdResponse,
                Data = departmentResponse
            };
        }

        //service to delete department
        public async Task<SuccessResponse<DeleteDepartmentDto>> DeleteDepartmentById(Guid id)
        {
            var department = await _departmentRepository.SingleOrDefaultNoTracking(x => x.Id == id);
            var department_name = department.Name;

            if (department == null)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DepartmentNotExist);

            _departmentRepository.Remove(department);
            await _employeeRepository.SaveChangesAsync();

            var departmentResponse = _mapper.Map<DeleteDepartmentDto>(department);

            return new SuccessResponse<DeleteDepartmentDto>
            {
                Message = department_name + ResponseMessages.DeleteDepartmentByIdResponse,
                Data = departmentResponse
            };
        }

        // service for getting all employees under a department
        public async Task<SuccessResponse<GetDepartmentEmployeesDto>> GetDepartmentEmployees(Guid id)
        {
            var department = await _departmentRepository.QueryableEntity(x => x.Id == id)
                ?.Include(x => x.Employees)
                ?.FirstOrDefaultAsync();

            if (department == null)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DepartmentNotExist);

            var departmentResponse = _mapper.Map<GetDepartmentEmployeesDto>(department);

            return new SuccessResponse<GetDepartmentEmployeesDto>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
                Data = departmentResponse
            };
        }
    }
}
