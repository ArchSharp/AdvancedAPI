using Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShareLoanApp.Application.DTOs
{
    public class CreateEmployeeDto
    {
        public String Name { get; set; }
        public String StaffId { get; set; }
        public String FirstName { get; set; }
        public Guid[] DepartmentIds { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String State { get; set; }
    }

    public class UpdateEmployeeDto : CreateEmployeeDto
    {
        
    }
    public class DeleteEmployeeDto : CreateDepartmentDto
    {

    }
    public class SearchEmployeeDto : CreateEmployeeDto
    {

    }

    public class GetEmployeeDto
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
    }
    public class GetEmployeeDepartmentsDto
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

        public IEnumerable<GetDepartmentDto> Departments { get; set; }
    }
}