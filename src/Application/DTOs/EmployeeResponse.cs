using System;

namespace ShareLoanApp.Application.DTOs
{
    public class CreateEmployeeDtoResponse : CreateEmployeeDto
    {
        public Guid Id { get; set; }
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
}
