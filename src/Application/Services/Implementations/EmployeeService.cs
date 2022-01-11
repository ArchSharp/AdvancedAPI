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

namespace ShareLoanApp.Application.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
                IRepository<Employee> employeeRepository,
                IRepository<Department> departmentRepository,
                IMapper mapper
        )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        // To create an Employee
        public async Task<SuccessResponse<CreateEmployeeDtoResponse>> CreateEmployee(CreateEmployeeDto model)
        {
            // ReSharper disable once HeapView.ClosureAllocation
            var staff_id = model.StaffId;
            var isStaffIdExist = await _employeeRepository.ExistsAsync(x => x.StaffId == staff_id);

            var department_id = model.DepartmentId;
            var isDepartmentIdExist = await _departmentRepository.ExistsAsync(x => x.Id == department_id);

            if (isStaffIdExist)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.staffIdExist);

            if (!isDepartmentIdExist)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DepartmentExist);

            //check age
            var age = DateTime.Now.Year - model.DateOfBirth.Year;
            if (age < 16)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.AgeLimitError);

            var employee = _mapper.Map<Employee>(model);

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            var employeeResponse = _mapper.Map<CreateEmployeeDtoResponse>(employee);

            return new SuccessResponse<CreateEmployeeDtoResponse>
            {
                Message = ResponseMessages.CreationSuccessResponse,
                Data = employeeResponse
            };
        }

        //To get employee by Id
        public async Task<SuccessResponse<GetEmployeeDtoResponse>> GetEmployeeById(Guid id)
        {
            var employee = await _employeeRepository.SingleOrDefaultNoTracking(x => x.Id == id);

            if (employee == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.EmployeeNotFound);

            var employeeResponse = _mapper.Map<GetEmployeeDtoResponse>(employee);

            return new SuccessResponse<GetEmployeeDtoResponse>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
                Data = employeeResponse
            };
        }

        // To update an employee data
        public async Task<SuccessResponse<UpdateEmployeeDtoResponse>> UpdateEmployeeById(Guid id, UpdateEmployeeDto model)
        {
            var employee = await _employeeRepository.SingleOrDefault(x => x.Id == id);
            var name = employee.FirstName + " " + employee.LastName;

            if (employee == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.EmployeeNotFound);

            //check age
            var age = DateTime.Now.Year - model.DateOfBirth.Year;
            if (age < 16)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.AgeLimitError);

            _mapper.Map(model, employee);
            var employeeResponse = _mapper.Map<UpdateEmployeeDtoResponse>(employee);

            await _employeeRepository.SaveChangesAsync();
            
            return new SuccessResponse<UpdateEmployeeDtoResponse>
            {
                Message = name+ResponseMessages.UpdateEmployeeByIdResponse,
                Data = employeeResponse
            };
        }

        // To delete an employee details
        public async Task<SuccessResponse<DeleteEmployeeDtoResponse>> DeleteEmployeeById(Guid id)
        {
            var employee = await _employeeRepository.SingleOrDefaultNoTracking(x => x.Id == id);
            
            if (employee == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.EmployeeNotFound);

            var name = employee.FirstName + " " + employee.LastName;
            _employeeRepository.Remove(employee);

            await _employeeRepository.SaveChangesAsync();

            var employeeResponse = _mapper.Map<DeleteEmployeeDtoResponse>(employee);

            return new SuccessResponse<DeleteEmployeeDtoResponse>
            {
                Message = name+ResponseMessages.DeleteEmployeeIdResponse,
                Data = employeeResponse
            };
        }

        // To get all search query
        public async Task<SuccessResponse<IEnumerable<SearchEmployeeDtoResponse>>> GetEmployeeBySearch(string search)
        {
            var employeeSearch = await _employeeRepository.FindAsync(x=>x.FirstName == search
                || x.LastName == search || x.StaffId == search);

            if (employeeSearch.Count() == 0)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.EmployeeNotFound);

            var employeeSearchResponse = _mapper.Map<IEnumerable<SearchEmployeeDtoResponse>>(employeeSearch);
            var count = employeeSearch.Count();
            
            return new SuccessResponse<IEnumerable<SearchEmployeeDtoResponse>>
            {
                Message = count+" "+ResponseMessages.RetrievalSuccessResponse,
                Data = employeeSearchResponse
            };
        }

        // Controller for Department CRUD
        public async Task<SuccessResponse<CreateDepartmentDtoResponse>> CreateDepartment(CreateDepartmentDto model)
        {
            // ReSharper disable once HeapView.ClosureAllocation
            var department_name = model.Name;
            var isDepartmentExist = await _employeeRepository.ExistsAsync(x => x.Name == department_name);

            if (isDepartmentExist)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DepartmentExist);

            var department = _mapper.Map<Department>(model);

            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveChangesAsync();

            var departmentResponse = _mapper.Map<CreateDepartmentDtoResponse>(department);

            return new SuccessResponse<CreateDepartmentDtoResponse>
            {
                Message = ResponseMessages.CreationSuccessResponse,
                Data = departmentResponse
            };
        }
    }
}
