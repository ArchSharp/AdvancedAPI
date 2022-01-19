using Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShareLoanApp.Application.DTOs
{
    public class CreateEmployeeDtoResponse
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String StaffId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        IEnumerable<GetDepartmentDtoResponse> Departments { get; set; }
    }

    public class GetEmployeeDtoResponse : CreateEmployeeDtoResponse
    {
     
    }

    public class UpdateEmployeeDtoResponse : CreateEmployeeDtoResponse
    {
     
    }

    public class DeleteEmployeeDtoResponse : CreateEmployeeDtoResponse
    {
        
    }

    public class SearchEmployeeDtoResponse : CreateEmployeeDtoResponse
    {
        
    }
    public class CreateDepartmentDtoResponse : CreateDepartmentDto
    {
        public Guid Id { get; set; }
    }
    public class GetDepartmentDtoResponse : CreateDepartmentDtoResponse
    {
        
    }

    public class UpdateDepartmentDtoResponse : CreateDepartmentDtoResponse
    {

    }

    public class DeleteDepartmentDtoResponse : CreateDepartmentDtoResponse
    {

    }
    public class GetEmployeeDepartmentDtoResponse
    {
        public Guid[] Id { get; set; }
    }
}
