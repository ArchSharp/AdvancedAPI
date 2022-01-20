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
        public async Task<SuccessResponse<CreateEmployeeDto>> CreateEmployee(CreateEmployeeDto model)
        {
            // ReSharper disable once HeapView.ClosureAllocation
            var staff_id = model.StaffId;
            var isStaffIdExist = await _employeeRepository.ExistsAsync(x => x.StaffId == staff_id);
            int cnt_failed = 0;
            int cnt_total = 0;
            string[] failed = {""};
            
           
            if (isStaffIdExist)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.staffIdExist);

            //if (!isDepartmentIdExist)
              //  throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DepartmentExist);

            //check age
            var age = DateTime.Now.Year - model.DateOfBirth.Year;
            if (age < 16)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.AgeLimitError);

            var employee = _mapper.Map<Employee>(model);
            foreach (var item in model.DepartmentIds)
            {
                var department = await _departmentRepository.FirstOrDefault(x => x.Id == item);
                if (department == null)
                {
                    cnt_failed++;
                    cnt_total++;
                    failed[failed.Length - 1] = "Department with Id " + item.ToString() + "does not exist in the database";
                }
                else
                    employee.Departments.Add(department);
            }

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();
           
            var employee_verified = await _employeeRepository.QueryableEntity(x => x.Id == employee.Id)
                .Include(x => x.Departments).FirstOrDefaultAsync();
            var employeeResponse = _mapper.Map<CreateEmployeeDto>(employee_verified);

            return new SuccessResponse<CreateEmployeeDto>
            {
                Message = ResponseMessages.CreationSuccessResponse,
                Data = employeeResponse,
                ExtraInfo = failed 
            };
        }

        //To get employee by Id
        public async Task<SuccessResponse<GetEmployeeDto>> GetEmployeeById(Guid id)
        {
            var employee = await _employeeRepository.SingleOrDefaultNoTracking(x => x.Id == id);

            if (employee == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.EmployeeNotFound);

            var employeeResponse = _mapper.Map<GetEmployeeDto>(employee);

            return new SuccessResponse<GetEmployeeDto>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
                Data = employeeResponse
            };
        }

        // To update an employee data
        public async Task<SuccessResponse<UpdateEmployeeDto>> UpdateEmployeeById(Guid id, UpdateEmployeeDto model)
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
            var employeeResponse = _mapper.Map<UpdateEmployeeDto>(employee);

            await _employeeRepository.SaveChangesAsync();
            
            return new SuccessResponse<UpdateEmployeeDto>
            {
                Message = name+ResponseMessages.UpdateEmployeeByIdResponse,
                Data = employeeResponse
            };
        }

        // To delete an employee details
        public async Task<SuccessResponse<DeleteEmployeeDto>> DeleteEmployeeById(Guid id)
        {
            var employee = await _employeeRepository.SingleOrDefaultNoTracking(x => x.Id == id);
            
            if (employee == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.EmployeeNotFound);

            var name = employee.FirstName + " " + employee.LastName;
            _employeeRepository.Remove(employee);

            await _employeeRepository.SaveChangesAsync();

            var employeeResponse = _mapper.Map<DeleteEmployeeDto>(employee);

            return new SuccessResponse<DeleteEmployeeDto>
            {
                Message = name+ResponseMessages.DeleteEmployeeIdResponse,
                Data = employeeResponse
            };
        }

        // To get all search query
        public async Task<SuccessResponse<IEnumerable<SearchEmployeeDto>>> GetEmployeeBySearch(string search)
        {
            var employeeSearch = await _employeeRepository.FindAsync(x=>x.FirstName == search
                || x.LastName == search || x.StaffId == search);

            if (employeeSearch.Count() == 0)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.EmployeeNotFound);

            var employeeSearchResponse = _mapper.Map<IEnumerable<SearchEmployeeDto>>(employeeSearch);
            var count = employeeSearch.Count();
            
            return new SuccessResponse<IEnumerable<SearchEmployeeDto>>
            {
                Message = count+" "+ResponseMessages.RetrievalSuccessResponse,
                Data = employeeSearchResponse
            };
        }

        // service for getting all departments of an employee
        public async Task<SuccessResponse<GetEmployeeDepartmentsDto>> GetEmployeeDepartments(Guid id)
        {
            var employee = await _employeeRepository.QueryableEntity(x=>x.Id == id)
                ?.Include(x=>x.Departments)
                ?.FirstOrDefaultAsync();

            if (employee == null)
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.DepartmentNotExist);

            var departmentResponse = _mapper.Map<GetEmployeeDepartmentsDto>(employee);

            return new SuccessResponse<GetEmployeeDepartmentsDto>
            {
                Message = ResponseMessages.RetrievalSuccessResponse,
                Data = departmentResponse
            };
        }
    }
}
