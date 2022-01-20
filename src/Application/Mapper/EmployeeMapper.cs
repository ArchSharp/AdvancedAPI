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


            //map for getEmployeebyIdResponse
            CreateMap<Employee, GetEmployeeDto>();
            //map for updateEmployeeResponse
            CreateMap<Employee, UpdateEmployeeDto>();
            //map for CreateEmployeeResponse
            CreateMap<Employee, CreateEmployeeDto>();
            //map for searchemployee
            CreateMap<Employee, SearchEmployeeDto>();
            //map for delete employee
            CreateMap<Employee, DeleteEmployeeDto>();
            //map for getEmployeebyIdResponse
            CreateMap<Employee, GetEmployeeDepartmentsDto>();
            
        }
    }
}
