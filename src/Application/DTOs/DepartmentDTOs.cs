using Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShareLoanApp.Application.DTOs
{
    public class CreateDepartmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GetDepartmentDto : CreateDepartmentDto
    {
        public Guid Id { get; set; }
    }

    public class UpdateDepartmentDto : CreateDepartmentDto
    {

    }

    public class DeleteDepartmentDto : CreateDepartmentDto
    {

    }
    public class GetDepartmentEmployeesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<GetEmployeeDto> Employees { get; set; }
    }
}