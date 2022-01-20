using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using ShareLoanApp.Application.DTOs;
using ShareLoanApp.Domain.DTOs;
using ShareLoanApp.Domain.Entities;

namespace ShareLoanApp.Application.Mapper
{
    public class DepartmentMapper : Profile
    {
        public DepartmentMapper()
        {

            CreateMap<CreateDepartmentDto, Department>();


            //map for create departmentResponse
            CreateMap<Department, CreateDepartmentDto>();
            //map for get departmentResponse
            CreateMap<Department, GetDepartmentDto>();
            //map for update departmentResponse
            CreateMap<Department, UpdateDepartmentDto>();
            //map for delete departmentResponse
            CreateMap<Department, DeleteDepartmentDto>();
            //map for getDepartmentEmployeesbyId
            CreateMap<Department, GetDepartmentEmployeesDto>();
        }
    }
}
