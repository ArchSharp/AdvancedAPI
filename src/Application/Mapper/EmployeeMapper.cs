using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using ShareLoanApp.Application.DTOs;
using ShareLoanApp.Domain.DTOs;
using ShareLoanApp.Domain.Entities;

namespace ShareLoanApp.Application.Mapper
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper()
        {
            
            //map for createEmployeebyId
            CreateMap<CreateEmployeeDto, Employee>();
            //map for UpdateEmployee
            CreateMap<UpdateEmployeeDto, Employee>();
            //map for create department
            CreateMap<CreateDepartmentDto, Department>();


            //map for getEmployeebyIdResponse
            CreateMap<Employee, GetEmployeeDtoResponse>();
            //map for updateEmployeeResponse
            CreateMap<Employee, UpdateEmployeeDtoResponse>();
            //map for CreateEmployeeResponse
            CreateMap<Employee, CreateEmployeeDtoResponse>();
            //map for searchemployee
            CreateMap<Employee, SearchEmployeeDtoResponse>();
            //map for delete employee
            CreateMap<Employee, DeleteEmployeeDtoResponse>();
            //map for create departmentResponse
            CreateMap<Department, CreateDepartmentDtoResponse>();

        }
    }
}
