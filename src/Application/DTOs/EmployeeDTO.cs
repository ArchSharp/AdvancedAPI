using Domain.Entities;
using System;

namespace ShareLoanApp.Application.DTOs
{
    public class CreateEmployeeDto
    {
        public String Name { get; set; }
        public String StaffId { get; set; }
        public Guid DepartmentId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String State { get; set; }
    }

    public class UpdateEmployeeDto : CreateEmployeeDto
    {
        
    }

    public class GetEmployeeDto
    {
        public Guid Id { get; set; }
    }
    public class CreateDepartmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}