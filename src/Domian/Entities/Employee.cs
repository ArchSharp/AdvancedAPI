using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class Employee: AuditableEntity
    {
        public Employee()
        {
            Departments = new List<Department>();
        }
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String StaffId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        // Navigation properties
        public ICollection<Department> Departments { get; set; }

        //public Guid DepartmentIds { get; set; }
    }
}