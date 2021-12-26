using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class User: AuditableEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public bool Verified { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLogin { get; set; }
        public ICollection<UserActivity> UserActivities { get; set; }
    }
}
