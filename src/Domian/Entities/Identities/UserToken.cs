using System;
using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identities
{
	public class UserToken : IdentityUserToken<Guid>, IAuditableEntity
    {
        // note this is a foreign key 
        public override Guid UserId { get; set; }
        public string Token { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public User User { get; set; }
    }

}
