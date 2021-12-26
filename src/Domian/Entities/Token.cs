using System;
using Domain.Entities.Identities;

namespace ShareLoanApp.Domain.Entities
{
    public class Tokens
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; }
    }
}
